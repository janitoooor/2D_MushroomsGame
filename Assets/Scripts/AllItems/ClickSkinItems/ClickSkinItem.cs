using Assets.Scripts.AllItems.ClickSkinItems;
using Assets.Scripts.Buttonss.PrestigButton;
using Assets.Scripts.Shop;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

class ClickSkinItem : Items
{
    [Space]
    [SerializeField] private GameObject _unlockIcon;
    [SerializeField] private GameObject _lockIcon;
    [SerializeField] private GameObject _iconIsSelect;
    [Space]
    [Multiline]
    [SerializeField] private string _description;
    [SerializeField] private int _price;
    [Space]
    [SerializeField] private float _income;
    [Space]
    [SerializeField] private ItemText _descriptionText;
    [SerializeField] private ItemText _priceText;
    [SerializeField] private ItemText _incomeText;

    public float IncomeSkinItem { get => _income; }
    private readonly GemBank _gemBank = GemBank.GetInstance();
    private readonly SkinItemStore _skinItemStore = SkinItemStore.GetInstance();

    private Button _button;

    private readonly int _indexStartItem = 777;
    private bool _itemIsBuying;
    private bool _itemSelected;

    private string _filePath;
    private SkinItemData _skinItemData = new();

    private void Awake()
    {
        _filePath = Application.persistentDataPath + "/" + $"SkinItem{name}.json";

        Load();
        GetComponents();
    }

    private void Start()
    {
        ButtonRestartScene.Instance.RestartsGame += ClearSaves;
        _gemBank.GemBankSetsNewBalance += ChangeButtonImageIfHaveGems;
        SetButton();
        LockItem();
        ChangeButtonImageIfHaveGems(_gemBank.GemsBalance);
        BuyStartItem();
        ChangeBuyingItem();
    }

    private void OnDestroy()
    {
        RemoveAllSubcriptions();
    }

    private void GetComponents()
    {
        _button = GetComponent<Button>();
        _audioSource = GameObject.Find(_audiosourceObjectName).GetComponent<AudioSource>();
    }

    private void LockItem()
    {
        ChangeLockItem(false, true, Color.black);
        SetStartText();
        _button.interactable = false;
    }

    private void ChangeButtonImageIfHaveGems(long gemBalance, long none = 0)
    {
        if (gemBalance >= _price)
        {
            _button.image.color = Color.white;
            _button.interactable = true;
        }
        else
        {
            LockItem();
        }
    }

    private void UnlockItem()
    {
        ChangeLockItem(true, false, Color.white);
    }

    private void BuyStartItem()
    {
        if (_indexItem == _indexStartItem)
            BuyItem();
    }

    private void ChangeSelectItem(bool unlockIcon, bool iconSelect, bool buttonInteractable)
    {
        _unlockIcon.SetActive(unlockIcon);
        _iconIsSelect.SetActive(iconSelect);
        _button.interactable = buttonInteractable;
    }

    private void ChangeLockItem(bool unlockIcon, bool lockIcon, Color buttonColor)
    {
        _unlockIcon.SetActive(unlockIcon);
        _lockIcon.SetActive(lockIcon);
        _iconIsSelect.SetActive(false);
        _button.image.color = buttonColor;
    }

    private void SetStartText()
    {
        _descriptionText.ChangeFontText(_font);
        _priceText.ChangeFontText(_font);
        _descriptionText.ChangeText(_description);
        _priceText.ChangeText($"{_price}");
        _incomeText.ChangeFontText(_font);
        _incomeText.ChangeText($"{_income * 100} %");
    }

    private void BuyItem()
    {
        if (_gemBank.GemsBalance >= _price)
        {
            _gemBank.WithdrawGems(_price);
            _itemIsBuying = true;
            ChangeBuyingItem();
            SelectItem();
            Save();
        }
    }

    private void ChangeBuyingItem()
    {
        if (_itemIsBuying)
        {
            UnlockItem();
            _button.onClick.RemoveListener(BuyItem);
            _button.onClick.AddListener(SelectItem);
            _skinItemStore.SkinItemSelectedInStore += UnSelectItem;
            _gemBank.GemBankSetsNewBalance -= ChangeButtonImageIfHaveGems;

            if (_itemSelected)
                SelectItem();
        }
    }

    private void RemoveAllSubcriptions()
    {
        _skinItemStore.SkinItemSelectedInStore -= UnSelectItem;
        _gemBank.GemBankSetsNewBalance -= ChangeButtonImageIfHaveGems;
        ButtonRestartScene.Instance.RestartsGame -= ClearSaves;
        _button.onClick.RemoveAllListeners();
    }

    private void SetButton()
    {
        _button.onClick.AddListener(BuyItem);
        _button.onClick.AddListener(PlayOneShot);
    }

    private void SelectItem()
    {
        _itemSelected = true;
        ChangeSelectItem(false, true, false);
        _skinItemStore.SetSelectedItem(this);
        Save();
    }

    private void UnSelectItem(ClickSkinItem skinItem)
    {
        if (skinItem != this)
            ChangeSelectItem(true, false, true);
    }

    [Serializable]
    public class SkinItemData
    {
        public bool ItemIsBuying;
        public bool ItemIsSelected;
    }

    public void Save()
    {
        _skinItemData.ItemIsBuying = _itemIsBuying;
        _skinItemData.ItemIsSelected = _itemSelected;

        string dataAsJson = JsonUtility.ToJson(_skinItemData, true);
        File.WriteAllText(_filePath, dataAsJson);
    }

    public void Load()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            _skinItemData = JsonUtility.FromJson<SkinItemData>(json);

            _itemIsBuying = _skinItemData.ItemIsBuying;
            _itemSelected = _skinItemData.ItemIsSelected;
        }
    }

    public void ClearSaves()
    {
        if (File.Exists(_filePath))
            File.Delete(_filePath);
    }

}
