using Assets.Scripts;
using System;
using TMPro;
using UnityEngine;

public class BonusTextAdv : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textBonus;

    [SerializeField] private int _minBonus = 50;

    private long _modifyBonus;
    public long ModifyBonus { get => _modifyBonus; }

    private readonly BankPassiveIncome _bankPassiveIncome = BankPassiveIncome.GetInstance();

    private void Awake()
    {
        _textBonus = GetComponent<TextMeshProUGUI>();

    }

    private void OnEnable()
    {
        ChangeTextBonus(_bankPassiveIncome.PassiveIncomeCoins);
    }

    private void ChangeModifyBonus()
    {
        _modifyBonus = UnityEngine.Random.Range(_minBonus, _minBonus * 2);
    }

    private void ChangeTextBonus(long passiveIncome)
    {
        ChangeModifyBonus();
        _textBonus.text = CoyntingSystemUpdate((passiveIncome + 1) * _modifyBonus);
    }

    private string CoyntingSystemUpdate(long passiveIncome)
    {
        if (passiveIncome < 1000)
            return $"Награда: {passiveIncome} " + "<sprite index=" + 0 + ">";

        int power = (int)(Math.Log(passiveIncome) / Math.Log(1000));
        int maxPower = Enum.GetValues(typeof(BigNumbersUnit)).Length - 1;
        if (power > maxPower)
            return $"Награда: {long.MaxValue} " + "<sprite index=" + 0 + ">";

        return string.Format("Награда: {0:0.0#} {1} ", passiveIncome / Math.Pow(1000, power), Enum.GetName(typeof(BigNumbersUnit), power)) + "<sprite index=" + 0 + ">";
    }
}
