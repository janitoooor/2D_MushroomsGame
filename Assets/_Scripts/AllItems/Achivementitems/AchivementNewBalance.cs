namespace Assets.Scripts.Items.Achivementitems
{
    class AchivementNewBalance : AchivementItem
    {
        public delegate void AchiveCreated(AchivementNewBalance achivementNewBalance);
        public event AchiveCreated AchivesCreated;

        private readonly BankBalance _bankBalance = BankBalance.GetInstance();

        private long _currentBalance;

        private void Start()
        {
            SetSubscriptions();
            LockAchivement();
        }

        private protected override void RemoveAllSubscriptions()
        {
            _bankBalance.BalanceSetNewBalance -= ChangeStateAchivementAfterNewBalance;
        }

        private protected override void GetValueOnClickUnlockedItem()
        {
            base.GetValueOnClickUnlockedItem();
            AchivesCreated?.Invoke(this);
        }

        private protected override void SetSubscriptions()
        {
            _bankBalance.BalanceSetNewBalance += ChangeStateAchivementAfterNewBalance;
        }

        private void ChangeStateAchivementAfterNewBalance(long balance)
        {
            _currentBalance = balance;
            ChangeCurrentStateText(_currentBalance);
            if (_currentBalance >= _goal)
            {
                UnlockAchivement();
                _bankBalance.BalanceChanged -= ChangeStateAchivementAfterNewBalance;
            }
        }
    }
}
