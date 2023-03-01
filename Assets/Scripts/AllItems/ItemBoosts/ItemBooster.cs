using Assets.Scripts;
using Assets.Scripts.Enumes;
using Assets.Scripts.Shop;
using Assets.Scripts.StoreItem;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [Space]
    [SerializeField] private TMP_SpriteAsset _spriteAssteHaveMoney;
    [SerializeField] private TMP_SpriteAsset _spriteAssteHaventMoney;

    private ItemText _itemPriceText;
    private ItemText _itemBoosterNameText;
    private ItemText _itemIncomeText;

    private long _price;

    private int _indexLvl = -1;
    public int IndexLvl { get => _indexLvl; }

    public int IndexBooster { get => _indexItem; }
    private bool _maxLvlBoster { get => _indexLvl >= _pricesLvls.Count - 1; }
    public bool IsMaxLvlBooster { get => _maxLvlBoster; }

    public long BoostPrice { get => _price; }
    public int BoostPassiveIncome { get => _passiveIncome; }

    private bool _itemIsLocked = false;

    private void Awake()
    {
        AddAndGetComponents();
    }
    private void Start()
    {
        SetSubscriptions();
        JsonSaveSystem.Instance.LoadBoosters(this);
        _store.GiveStoreItemBoosterLvl(this);

        if (_maxLvlBoster)
        {
            DeactivateGameObject();
            return;
        }

        SetNewValue();
        SetFont();
        SetButton();
        LockItemBooster(0);
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

    }

    private void BuyBooster()
    {
        _indexLvl++;
        _store.BuyBooster(this);
        ChangeBoosterToNewLvlAfterBuy();
        JsonSaveSystem.Instance.SaveBoosters(this);
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
        DeactivateGameObject();
        SetNewValue();
    }

    private void SetNewValue()
    {
        if (!_maxLvlBoster && gameObject != null)
        {
            _iconLvls[_indexLvl + 1].SetActive(true);
            for (int i = 0; i < _iconLvls.Count; i++)
            {
                if (_iconLvls[i] != _iconLvls[_indexLvl + 1])
                    _iconLvls[i].SetActive(false);
            }

            _price = _pricesLvls[_indexLvl + 1];
            _itemBoosterNameText.ChangeText(_nameTextsLvl[_indexLvl + 1]);
            ChangeIncomeText(_passiveIncomeLvls[_indexLvl + 1]);
            ChangeBoosterPriceText();
        }
    }

    private void SetLockItem(Color color, bool interactable, TMP_SpriteAsset spriteAsset)
    {
        _itemBoosterButtonImage.ChangeImageColor(color);
        _itemBoosterButton.ChangeButtonInteractable(interactable);

        _itemIncomeText.ChangeSpriteAsset(spriteAsset);
        _itemPriceText.ChangeSpriteAsset(spriteAsset);
    }

    private void LockItemBooster(long currentBankBalance)
    {
        if (!_itemIsLocked && _price > currentBankBalance)
        {
            SetLockItem(Color.grey, false, _spriteAssteHaventMoney);
            _itemIsLocked = true;
        }
    }

    private void UnlockItemBooster(long currentBankBalance)
    {
        if (_itemIsLocked && _price <= currentBankBalance && !_maxLvlBoster)
        {
            SetLockItem(Color.white, true, _spriteAssteHaveMoney);
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

    public void LoadData(int dataLvlBooster)
    {
        if (dataLvlBooster != _indexLvl)
            _indexLvl = dataLvlBooster;
    }
}
