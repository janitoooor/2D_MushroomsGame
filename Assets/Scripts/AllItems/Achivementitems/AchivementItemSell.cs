namespace Assets.Scripts.Items.Achivementitems
{
    class AchivementItemSell : AchivementItem
    {
        public delegate void AchiveCreated(AchivementItemSell achivementItemSell);
        public event AchiveCreated AchivesCreated;

        private readonly Store _store = Store.GetInstance();

        private long _currentAmountItemsSell;
        public long CurrentAmountItemsSell { get => _currentAmountItemsSell; }

        private void Start()
        {
            JsonSaveSystem.Instance.LoadAchivesSell(this);
            SetSubscriptions();
            LockAchivement();
            ChangeCurrentStateText(_currentAmountItemsSell);
        }

        public void LoadData(long amountItemsSell)
        {
            _currentAmountItemsSell = amountItemsSell;
        }

        private void ChangeStateAchivementAfterSellItem(int indexItem, long desiredAmount)
        {
            if (_indexItem == indexItem)
            {
                _currentAmountItemsSell += desiredAmount;
                ChangeCurrentStateText(_currentAmountItemsSell);
                JsonSaveSystem.Instance.SaveAchivesSell(this);
                if (_currentAmountItemsSell >= _goal)
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
