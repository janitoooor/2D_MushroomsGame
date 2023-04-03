﻿using UnityEngine;
using System;
using TMPro;

public class StoreItemsObject : Items
{
    private readonly Store _store = Store.GetInstance();
    private readonly BankBalance _bankBalance = BankBalance.GetInstance();
    private readonly Menu _menu = Menu.GetInstance();
    private readonly BoostersAndItems _upgradesAndItems = BoostersAndItems.GetInstance();

    private ItemText _itemAmountText;
    private ItemText _itemNameText;
    private ItemText _itemPriceText;

    private StoreItemValue _itemValue = new StoreItemValue();
    private ItemButton _itemButton;
    private ItemImage _itemImageButton;

    [Space]
    [SerializeField] private GameObject _iconLocked;
    [SerializeField] private GameObject _iconUnlocked;
    [Space]
    [SerializeField] private string _itemName;
    [SerializeField] private long _itemPrice;
    [SerializeField] private long _itemPasssiveIncome;
    [Space]
    [SerializeField] private TMP_SpriteAsset _spriteAssetUnlock;
    [SerializeField] private TMP_SpriteAsset _spriteAssetLock;

    public long ItemPrice { get => _itemPrice; }
    public long ItemPassiveIncome { get => _itemPasssiveIncome; }

    public long ItemCurrentAmount { get => _itemValue.CurrentAmount; }

    public long DesiredAmount { get => _itemValue.DesiredAmount; }

    private bool _itemIsHidden = false;
    public bool ItemIsHidden { get => _itemIsHidden; }

    private long _currentBankBalance;
    private long _startPrice;

    private string _secretItemString = "???";

    private void Awake()
    {
        GetChildsComponents();
        AddComponents();
        GetComponents();
        SetSubscriptions();
        SetStartOptions();
    }

    private void Start()
    {
        if (gameObject.activeInHierarchy)
            _isCreated = true;
    }

    private void OnDestroy()
    {
        RemoveAllSubcriptions();
    }
    public void BuyItem()
    {
        if (!_store.PressedBuy || _bankBalance.CoinsBalance < _itemPrice)
            return;

        long deviderValue = 3;
        _itemValue.ChangeCurrentAmount(_itemValue.DesiredAmount);
        _store.BuyItem(this);
        _startPrice += _startPrice / deviderValue;
        _itemValue.IfBuyChangePriceDependOfDesiredAmount(_startPrice, ref _itemPrice, _itemValue.DesiredAmount);
        _itemPriceText.ChangeText(UpdateTextDesiredAmountAndPrice());
        _itemAmountText.ChangeText(CoyntingSystemUpdate(_itemValue.CurrentAmount));
        LockItemInBuy(_currentBankBalance);
        JsonSaveSystem.Instance.SaveItems(this);
    }

    public void SellItem()
    {
        if (!_store.PressedSell || _itemValue.CurrentAmount < 1)
            return;

        long deviderValue = 10;
        _itemValue.ChangeCurrentAmount(-_itemValue.DesiredAmount);
        _store.SellItem(this);
        _startPrice -= _startPrice / deviderValue;
        _itemValue.IfSellChangePriceDependOfDesiredAmountIfBuy(_startPrice, ref _itemPrice, _itemValue.DesiredAmount);
        _itemPriceText.ChangeText(UpdateTextDesiredAmountAndPrice());
        _itemAmountText.ChangeText(CoyntingSystemUpdate(_itemValue.CurrentAmount));
        LockItemInSell(_itemValue.DesiredAmount);
        JsonSaveSystem.Instance.SaveItems(this);
    }

    private void SetSubscriptions()
    {
        _store.PressedButtonBuy += LockItemInBuy;
        _store.PressedButtonBuy += UnLockItemInBuy;
        _store.PressedButtonBuy += PressedButtonBuy;
        _store.PressedButtonBuy += PressedButtonAmount;
        _store.PressedButtonBuy += UnHiddenItem;

        _store.PressedButtonSell += PressedButtonSell;
        _store.PressedButtonSell += UnLockItemInSell;
        _store.PressedButtonSell += LockItemInSell;
        _store.PressedButtonSell += PressedButtonAmount;

        _bankBalance.BalanceSetNewBalance += UnHiddenItem;
        _bankBalance.BalanceSetNewBalance += LockItemInBuy;
        _bankBalance.BalanceSetNewBalance += UnLockItemInBuy;
        _bankBalance.BalanceSetNewBalance += TakeCurrentBalance;

        _store.PressedButtonAmount += TakeDesiredAmount;
        _store.PressedButtonAmount += PressedButtonAmount;

        _store.BuyBoosterIsMades += ChangePassiveIncome;

        _menu.ButtonStorePressed += UnHiddenItem;
        _menu.ButtonStorePressed += LockItemInBuy;
        _menu.ButtonStorePressed += UnLockItemInBuy;

        _upgradesAndItems.ButtonItemsPressed += UnHiddenItem;
        _upgradesAndItems.ButtonItemsPressed += LockItemInBuy;
        _upgradesAndItems.ButtonItemsPressed += UnLockItemInBuy;
    }

