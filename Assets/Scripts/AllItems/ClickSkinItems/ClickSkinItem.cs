using Assets.Scripts.AllItems.ClickSkinItems;
using Assets.Scripts.Shop;
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

    private void Awake()
    {
        GetComponents();
    }

    private void Start()
    {
        SetButton();
        LockItem();

        _gemBank.GemBankSetsNewBalance += ChangeButtonImageIfHaveGems;
        BuyStartItem();
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
            UnlockItem();
            _itemIsBuying = true;
            _button.onClick.RemoveListener(BuyItem);
            _button.onClick.AddListener(SelectItem);
            _skinItemStore.SkinItemSelectedInStore += UnSelectItem;
            _gemBank.GemBankSetsNewBalance -= ChangeButtonImageIfHaveGems;
            SelectItem();
        }
    }
    private void RemoveAllSubcriptions()
    {
        _skinItemStore.SkinItemSelectedInStore -= UnSelectItem;
        _gemBank.GemBankSetsNewBalance -= ChangeButtonImageIfHaveGems;
        _button.onClick.RemoveAllListeners();
    }

    private void SetButton()
    {
        _button.onClick.AddListener(BuyItem);
        _button.onClick.AddListener(PlayOneShot);
    }

    private void SelectItem()
    {
        if (_itemIsBuying)
        {
            ChangeSelectItem(false, true, false);
            _skinItemStore.SetSelectedItem(this);
        }
    }

    private void UnSelectItem(ClickSkinItem skinItem)
    {
        if (skinItem != this)
            ChangeSelectItem(true, false, true);
    }
}
