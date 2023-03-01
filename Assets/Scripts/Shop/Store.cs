using Assets.Scripts;
using UnityEngine;

public class Store
{
    public delegate void SayDesiredAmountItemAfterBuy(long desiredAmount, int index);
    public event SayDesiredAmountItemAfterBuy SayDesiredAmountAfterBuy;

    public delegate void BuyBoosterIsMade(int indexBooster, long passiveIncome);
    public event BuyBoosterIsMade BuyBoosterIsMades;

    public delegate void BuyItemsIsMade(int indexItem);
    public event BuyItemsIsMade BuyItemsIsMades;

    public delegate void SellItemIsMade(int indexItem, long desiredAmount);
    public event SellItemIsMade SellItemsIsMades;

    public delegate void BoosterSetNewLvl(int lvlBoosterm, int indexBooster);
    public event BoosterSetNewLvl BoosterSetNewLevels;

    public delegate void BuyItemIsMadeBlockCanCreate(long currentAmount, int indexItem);
    public event BuyItemIsMadeBlockCanCreate BuyItemIsMadeBlockCanCreates;

    public delegate void SellItemIsMadeBlockCanCreate(long currentAmount, int indexItem);
    public event SellItemIsMadeBlockCanCreate SellItemIsMadeBlockCanCreates;

    public delegate void PressButtonBuy(long balance);
    public event PressButtonBuy PressedButtonBuy;

    public delegate void PressButtonSell(long amount);
    public event PressButtonSell PressedButtonSell;

    public delegate void PressButtonAmount(long amount);
    public event PressButtonAmount PressedButtonAmount;

    private readonly static Store s_Store = new();

    private readonly BankBalance _bankBalance = BankBalance.GetInstance();
    private readonly BankPassiveIncome _bankPassiveIncome = BankPassiveIncome.GetInstance();

    private long _buttonAmountType;
    public long ButtonAmountType { get => _buttonAmountType; }

    private bool _pressedBuy;
    private bool _pressedSell;

    public bool PressedBuy { get => _pressedBuy; }
    public bool PressedSell { get => _pressedSell; }

    public static Store GetInstance()
    {
        return s_Store;
    }

    public void ChangePressedButton(bool pressedBuy, bool pressedSell)
    {
        _pressedBuy = pressedBuy;
        _pressedSell = pressedSell;
    }

    public void BuyItem(StoreItemsObject storeItem)
    {
        BuyAddItemStats(storeItem);

        if (storeItem != null)
        {
            BuyItemsIsMades?.Invoke(storeItem.IndexItem);
            BuyItemIsMadeBlockCanCreates?.Invoke(storeItem.ItemCurrentAmount, storeItem.IndexItem);
            SayDesiredAmountAfterBuy?.Invoke(storeItem.DesiredAmount, storeItem.IndexItem);
        }
    }

    public void ChangePassiveIncomeCurrentAmount(StoreItemsObject storeItem)
    {
        _bankPassiveIncome.IncreasePassiveIncome(storeItem.ItemPassiveIncome * storeItem.ItemCurrentAmount);
    }

    public void SellItem(StoreItemsObject storeItem)
    {
        SellAddItemsStats(storeItem);

        if (storeItem != null)
        {
            SellItemIsMadeBlockCanCreates?.Invoke(storeItem.ItemCurrentAmount, storeItem.IndexItem);
            SellItemsIsMades?.Invoke(storeItem.IndexItem, storeItem.DesiredAmount);
        }
    }

    public void GiveStoreItemBoosterLvl(ItemBooster itemBooster)
    {
        BoosterSetNewLevels?.Invoke(itemBooster.IndexLvl, itemBooster.IndexItem);
    }

    public void BuyBooster(ItemBooster itemBooster)
    {
        BuyAddBoosterStats(itemBooster);
        BuyBoosterIsMades?.Invoke(itemBooster.IndexItem, itemBooster.BoostPassiveIncome);
        BoosterSetNewLevels?.Invoke(itemBooster.IndexLvl, itemBooster.IndexBooster);
    }

    public void ButtonSellIsPressed()
    {
        PressedButtonSell?.Invoke(_buttonAmountType);
    }

    public void ButtonBuyIsPressed()
    {
        PressedButtonBuy?.Invoke(_bankBalance.CoinsBalance);
    }

    public void TakeButtonAmount(long amount)
    {
        _buttonAmountType = amount;
        PressedButtonAmount?.Invoke(amount);
    }

    private void BuyAddBoosterStats(ItemBooster itemBooster)
    {
        _bankBalance.WithdrawCoins(itemBooster.BoostPrice);
    }

    private void BuyAddItemStats(StoreItemsObject storeItem)
    {
        _bankPassiveIncome.IncreasePassiveIncome(storeItem.ItemPassiveIncome * storeItem.DesiredAmount);
        _bankBalance.WithdrawCoins(storeItem.ItemPrice);
    }

    private void SellAddItemsStats(StoreItemsObject storeItem)
    {
        _bankPassiveIncome.DecreasePassiveIncome(-storeItem.ItemPassiveIncome * storeItem.DesiredAmount);
        _bankBalance.AddCoins(Mathf.RoundToInt(storeItem.ItemPrice));
    }
}
