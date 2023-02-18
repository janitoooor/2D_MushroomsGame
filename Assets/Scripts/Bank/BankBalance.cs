using System.Collections;
using UnityEngine;

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

        private readonly float _timerAutoSave = 1f;

        public long CoinsBalance { get => _coinsBalance; }

        public void AddCoins(long amount)
        {
            long oldBalance = _coinsBalance;
            _coinsBalance += amount;
            BalanceChanged?.Invoke(amount);
            BalanceSetOldBalance?.Invoke(_coinsBalance, oldBalance);
            BalanceSetNewBalance?.Invoke(_coinsBalance);
        }

        public void WithdrawCoins(long amount)
        {
            long oldBalance = _coinsBalance;
            _coinsBalance -= amount;
            BalanceChanged?.Invoke(amount);
            BalanceSetOldBalance?.Invoke(_coinsBalance, oldBalance);
            BalanceSetNewBalance?.Invoke(_coinsBalance);
            JsonBalanceSaveSystem.Instance.Save();
        }

        public void LoadCoinsBalance(long coinsBalance)
        {
            _coinsBalance = coinsBalance;
        }

        public static BankBalance GetInstance()
        {
            return s_bankBalance;
        }

        public void StartTimerSaveRoutine()
        {
            Coroutines.StartRoutine(TimerSaveBalance());
        }

        public void StopTimerSaveRoutine()
        {
            Coroutines.StopRoutine(TimerSaveBalance());
        }

        private IEnumerator TimerSaveBalance()
        {
            yield return new WaitForSeconds(_timerAutoSave);
            JsonBalanceSaveSystem.Instance.Save();
            Coroutines.StartRoutine(TimerSaveBalance());
        }
    }
}
