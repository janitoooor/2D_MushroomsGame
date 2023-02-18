namespace Assets.Scripts.Items.Achivementitems
{
    class AchivementItemSell : AchivementItem
    {
        public delegate void AchiveCreated(AchivementItemSell achivementItemSell);
        public event AchiveCreated AchivesCreated;

        private readonly Store _store = Store.GetInstance();

        private long _currentItemAmountAfterSell;

        private void ChangeStateAchivementAfterSellItem(int indexItem, long desiredAmount)
        {
            if (_indexItem == indexItem)
            {
                _currentItemAmountAfterSell += desiredAmount;
                ChangeCurrentStateText(_currentItemAmountAfterSell);
                if (_currentItemAmountAfterSell >= _goal)
                {
                    UnlockAchivement();
                    _store.SellItemsIsMades -= ChangeStateAchivementAfterSellItem;
                }
            }
        }

        private protected override void GetValueOnClickUnlockedItem()
        {
            base.GetValueOnClickUnlockedItem();
            AchivesCreated?.Invoke(this);
        }

        private protected override void RemoveAllSubscriptions()
        {
            _store.SellItemsIsMades -= ChangeStateAchivementAfterSellItem;
        }

        private protected override void SetSubscriptions()
        {
            _store.SellItemsIsMades += ChangeStateAchivementAfterSellItem;
        }
    }
}
