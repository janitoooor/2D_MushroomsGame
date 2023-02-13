namespace Assets.Scripts
{
    public class BankBalance
    {
        public delegate void BankBalanceChanged(long amount);
        public event BankBalanceChanged BalanceChanged;

        public delegate void BankBalanceSetNewBalance(long newBankBalance);
        public event BankBalanceSetNewBalance BalanceSetNewBalance;

        public delegate void BankBalanceSetOldBalance(long newBalance, long oldBalance);
        public event BankBalanceSetOldBalance BalanceSetOldBalance;

        private static readonly BankBalance s_bankBalance = new();

        private long _coinsBalance;

        public long CoinsBalance { get => _coinsBalance; }

        public void AddCoins(long amount)
        {
            long oldBalance = _coinsBalance;
            _coinsBalance += amount;
            BalanceChanged?.Invoke(amount);
            BalanceSetOldBalance?.Invoke(_coinsBalance, oldBalance) ;
            BalanceSetNewBalance?.Invoke(_coinsBalance);
        }

        public void WithdrawCoins(long amount)
        {
            long oldBalance = _coinsBalance;
            _coinsBalance -= amount;
            BalanceChanged?.Invoke(amount);
            BalanceSetOldBalance?.Invoke(_coinsBalance, oldBalance);
            BalanceSetNewBalance?.Invoke(_coinsBalance);
        }

        public static BankBalance GetInstance()
        {
            return s_bankBalance;
        }
    }
}
