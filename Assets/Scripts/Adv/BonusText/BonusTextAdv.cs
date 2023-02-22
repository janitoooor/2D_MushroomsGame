using Assets.Scripts;
using Assets.Scripts.Enumes;
using System;
using TMPro;
using UnityEngine;

public class BonusTextAdv : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textBonus;

    [SerializeField] private int _modifyBonus = 50;
    public int ModifyBonus { get => _modifyBonus; }

    private readonly BankPassiveIncome _bankPassiveIncome = BankPassiveIncome.GetInstance();

    private void Awake()
    {
        _textBonus = GetComponent<TextMeshProUGUI>();

    }

    private void OnEnable()
    {
        ChangeTextBonus(_bankPassiveIncome.PassiveIncomeCoins);
    }

    private int ChangeModifyBonus()
    {
        return UnityEngine.Random.Range(_modifyBonus, _modifyBonus * 2);
    }

    private void ChangeTextBonus(long passiveIncome)
    {
        _textBonus.text = CoyntingSystemUpdate(passiveIncome * ChangeModifyBonus());
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
