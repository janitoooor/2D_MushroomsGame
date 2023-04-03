using Assets.Scripts;
using UnityEngine;

public class ShowAdvButton : ButtonBonus
{
    private readonly BankPassiveIncome _bankPassiveIncome = BankPassiveIncome.GetInstance();

    [Space]
    [SerializeField] private BonusTextAdv _bonusTextAdv;
    [Space]
    [SerializeField] private AdvShow _advShow;

    private void Awake()
    {
        GetComponents();
        _button.onClick.AddListener(ShowAdvOnClick);
        _button.onClick.AddListener(CloseLayer);
    }

    private void ShowAdvOnClick()
    {
        _advShow.ShowAddButton((_bankPassiveIncome.PassiveIncomeCoins + 1) * _bonusTextAdv.ModifyBonus);
    }
}
