using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrestigLayer : MonoBehaviour
{
    [SerializeField] private GameObject _layer;
    [SerializeField] private List<GameObject> _allLayers;
    [SerializeField] private Button _buttonPrestig;

    private float _timeToWaitButton = 5;

    private readonly BankBalance _bankBalance = BankBalance.GetInstance();

    private void Start()
    {
        _bankBalance.BalanceSetNewBalance += RestartGame;

        SetActiveAllLayers(true, false);
        _buttonPrestig.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _bankBalance.BalanceSetNewBalance -= RestartGame;
    }

    private void SetActiveAllLayers(bool allLayers, bool prestigLayer)
    {
        foreach (var item in _allLayers)
            item.SetActive(allLayers);

        _layer.SetActive(prestigLayer);
    }

    private void RestartGame(long balance)
    {
        if (balance >= long.MaxValue || balance < 0)
        {
            SetActiveAllLayers(false, true);
            StartCoroutine(OpenButton());
        }
    }

    private IEnumerator OpenButton()
    {
        yield return new WaitForSeconds(_timeToWaitButton);
        _buttonPrestig.gameObject.SetActive(true);
    }
}
