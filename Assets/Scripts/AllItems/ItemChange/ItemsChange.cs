using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

class ItemsChange : Items
{
    [SerializeField] private long _priceCoinsToChange;
    [SerializeField] private long _priceGemsToChange;

    private Button _button;

    private readonly GemBank _gemBank = GemBank.GetInstance();
    private readonly BankBalance _bankBalance = BankBalance.GetInstance();

    private void Start()
    {
        SetStartComponents();
        ChekBalanceAndChangeLock(_bankBalance.CoinsBalance);
    }

    private void OnDestroy()
    {
        _bankBalance.BalanceSetNewBalance -= ChekBalanceAndChangeLock;
    }

    private void SetStartComponents()
    {
        _audioSource = GameObject.Find(_audiosourceObjectName).GetComponent<AudioSource>();
        SetButton();
        _bankBalance.BalanceSetNewBalance += ChekBalanceAndChangeLock;
    }

    private void SetButton()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlayOneShot);
        _button.onClick.AddListener(ChangeCoinsToGems);
    }

    private void ChangeCoinsToGems()
    {
        if (_bankBalance.CoinsBalance >= _priceCoinsToChange)
        {
            _bankBalance.WithdrawCoins(_priceCoinsToChange);
            _gemBank.AddGems(_priceGemsToChange);
        }
    }

    private void ChangeLockItem(Color buttonColor, bool buttonInteractable)
    {
        _button.image.color = buttonColor;
        _button.interactable = buttonInteractable;
    }

    private void ChekBalanceAndChangeLock(long bankBalance)
    {
        if (bankBalance >= _priceCoinsToChange)
            ChangeLockItem(Color.white, true);
        else
            ChangeLockItem(Color.black, false);
    }
}
