using UnityEngine;

class StoreItemValue
{
    private long _priceMargin = 2;

    private long _desiredAmount = 1;
    public long DesiredAmount { get => _desiredAmount; }

    private long _currentAmount;
    public long CurrentAmount { get => _currentAmount; }

    public void IfBuyChangePriceDependOfDesiredAmount(long startPrice, ref long price, long desiredAmount)
    {
        price = startPrice * desiredAmount;
        if (price < 0)
            price = long.MaxValue;
    }

    public void IfSellChangePriceDependOfDesiredAmountIfBuy(long startPrice, ref long price, long desiredAmount)
    {
        price = (startPrice / _priceMargin) * desiredAmount;
        if (price < 0)
            price = long.MaxValue;
    }

    public void ChangePriceToSell(long startPrice, ref long price)
    {
        price = startPrice / _priceMargin;
        if (price < 0)
            price = long.MaxValue;
    }

    public void ChangePriceToBuy(long startPrice, ref long price)
    {
        price = startPrice;
        if (price < 0)
            price = long.MaxValue;
    }

    public void ChangeCurrentAmount(long amount)
    {
        _currentAmount += amount;
    }

    public void TakeDesiredAmount(long amount)
    {
        _desiredAmount = amount;
    }
}
