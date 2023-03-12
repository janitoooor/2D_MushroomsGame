using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _tutorialObject;
    [Space]
    [SerializeField] private float _timerHideObject = 5f;
    [SerializeField] private float _timerCanvasGroupAlphaTimer = 0.05f;

    private Button _button;

    private bool _isShowed;

    private static int s_indexItem;
    public int IndexItem { get; private set; }
    public bool IsShowed { get => _isShowed; }

    private void Awake()
    {
        s_indexItem++;
        IndexItem = s_indexItem;

        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            StartCoroutine(CanvasGroupAlphaTimer());
        });

        JsonSaveSystem.Instance.LoadTutorialPanel(this);

        if (_isShowed)
            Hide();
        else
            Show();
    }

    private void OnEnable()
    {
        if (_tutorialObject.activeInHierarchy)
            StartCoroutine(HideObjectTimer());
    }

    private void Show()
    {
        if (_isShowed)
            return;

        _canvasGroup.alpha = 1;
        _tutorialObject.SetActive(true);
        StartCoroutine(HideObjectTimer());
        _isShowed = true;
        JsonSaveSystem.Instance.SaveTutorialPanel(this);
    }

    private void Hide()
    {
        _tutorialObject.SetActive(false);
    }

    private IEnumerator HideObjectTimer()
    {
        yield return new WaitForSeconds(_timerHideObject);
        StartCoroutine(CanvasGroupAlphaTimer());
    }

    private IEnumerator CanvasGroupAlphaTimer()
    {
        yield return new WaitForSeconds(_timerCanvasGroupAlphaTimer);
        _canvasGroup.alpha -= _timerCanvasGroupAlphaTimer;

        if (_canvasGroup.alpha <= 0)
            Hide();
        else
            StartCoroutine(CanvasGroupAlphaTimer());
    }

    public void LoadData(bool isShowed)
    {
        _isShowed = isShowed;
    }
}
