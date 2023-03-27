using System.Collections;
using System.Runtime.InteropServices;
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
        private bool _stopSave;

        private long _maxBalance;
        public long MaxBalance { get => _maxBalance; }

        private readonly float _timerAutoSave = 3f;

        [DllImport("__Internal")]
        private static extern void SetToLeaderboard(long value);

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
            JsonSaveSystem.Instance.SaveBalance();
        }

        public void LoadCoinsBalance(long coinsBalance)
        {
            _coinsBalance = coinsBalance;
        }

        public void LoadMaxBalance(long maxBalance)
        {
            _maxBalance = maxBalance;
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
            _stopSave = true;
            Coroutines.StopRoutine(TimerSaveBalance());
        }

        private IEnumerator TimerSaveBalance()
        {
            yield return new WaitForSeconds(_timerAutoSave);
            if (!_stopSave)
            {
                JsonSaveSystem.Instance.SaveBalance();
#if !UNITY_EDITOR && UNITY_WEBGL
            if (_maxBalance < _coinsBalance && AuthBonus.Instance.GemsAdded)
            {
                _maxBalance = _coinsBalance;
                SetToLeaderboard(_maxBalance);
                JsonSaveSystem.Instance.SaveMaxBalance();
            }
#endif
                Coroutines.StartRoutine(TimerSaveBalance());
            }
        }
    }
}
