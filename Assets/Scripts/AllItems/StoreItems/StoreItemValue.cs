using UnityEngine;

namespace Assets.Scripts.StoreItem
{
    class StoreItemValue
    {
        private float _priceMargin = 2;

        private long _desiredAmount = 1;
        public long DesiredAmount { get => _desiredAmount; }

        private long _currentAmount;
        public long CurrentAmount { get => _currentAmount; }

        public void IfBuyChangePriceDependOfDesiredAmount(long startPrice, ref long price, long desiredAmount)
        {
            price = startPrice * desiredAmount;
        }

        public void IfSellChangePriceDependOfDesiredAmountIfBuy(long startPrice, ref long price, long desiredAmount)
        {
            price = Mathf.RoundToInt(startPrice / _priceMargin ) * desiredAmount;
        }

        public void ChangePriceToSell(long startPrice, ref long price)
        {
            price = Mathf.RoundToInt(startPrice / _priceMargin);
        }

        public void ChangePriceToBuy(long startPrice, ref long price)
        {
            price = startPrice;
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
}
