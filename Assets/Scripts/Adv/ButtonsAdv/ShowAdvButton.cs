using Assets.Scripts;
using Assets.Scripts.Buttonss.ButtonsAdv;
using System.Collections;
using UnityEngine;

public class ShowAdvButton : ButtonAdv
{
    private readonly BankPassiveIncome _bankPassiveIncome = BankPassiveIncome.GetInstance();

    [SerializeField] private int _timeToActiveateAdv = 60;
    [Space]
    [SerializeField] private ButtonAdvOpen _buttonAdvOpen;
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
        _yandex.ShowAddButton((_bankPassiveIncome.PassiveIncomeCoins + 1)* _bonusTextAdv.ModifyBonus);
        _buttonAdvOpen.gameObject.SetActive(false);
        StartCoroutine(ActiveAdvAfterShow());
    }

    private IEnumerator ActiveAdvAfterShow()
    {
        yield return new WaitForSeconds(_timeToActiveateAdv);
        _buttonAdvOpen.gameObject.SetActive(true);
    }
}
