﻿using System.Globalization;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    class BankBalanceText : MonoBehaviour
    {
        [Space]
        [SerializeField] private TextMeshProUGUI _balanceText;
        [Space]
        [SerializeField] private TMP_FontAsset _font;
        [Space]
        [SerializeField] private float _timeToChange = 0.3f;

        private readonly BankBalance _bankBalance = BankBalance.GetInstance();

        private void Start()
        {
            UpdateBalanceText(_bankBalance.CoinsBalance, 0);
            _balanceText.font = _font;
        }

        private void OnEnable()
        {
            _bankBalance.BalanceSetOldBalance += UpdateBalanceText;
        }

        private void OnDisable()
        {
            _bankBalance.BalanceSetOldBalance -= UpdateBalanceText;
        }

        private void UpdateBalanceText(long newBalance, long oldBalance)
        {
            if (newBalance >= long.MaxValue || newBalance < 0)
            {
                _balanceText.text = CoyntingSystemUpdate(long.MaxValue);
                return;
            }

            if (newBalance - oldBalance < 0)
            {
                LeanTween.value(_balanceText.gameObject, oldBalance, newBalance, _timeToChange)
                .setOnUpdate((float val) =>
                {
                    _balanceText.text = CoyntingSystemUpdate(newBalance);
                });
            }
            else
            {
                _balanceText.text = CoyntingSystemUpdate(newBalance);
            }
        }

        private string CoyntingSystemUpdate(long balance)
        {
            return "<sprite index=" + 0 + ">" + balance.ToString("#,0", CultureInfo.InvariantCulture);
        }
    }
}