namespace Assets.Scripts.Items.Achivementitems
{
    class AchivementItemBuy : AchivementItem
    {
        public delegate void AchiveCreated(AchivementItemBuy achivementItemBuy);
        public event AchiveCreated AchivesCreated;

        private readonly Store _store = Store.GetInstance();

        private long _currentAmountBuysItems;
        public long CurrentAmountItemsBuy { get => _currentAmountBuysItems; }

        private void Start()
        {
            JsonSaveSystem.Instance.LoadAchivesBuy(this);
            SetSubscriptions();
            LockAchivement();
            ChangeCurrentStateText(_currentAmountBuysItems);
        }

        public void LoadData(long amountBuyItems)
        {
            _currentAmountBuysItems = amountBuyItems;
        }

        private protected override void RemoveAllSubscriptions()
        {
            _store.SayDesiredAmountAfterBuy -= ChangeStateAchivementAfterBuyItem;
        }

        private protected override void SetSubscriptions()
        {
            _store.SayDesiredAmountAfterBuy += ChangeStateAchivementAfterBuyItem;
        }

        private protected override void GetValueOnClickUnlockedItem()
        {
            base.GetValueOnClickUnlockedItem();
            AchivesCreated?.Invoke(this);
        }

        private void ChangeStateAchivementAfterBuyItem(long desiredtAmount, int index)
        {
            if (_indexItem == index)
            {
                _currentAmountBuysItems += desiredtAmount;
                ChangeCurrentStateText(_currentAmountBuysItems);
                JsonSaveSystem.Instance.SaveAchivesBuy(this);
                if (_currentAmountBuysItems >= _goal)
                {
                    UnlockAchivement();
                    _store.BuyItemIsMadeBlockCanCreates -= ChangeStateAchivementAfterBuyItem;
                }
            }
        }
    }
}
