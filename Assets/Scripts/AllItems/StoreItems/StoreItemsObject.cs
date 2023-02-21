using UnityEngine;
using Assets.Scripts.StoreItem;
using System;
using Assets.Scripts.Enumes;
using Assets.Scripts.Buttonss.StoreButtons;
using Assets.Scripts;
using Assets.Scripts.Shop;

public class StoreItemsObject : Items
{
    private readonly Store _store = Store.GetInstance();
    private readonly BankBalance _bankBalance = BankBalance.GetInstance();
    private readonly Menu _menu = Menu.GetInstance();
    private readonly BoostersAndItems _upgratesAndItems = BoostersAndItems.GetInstance();

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

    public long ItemPrice { get => _itemPrice; }
    public long ItemPassiveIncome { get => _itemPasssiveIncome; }

    public long ItemCurrentAmount { get => _itemValue.CurrentAmount; }

    public long DesiredAmount { get => _itemValue.DesiredAmount; }

    private bool _itemIsHidden = false;
    public bool ItemIsHidden { get => _itemIsHidden; }

    private long _currentBankBalance;
    private long _startPrice;

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
        if (_store.PressedBuy && _bankBalance.CoinsBalance >= _itemPrice)
        {
            _itemValue.ChangeCurrentAmount(_itemValue.DesiredAmount);
            _store.BuyItem(this);
            _startPrice += Mathf.RoundToInt(_startPrice * 0.3f);
            _itemValue.IfBuyChangePriceDependOfDesiredAmount(_startPrice, ref _itemPrice, _itemValue.DesiredAmount);
            _itemPriceText.ChangeText(UpdateTextDesiredAmountAndPrice());
            _itemAmountText.ChangeText(CoyntingSystemUpdate(_itemValue.CurrentAmount));
            LockItemInBuy(_currentBankBalance);
            JsonSaveSystem.Instance.SaveItems(this);
        }
    }

    public void SellItem()
    {
        if (_store.PressedSell && _itemValue.CurrentAmount >= 1)
        {
            _itemValue.ChangeCurrentAmount(-_itemValue.DesiredAmount);
            _store.SellItem(this);
            _startPrice -= Mathf.RoundToInt(_startPrice * 0.1f);
            _itemValue.IfSellChangePriceDependOfDesiredAmountIfBuy(_startPrice, ref _itemPrice, _itemValue.DesiredAmount);
            _itemPriceText.ChangeText(UpdateTextDesiredAmountAndPrice());
            _itemAmountText.ChangeText(CoyntingSystemUpdate(_itemValue.CurrentAmount));
            LockItemInSell(_itemValue.DesiredAmount);
            JsonSaveSystem.Instance.SaveItems(this);
        }
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

        _upgratesAndItems.ButtonItemsPressed += UnHiddenItem;
        _upgratesAndItems.ButtonItemsPressed += LockItemInBuy;
        _upgratesAndItems.ButtonItemsPressed += UnLockItemInBuy;
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

        _upgratesAndItems.ButtonItemsPressed -= UnHiddenItem;
        _upgratesAndItems.ButtonItemsPressed -= LockItemInBuy;
        _upgratesAndItems.ButtonItemsPressed -= UnLockItemInBuy;

        _itemButton.RemoveAllListeners();
    }

    private void SetStartOptions()
    {
        SetFont();
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

    private void SetFont()
    {
        _itemAmountText.ChangeFontText(_font);
        _itemNameText.ChangeFontText(_font);
        _itemPriceText.ChangeFontText(_font);
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
        if (index == _indexItem)
        {
            _store.ChangePassiveIncomeCurrentAmount(this);
            _itemPasssiveIncome *= passiveIncome;
        }
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
        if (!_itemIsHidden)
        {
            ChangeHiddenItem(true, false, "???", Color.black, false, false);
            _itemPriceText.ChangeText(UpdateTextDesiredAmountAndPrice());
            _itemAmountText.ChangeText("");
        }
    }

    private void UnHiddenItem(long balance)
    {
        if (!_store.PressedSell && !_itemIsHidden)
        {
            if (_bankBalance.CoinsBalance >= _itemPrice || ItemCurrentAmount >= 1)
            {
                ChangeHiddenItem(false, true, _itemName, Color.white, true, true);
                _itemPriceText.ChangeText(UpdateTextDesiredAmountAndPrice());
                _itemAmountText.ChangeText($"{ItemCurrentAmount}");
            }

            _bankBalance.BalanceChanged -= UnHiddenItem;
            _upgratesAndItems.ButtonItemsPressed -= UnHiddenItem;
            _menu.ButtonStorePressed -= UnHiddenItem;
        }
    }

    private void ChangeLockItem(Color color, bool interactable)
    {
        _itemImageButton.ChangeImageColor(color);
        _itemButton.ChangeButtonInteractable(interactable);
    }

    private void LockItemInBuy(long balance)
    {
        if (!_store.PressedSell && _store.PressedBuy && _itemIsHidden)
        {
            if (_bankBalance.CoinsBalance < _itemPrice)
                ChangeLockItem(Color.black, false);
        }
    }

    private void UnLockItemInBuy(long balance)
    {
        if (!_store.PressedSell && _store.PressedBuy && _itemIsHidden)
        {
            if (_bankBalance.CoinsBalance >= _itemPrice)
                ChangeLockItem(Color.white, true);
        }
    }

    private void LockItemInSell(long desiredAmout)
    {
        Color colorLockInSell = Color.Lerp(Color.red, Color.blue, 0.5f);

        if (!_store.PressedBuy && _store.PressedSell && _itemIsHidden)
            if (_itemValue.CurrentAmount < _itemValue.DesiredAmount)
                ChangeLockItem(colorLockInSell, false);
    }

    private void UnLockItemInSell(long desiredAmout)
    {
        if (!_store.PressedBuy && _store.PressedSell && _itemIsHidden)
            if (_itemValue.CurrentAmount >= _itemValue.DesiredAmount)
                ChangeLockItem(Color.red, true);
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
            _itemValue.IfBuyChangePriceDependOfDesiredAmount(_startPrice, ref _itemPrice, _itemValue.DesiredAmount);
        else if (_store.PressedSell)
            _itemValue.IfSellChangePriceDependOfDesiredAmountIfBuy(_startPrice, ref _itemPrice, _itemValue.DesiredAmount);

        _itemPriceText.ChangeText(UpdateTextDesiredAmountAndPrice());

        if (_store.PressedBuy)
        {
            LockItemInBuy(_currentBankBalance);
            UnLockItemInBuy(_currentBankBalance);
        }
        else if (_store.PressedSell)
        {
            LockItemInSell(_itemValue.DesiredAmount);
            UnLockItemInSell(_itemValue.DesiredAmount);
        }
    }

    private string UpdateTextDesiredAmountAndPrice()
    {
        return "x" + CoyntingSystemUpdate(_itemValue.DesiredAmount) + "  " + CoyntingSystemUpdate(_itemPrice) + "<sprite index=" + 0 + ">";
    }

    private string CoyntingSystemUpdate(long value)
    {
        if (value < 1000)
            return $"{value}";

        int power = (int)(Math.Log(value) / Math.Log(1000));
        int maxPower = Enum.GetValues(typeof(BigNumbersUnit)).Length - 1;
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