    private void RemoveAllSubcriptions()
    {
        _store.PressedButtonBuy -= UnLockItemInBuy;
        _store.PressedButtonBuy -= LockItemInBuy;
        _store.PressedButtonBuy -= PressedButtonBuy;
        _store.PressedButtonBuy -= PressedButtonAmount;
        _store.PressedButtonBuy -= UnHiddenItem;

        _store.PressedButtonSell -= PressedButtonSell;
        _store.PressedButtonSell -= UnLockItemInSell;
        _store.PressedButtonSell -= LockItemInSell;
        _store.PressedButtonSell -= PressedButtonAmount;

        _bankBalance.BalanceSetNewBalance -= UnHiddenItem;
        _bankBalance.BalanceSetNewBalance -= LockItemInBuy;
        _bankBalance.BalanceSetNewBalance -= UnLockItemInBuy;
        _bankBalance.BalanceSetNewBalance -= TakeCurrentBalance;

        _store.PressedButtonAmount -= TakeDesiredAmount;
        _store.PressedButtonAmount -= PressedButtonAmount;

        _store.BuyBoosterIsMades -= ChangePassiveIncome;

        _menu.ButtonStorePressed -= UnHiddenItem;
        _menu.ButtonStorePressed -= UnLockItemInBuy;
        _menu.ButtonStorePressed -= LockItemInBuy;

        _upgradesAndItems.ButtonItemsPressed -= UnHiddenItem;
        _upgradesAndItems.ButtonItemsPressed -= LockItemInBuy;
        _upgradesAndItems.ButtonItemsPressed -= UnLockItemInBuy;

        _itemButton.RemoveAllListeners();
    }

    private void SetStartOptions()
    {
        SetTextComponents();
        SetButton();

        JsonSaveSystem.Instance.LoadItems(this);

        SetStartPrice();
        SetStartItem();
        HiddenItem();
        UnHiddenItem(_bankBalance.CoinsBalance);

        if (ItemCurrentAmount >= 1)
            gameObject.SetActive(true);
    }

    private void SetStartItem()
    {
        _iconLocked.SetActive(false);
        _iconUnlocked.SetActive(true);
        _itemNameText.ChangeText(_itemName);
        _itemImageButton.ChangeImageColor(Color.white);
        _itemButton.ChangeButtonInteractable(true);
        _itemPriceText.ChangeText(UpdateTextDesiredAmountAndPrice());
        _itemAmountText.ChangeText($"{ItemCurrentAmount}");
    }

    private void SetStartPrice()
    {
        _startPrice = _itemPrice;
        _itemValue.IfBuyChangePriceDependOfDesiredAmount(_startPrice, ref _itemPrice, 1);
    }

    private void SetTextComponents()
    {
        _itemAmountText.ChangeFontText(_font);
        _itemNameText.ChangeFontText(_font);
        _itemPriceText.ChangeFontText(_font);
        _itemPriceText.ChangeSpriteAsset(_spriteAssetLock);
    }

    private void GetChildsComponents()
    {
        var name = transform.GetChild(1);
        var price = transform.GetChild(2);
        var amount = transform.GetChild(3);

        name.gameObject.AddComponent<ItemText>();
        price.gameObject.AddComponent<ItemText>();
        amount.gameObject.AddComponent<ItemText>();

        _itemAmountText = amount.GetComponent<ItemText>();
        _itemNameText = name.GetComponent<ItemText>();
        _itemPriceText = price.GetComponent<ItemText>();
    }

    private void AddComponents()
    {
        gameObject.AddComponent<ItemButton>();
        gameObject.AddComponent<ItemImage>();
    }

    private void GetComponents()
    {
        _audioSource = GameObject.Find(_audiosourceObjectName).GetComponent<AudioSource>();
        _itemButton = GetComponent<ItemButton>();
        _itemImageButton = GetComponent<ItemImage>();
    }
    private void ChangePassiveIncome(int index, long passiveIncome)
    {
        if (index != _indexItem)
            return;

        _store.ChangePassiveIncomeCurrentAmount(this);
        _itemPasssiveIncome *= passiveIncome;
        JsonSaveSystem.Instance.SaveItems(this);
    }

    private void TakeCurrentBalance(long balanceBank)
    {
        _currentBankBalance = balanceBank;
    }

    private void SetButton()
    {
        _itemButton.AddListeners(BuyItem);
        _itemButton.AddListeners(SellItem);
        _itemButton.AddListeners(PlayOneShot);
    }

    private void ChangeHiddenItem(bool iconLocked, bool iconUnlocked, string name, Color color, bool interactable, bool isHidden)
    {
        _iconLocked.SetActive(iconLocked);
        _iconUnlocked.SetActive(iconUnlocked);
        _itemNameText.ChangeText(name);
        _itemImageButton.ChangeImageColor(color);
        _itemButton.ChangeButtonInteractable(interactable);
        _itemIsHidden = isHidden;
    }

    private void HiddenItem()
    {
        if (_itemIsHidden)
            return;

        ChangeHiddenItem(true, false, _secretItemString, Color.black, false, false);
        _itemPriceText.ChangeText(UpdateTextDesiredAmountAndPrice());
        _itemAmountText.ChangeText("");
    }

