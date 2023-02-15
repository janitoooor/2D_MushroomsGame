using Assets.Scripts.Enumes;
using Assets.Scripts.Shop;
using Assets.Scripts.StoreItem;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class AchivementItem : Items
{
    [Space]
    [SerializeField] private GameObject _unlockIcon;
    [SerializeField] private GameObject _lockIcon;
    [SerializeField] private GameObject _iconIsMade;
    [Space]
    [Multiline]
    [SerializeField] private string _description;
    [Space]
    [SerializeField] private ItemText _descriptionText;
    [SerializeField] private ItemText _currentStateText;
    [SerializeField] private ItemText _prizeAmountText;
    [Space]
    [SerializeField] private ItemImage _imageButton;
    [Space]
    [SerializeField] protected private long _goal;
    [Space]
    [SerializeField] private Color _lockColor;
    [SerializeField] private Color _unlockColor;
    [Space]
    [SerializeField] private int _amountPrize;

    private readonly GemBank _gemBank = GemBank.GetInstance();

    private Button _button;

    private void Awake()
    {
        GetComponents();
        SetButtonListeners();
    }

    private void Start()
    {
        SetSubscriptions();
        LockAchivement();
        SetFonts();
    }
    private void OnDestroy()
    {
        RemoveAllSubscriptions();
        _button.onClick.RemoveAllListeners();

    }
    private protected void ChangeCurrentStateText(long currentValue)
    {
        _currentStateText.ChangeText($"{CoyntingSystemUpdate(currentValue)} / {CoyntingSystemUpdate(_goal)}");
    }

    protected private abstract void SetSubscriptions();

    protected private abstract void RemoveAllSubscriptions();

    private void GetComponents()
    {
        _button = GetComponent<Button>();
        _audioSource = GameObject.Find(_audiosourceObjectName).GetComponent<AudioSource>();
    }

    protected private void UnlockAchivement()
    {
        ChangeStateObjectAchivement(true, false, true, false, true, _unlockColor);
        _prizeAmountText.ChangeText($"{_amountPrize}");
    }

    protected private void LockAchivement()
    {
        ChangeStateObjectAchivement(false, true, false, true, false, _lockColor);
        _descriptionText.ChangeText(_description);
        _currentStateText.ChangeText($"{0} / {_goal}");
    }

    private void SetButtonListeners()
    {
        _button.onClick.AddListener(AddGemsOnClickDeactivateObject);
        _button.onClick.AddListener(PlayOneShot);
    }

    protected private virtual void AddGemsOnClickDeactivateObject()
    {
        _gemBank.AddGems(_amountPrize);
    }

    private string CoyntingSystemUpdate(long value)
    {
        if (value < 1000)
            return $"{value}";

        int power = (int)(Math.Log(value) / Math.Log(1000));
        int maxPower = Enum.GetValues(typeof(BigNumbersUnit)).Length - 1;
        if (power > maxPower)
            return $"{long.MaxValue}";

        return string.Format("{0:0.0#} {1}", value / Math.Pow(1000, power), Enum.GetName(typeof(BigNumbersUnit), power));
    }

    private void SetFonts()
    {
        _descriptionText.ChangeFontText(_font);
        _currentStateText.ChangeFontText(_font);
    }

    private void ChangeStateObjectAchivement(bool unlockIcon, bool lockIcon, bool iconIsMade, bool currentStateText, bool buttonInteractable, Color color)
    {
        _unlockIcon.SetActive(unlockIcon);
        _lockIcon.SetActive(lockIcon);
        _iconIsMade.SetActive(iconIsMade);
        _currentStateText.gameObject.SetActive(currentStateText);
        _imageButton.ChangeImageColor(color);
        _button.interactable = buttonInteractable;
    }
}
