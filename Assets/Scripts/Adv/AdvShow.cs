using Assets.Scripts;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class AdvShow : MonoBehaviour
{
    [SerializeField] private BackGroundMusic _backgroundMusic;
    [Space]
    [SerializeField] private GameObject _buttonAdvOpen;
    [Space]
    [SerializeField] private int _timeToActivateAdv = 60;

    private readonly Object _lockObject = new();

    private readonly BankBalance _bankBalance = BankBalance.GetInstance();

    private long _bonusValue;

    [DllImport("__Internal")]
    private static extern void AddCoinsExtern();

    public void AddCoinsAdv()
    {
        lock (_lockObject)
        {
            _bankBalance.AddCoins(_bonusValue);
            DeactivateAdvButtonAfterAdvShow();
        }
    }

    public void ShowAddButton(long value)
    {
        _bonusValue = value;
#if !UNITY_EDITOR && UNITY_WEBGL
        AddCoinsExtern();
        _backgroundMusic.StopPlay();
#endif
    }

    private void DeactivateAdvButtonAfterAdvShow()
    {
        _buttonAdvOpen.gameObject.SetActive(false);
        StartCoroutine(ActiveAdvAfterShow());
        _backgroundMusic.StartPlay();
    }

    private IEnumerator ActiveAdvAfterShow()
    {
        yield return new WaitForSeconds(_timeToActivateAdv);
        _buttonAdvOpen.SetActive(true);
    }
}
