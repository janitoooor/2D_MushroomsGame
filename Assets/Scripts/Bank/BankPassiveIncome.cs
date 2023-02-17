namespace Assets.Scripts
{
    class BankPassiveIncome
    {
        public delegate void BankPassiveIncomeChanged(long coins);
        public event BankPassiveIncomeChanged PassiveIncomeChanged;

        private static readonly BankPassiveIncome s_bankPassiveIncome = new();

        private long _passiveCoins;
        public long PassiveIncomeCoins { get => _passiveCoins; }

        public void IncreasePassiveIncome(long coins)
        {
            _passiveCoins += coins;
            PassiveIncomeChanged?.Invoke(coins);
        }

        public void DecreasePassiveIncome(long coins)
        {
            _passiveCoins += coins;
            PassiveIncomeChanged?.Invoke(coins);
        }

        public void LoadPassiveIncome(long passiveIncome)
        {
            _passiveCoins = passiveIncome;
        }

        public static BankPassiveIncome GetInstance()
        {
            return s_bankPassiveIncome;
        }
    }
}
