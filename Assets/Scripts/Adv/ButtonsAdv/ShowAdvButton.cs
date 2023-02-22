using Assets.Scripts;
using Assets.Scripts.Buttonss.ButtonsAdv;
using UnityEngine;

public class ShowAdvButton : ButtonAdv
{
    private readonly BankPassiveIncome _bankPassiveIncome = BankPassiveIncome.GetInstance();

    [Space]
    [SerializeField] private BonusTextAdv _bonusTextAdv;
    [Space]
    [SerializeField] private Yandex _yandex;

    private void Awake()
    {
        GetComponents();
        _button.onClick.AddListener(ShowAdvOnClick);
        _button.onClick.AddListener(CloseLayer);
    }

    private void ShowAdvOnClick()
    {
        _yandex.ShowAddButton((_bankPassiveIncome.PassiveIncomeCoins + 1) * _bonusTextAdv.ModifyBonus);
    }
}
