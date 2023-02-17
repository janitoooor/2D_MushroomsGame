using System.Globalization;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GemBanks
{
    class GemBalanceText : MonoBehaviour
    {
        [Space]
        [SerializeField] private TextMeshProUGUI _gemBalanceText;
        [Space]
        [SerializeField] private TMP_FontAsset _font;
        [Space]
        [SerializeField] private float _timeToChange = 0.5f;

        private readonly GemBank _gemBank = GemBank.GetInstance();

        private void Start()
        {
            _gemBalanceText.text = CoyntingSystemUpdate(_gemBank.GemsBalance);
            _gemBalanceText.font = _font;
        }

        private void OnEnable()
        {
            _gemBank.GemBankSetsNewBalance += UpdateBalanceText;
        }

        private void OnDisable()
        {
            _gemBank.GemBankSetsNewBalance -= UpdateBalanceText;
        }

        private void UpdateBalanceText(long newBalance, long oldBalance)
        {
            LeanTween.value(_gemBalanceText.gameObject, oldBalance, newBalance, _timeToChange)
            .setOnUpdate((float val) =>
            {
                _gemBalanceText.text = CoyntingSystemUpdate((long)val);
            });
        }

        private string CoyntingSystemUpdate(long balance)
        {
            return balance.ToString("#,0", CultureInfo.InvariantCulture);
        }
    }
}
