using UnityEngine;

namespace Assets.Scripts
{
    class UpdateCoinsBalance : MonoBehaviour
    {
        private readonly BankBalance _bankBalance = BankBalance.GetInstance();
        private readonly BankPassiveIncome _bankPassiveIncome = BankPassiveIncome.GetInstance();

        private float _timeToChange = 1f;

        private void Update()
        {
            if (_bankPassiveIncome.PassiveIncomeCoins >= 1 / Time.deltaTime)
                _bankBalance.AddCoins((long)(_bankPassiveIncome.PassiveIncomeCoins * (double)Time.deltaTime));
            else
                UpdateBalance();
        }

        private void UpdateBalance()
        {
            _timeToChange -= Time.deltaTime;

            if (_timeToChange <= 0 && _bankPassiveIncome.PassiveIncomeCoins >= 1)
            {
                _bankBalance.AddCoins(1);
                _timeToChange = 1f / _bankPassiveIncome.PassiveIncomeCoins;
            }
        }
    }
}
