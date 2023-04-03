using UnityEngine;
using UnityEngine.UI;
using TMPro;

class ItemsChange : Items
{
    [SerializeField] private long _priceCoinsToChange;
    [SerializeField] private long _priceGemsToChange;
    [Space]
    [SerializeField] private TMP_SpriteAsset _spriteAssetLock;
    [SerializeField] private TMP_SpriteAsset _spriteAssetUnlock;
    [SerializeField] private ItemText _itemPrice;

    private Button _button;

    private readonly GemBank _gemBank = GemBank.GetInstance();
    private readonly BankBalance _bankBalance = BankBalance.GetInstance();

    private void Awake()
    {
        GetComponents();
    }

    private void Start()
    {
        SetStartComponents();
        ChekBalanceAndChangeLock(_bankBalance.CoinsBalance);
    }

    private void OnDestroy()
    {
        _bankBalance.BalanceSetNewBalance -= ChekBalanceAndChangeLock;
        _button.onClick.RemoveAllListeners();
    }

    private void GetComponents()
    {
        _button = GetComponent<Button>();
        _audioSource = GameObject.Find(_audiosourceObjectName).GetComponent<AudioSource>();
    }

    private void SetStartComponents()
    {
        SetButtonListeners();
        _bankBalance.BalanceSetNewBalance += ChekBalanceAndChangeLock;
    }

    private void SetButtonListeners()
    {
        _button.onClick.AddListener(PlayOneShot);
        _button.onClick.AddListener(ChangeCoinsToGems);
    }

    private void ChangeCoinsToGems()
    {
        if (_bankBalance.CoinsBalance < _priceCoinsToChange)
            return;

        _bankBalance.WithdrawCoins(_priceCoinsToChange);
        _gemBank.AddGems(_priceGemsToChange);
    }

    private void ChangeLockItem(Color buttonColor, bool buttonInteractable, TMP_SpriteAsset spriteAsset)
    {
        _button.image.color = buttonColor;
        _button.interactable = buttonInteractable;
        _itemPrice.ChangeSpriteAsset(spriteAsset);
    }

    private void ChekBalanceAndChangeLock(long bankBalance)
    {
        if (bankBalance >= _priceCoinsToChange)
            ChangeLockItem(Color.white, true, _spriteAssetUnlock);
        else
            ChangeLockItem(Color.black, false, _spriteAssetLock);
    }
}
