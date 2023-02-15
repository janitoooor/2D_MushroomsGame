﻿using Assets.Scripts;
using Assets.Scripts.Enumes;
using Assets.Scripts.Shop;
using Assets.Scripts.StoreItem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemBooster : Items
{
    private ItemImage _itemBoosterButtonImage;
    private ItemButton _itemBoosterButton;

    private readonly Store _store = Store.GetInstance();
    private readonly BankBalance _bankBalance = BankBalance.GetInstance();

    [Space]
    [Multiline]
    [SerializeField] private List<string> _nameTextsLvl;
    [Space]
    [SerializeField] private List<GameObject> _iconLvls;
    [Space]
    [SerializeField] private List<long> _pricesLvls;
    [Space]
    [SerializeField] private int _passiveIncome;
    [Space]
    [SerializeField] private List<long> _passiveIncomeLvls;

    private ItemText _itemPriceText;
    private ItemText _itemBoosterNameText;
    private ItemText _itemIncomeText;

    private long _price;

    private int _indexLvl;
    public int IndexLvl { get => _indexLvl; }

    public int IndexBooster { get => _indexItem; }
    private bool _maxLvlBoster { get => _indexLvl >= _pricesLvls.Count; }

    public long BoostPrice { get => _price; }
    public int BoostPassiveIncome { get => _passiveIncome; }

    private bool _itemIsLocked = false;

    private void Awake()
    {
        AddAndGetComponents();
        SetSubscriptions();
    }

    private void Start()
    {
        SetButton();
        SetFont();
        ChangeBoosterPriceText();
        ChangeIncomeText(_passiveIncomeLvls[_indexLvl]);
    }

    private void OnDestroy()
    {
        RemoveAllSubcriptions();
    }

    private void SetSubscriptions()
    {
        _bankBalance.BalanceSetNewBalance += LockItemBooster;
        _bankBalance.BalanceSetNewBalance += UnlockItemBooster;
    }

    private void RemoveAllSubcriptions()
    {
        _bankBalance.BalanceSetNewBalance -= LockItemBooster;
        _bankBalance.BalanceSetNewBalance -= UnlockItemBooster;
        _itemBoosterButton.RemoveAllListeners();
    }

    private void AddAndGetComponents()
    {
        gameObject.AddComponent<ItemButton>();
        gameObject.AddComponent<ItemImage>();

        Transform itemName = transform.GetChild(2);
        Transform priceText = transform.GetChild(3);
        Transform incomeText = transform.GetChild(4);

        priceText.gameObject.AddComponent<ItemText>();
        itemName.gameObject.AddComponent<ItemText>();
        incomeText.gameObject.AddComponent<ItemText>();

        _itemPriceText = priceText.GetComponent<ItemText>();
        _itemBoosterNameText = itemName.GetComponent<ItemText>();
        _itemIncomeText = incomeText.GetComponent<ItemText>();

        _itemBoosterButtonImage = GetComponent<ItemImage>();
        _itemBoosterButton = GetComponent<ItemButton>();
        _audioSource = GameObject.Find(_audiosourceObjectName).GetComponent<AudioSource>();
    }

    private void SetFont()
    {
        _itemPriceText.ChangeFontText(_font);
        _itemBoosterNameText.ChangeFontText(_font);
    }

    private void SetButton()
    {
        _itemBoosterButton.AddListeners(BuyBooster);
        _itemBoosterButton.AddListeners(PlayOneShot);
        SetNewValue();
        LockItemBooster(0);
    }

    private void BuyBooster()
    {
        _store.BuyBooster(this);
        ChangeBoosterToNewLvlAfterBuy();
    }

    private void ChangeBoosterPriceText()
    {
        _itemPriceText.ChangeText(CoyntingSystemUpdate(_price) + "<sprite index=" + 0 + ">");
    }

    private void ChangeIncomeText(long income)
    {
        _itemIncomeText.ChangeText(CoyntingSystemUpdate(income) + "<sprite index=" + 0 + ">");
    }

    private void ChangeBoosterToNewLvlAfterBuy()
    {
        _isCreated = true;
        _indexLvl++;
        DeactivateGameObject();
        SetNewValue();
        ChangeBoosterPriceText();
        ChangeIncomeText(_passiveIncomeLvls[1]);
    }

    private void SetNewValue()
    {
        if (!_maxLvlBoster && gameObject != null)
        {
            _iconLvls[_indexLvl].gameObject.SetActive(true);
            for (int i = 0; i < _iconLvls.Count; i++)
            {
                if (_iconLvls[i] != _iconLvls[_indexLvl])
                    _iconLvls[i].gameObject.SetActive(false);
            }

            _price = _pricesLvls[_indexLvl];
            _itemBoosterNameText.ChangeText(_nameTextsLvl[1]);
        }
    }

    private void SetLockItem(Color color, bool interactable)
    {
        _itemBoosterButtonImage.ChangeImageColor(color);
        _itemBoosterButton.ChangeButtonInteractable(interactable);
    }

    private void LockItemBooster(long currentBankBalance)
    {
        if (!_itemIsLocked && _price > currentBankBalance)
        {
            SetLockItem(Color.grey, false);
            _itemIsLocked = true;
        }
    }

    private void UnlockItemBooster(long currentBankBalance)
    {
        if (_itemIsLocked && _price <= currentBankBalance && !_maxLvlBoster)
        {
            SetLockItem(Color.white, true);
            _itemIsLocked = false;
        }
    }

    private void DeactivateGameObject()
    {
        if (_maxLvlBoster)
        {
            gameObject.SetActive(false);
            _bankBalance.BalanceSetNewBalance -= LockItemBooster;
            _bankBalance.BalanceSetNewBalance -= UnlockItemBooster;
        }
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
}