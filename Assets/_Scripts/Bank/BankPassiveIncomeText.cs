using System;
using TMPro;
using UnityEngine;

class BankPassiveIncomeText : MonoBehaviour
{
    [Space]
    [SerializeField] private TextMeshProUGUI _passiveIncomeText;
    [Space]
    [SerializeField] private TMP_SpriteAsset _spriteAsset;
    [SerializeField] private TMP_FontAsset _font;
    [Space]
    [SerializeField] private float _timeToChange = 0.7f;

    private readonly BankPassiveIncome _bankPassiveIncome = BankPassiveIncome.GetInstance();

    private long _currentPassiveIncome;

    private void Start()
    {
        JsonSaveSystem.Instance.LoadBalance();
        SetStartText();
    }

    private void OnEnable()
    {
        _bankPassiveIncome.PassiveIncomeChanged += UpdatePassiveIncomeText;
    }

    private void OnDisable()
    {
        _bankPassiveIncome.PassiveIncomeChanged -= UpdatePassiveIncomeText;
    }

    private void SetStartText()
    {
        _passiveIncomeText.font = _font;
        _passiveIncomeText.spriteAsset = _spriteAsset;
        UpdatePassiveIncomeText(_bankPassiveIncome.PassiveIncomeCoins);
    }

    private void UpdatePassiveIncomeText(long amount)
    {
        long oldPassiveIncome = _currentPassiveIncome;
        _currentPassiveIncome += amount;

        LeanTween.value(_passiveIncomeText.gameObject, oldPassiveIncome, _currentPassiveIncome, _timeToChange)
            .setOnUpdate((float val) =>
            {
                _passiveIncomeText.text = CoyntingSystemUpdate((long)val);
            });
    }

    private string CoyntingSystemUpdate(long passiveIncome)
    {
        if (passiveIncome < 1000)
            return $"{passiveIncome} " + "<sprite index=" + 0 + ">" + "/с";

        int power = (int)(Math.Log(passiveIncome) / Math.Log(1000));
        int maxPower = Enum.GetValues(typeof(BigNumbersUnit)).Length - 1;
        if (power > maxPower)
            return $"{long.MaxValue} " + "<sprite index=" + 0 + ">" + "/с";

        return string.Format("{0:0.0#} {1} ", passiveIncome / Math.Pow(1000, power), Enum.GetName(typeof(BigNumbersUnit), power)) + "<sprite index=" + 0 + ">" + "/с";
    }
}
