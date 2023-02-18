namespace Assets.Scripts.Items.Achivementitems
{
    class AchivementItemBuy : AchivementItem
    {
        public delegate void AchiveCreated(AchivementItemBuy achivementItemBuy);
        public event AchiveCreated AchivesCreated;

        private readonly Store _store = Store.GetInstance();

        private long _currentItemAmountAfterBuy;

        private protected override void RemoveAllSubscriptions()
        {
            _store.BuyItemIsMadeBlockCanCreates -= ChangeStateAchivementAfterBuyItem;
        }

        private protected override void SetSubscriptions()
        {
            _store.BuyItemIsMadeBlockCanCreates += ChangeStateAchivementAfterBuyItem;
        }

        private protected override void GetValueOnClickUnlockedItem()
        {
            base.GetValueOnClickUnlockedItem();
            AchivesCreated?.Invoke(this);
        }

        private void ChangeStateAchivementAfterBuyItem(long currentAmount, int index)
        {
            if (_indexItem == index)
            {
                if (_currentItemAmountAfterBuy <= currentAmount)
                    _currentItemAmountAfterBuy = currentAmount;
                else
                    _currentItemAmountAfterBuy -= currentAmount;

                ChangeCurrentStateText(_currentItemAmountAfterBuy);
                if (_currentItemAmountAfterBuy >= _goal)
                {
                    UnlockAchivement();
                    _store.BuyItemIsMadeBlockCanCreates -= ChangeStateAchivementAfterBuyItem;
                }
            }
        }
    }
}