    private bool CanUnhiddenItem()
    {
        return !_store.PressedSell && !_itemIsHidden;
    }

    private void UnHiddenItem(long balance)
    {
        bool canUnhiddenInBuy = _bankBalance.CoinsBalance >= _itemPrice || ItemCurrentAmount >= 1;

        if (CanUnhiddenItem() && canUnhiddenInBuy)
        {
            ChangeHiddenItem(false, true, _itemName, Color.white, true, true);
            _itemPriceText.ChangeText(UpdateTextDesiredAmountAndPrice());
            _itemAmountText.ChangeText($"{ItemCurrentAmount}");

            _bankBalance.BalanceChanged -= UnHiddenItem;
            _upgradesAndItems.ButtonItemsPressed -= UnHiddenItem;
            _menu.ButtonStorePressed -= UnHiddenItem;
        }
    }

    private void ChangeLockItem(Color color, bool interactable, TMP_SpriteAsset spriteAsset)
    {
        _itemImageButton.ChangeImageColor(color);
        _itemButton.ChangeButtonInteractable(interactable);
        _itemPriceText.ChangeSpriteAsset(spriteAsset);
    }

    private void LockItemInBuy(long balance)
    {
        if (InBuy() && !HaveMoney())
            ChangeLockItem(Color.black, false, _spriteAssetLock);
    }

    private bool InBuy()
    {
        return !_store.PressedSell && _store.PressedBuy && _itemIsHidden;
    }

    private bool HaveMoney()
    {
        return _bankBalance.CoinsBalance >= _itemPrice;
    }

    private void UnLockItemInBuy(long balance)
    {
        if (InBuy() && HaveMoney())
            ChangeLockItem(Color.white, true, _spriteAssetUnlock);
    }

    private bool InSeLL()
    {
        return !_store.PressedBuy && _store.PressedSell && _itemIsHidden;
    }

    private bool HaveAmount()
    {
        return _itemValue.CurrentAmount >= _itemValue.DesiredAmount;
    }

    private void LockItemInSell(long desiredAmout)
    {
        Color colorLockInSell = Color.Lerp(Color.red, Color.blue, 0.5f);

        if (InSeLL() && !HaveAmount())
            ChangeLockItem(colorLockInSell, false, _spriteAssetLock);
    }

    private void UnLockItemInSell(long desiredAmout)
    {
        if (InSeLL() && HaveAmount())
            ChangeLockItem(Color.red, true, _spriteAssetUnlock);
    }

    private void PressedButtonSell(long amount = 0)
    {
        _itemValue.ChangePriceToSell(_startPrice, ref _itemPrice);
        _itemPriceText.ChangeText(UpdateTextDesiredAmountAndPrice());

    }

    private void PressedButtonBuy(long balance = 0)
    {
        _itemValue.ChangePriceToBuy(_startPrice, ref _itemPrice);
        _itemPriceText.ChangeText(UpdateTextDesiredAmountAndPrice());
    }

    private void TakeDesiredAmount(long amount)
    {
        _itemValue.TakeDesiredAmount(amount);
    }

    private void PressedButtonAmount(long none = 0)
    {
        if (_store.PressedBuy)
        {
            _itemValue.IfBuyChangePriceDependOfDesiredAmount(_startPrice, ref _itemPrice, _itemValue.DesiredAmount);
            LockItemInBuy(_currentBankBalance);
            UnLockItemInBuy(_currentBankBalance);
        }
        else if (_store.PressedSell)
        {
            _itemValue.IfSellChangePriceDependOfDesiredAmountIfBuy(_startPrice, ref _itemPrice, _itemValue.DesiredAmount);
            LockItemInSell(_itemValue.DesiredAmount);
            UnLockItemInSell(_itemValue.DesiredAmount);
        }

        _itemPriceText.ChangeText(UpdateTextDesiredAmountAndPrice());
    }

    private string UpdateTextDesiredAmountAndPrice()
    {
        return "x" + CoyntingSystemUpdate(_itemValue.DesiredAmount) + "  " + CoyntingSystemUpdate(_itemPrice) + "<sprite index=" + 0 + ">";
    }

    private string CoyntingSystemUpdate(long value)
    {
        if (value < 1000)
            return $"{value}";

        long power = (long)(Math.Log(value) / Math.Log(1000));
        long maxPower = Enum.GetValues(typeof(BigNumbersUnit)).Length - 1;
        if (power > maxPower)
            return $"{long.MaxValue}";

        return string.Format("{0:0.0#}{1}", value / Math.Pow(1000, power), Enum.GetName(typeof(BigNumbersUnit), power));
    }

    public void LoadData(long dataCurrentAmount, long dataPassiveIncome, long dataItemPrice, bool dataItemIsHidden)
    {
        _itemValue.ChangeCurrentAmount(dataCurrentAmount);

        if (dataPassiveIncome > 0)
            _itemPasssiveIncome = dataPassiveIncome;

        if (dataItemPrice > 0)
            _itemPrice = dataItemPrice;

        _itemIsHidden = dataItemIsHidden;
    }
}
