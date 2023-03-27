using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    private static int s_indexItem;
    public int IndexItem { get; private set; }

    [SerializeField] private AudioClip _soundOnClick;
    [Space]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _tutorialObject;
    [Space]
    [SerializeField] private float _timerHideObject = 5f;
    [SerializeField] private float _timerCanvasGroupAlphaTimer = 0.02f;
    [Space]
    [SerializeField] private bool _saveInJson;

    public bool IsShowed { get => _isShowed; }

    private bool _isShowed;

    private readonly string _audiosourceObjectName = "AudioSource";

    private Button _button;

    private AudioSource _audioSource;

    private void Awake()
    {
        s_indexItem++;
        IndexItem = s_indexItem;

        _audioSource = GameObject.Find(_audiosourceObjectName).GetComponent<AudioSource>();
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        if (!_saveInJson)
            return;

        _button.onClick.AddListener(() =>
            {
                StartCoroutine(CanvasGroupAlphaTimer());
                _audioSource.PlayOneShot(_soundOnClick);
            });

        JsonSaveSystem.Instance.LoadTutorialPanel(this);

        if (_isShowed)
            Hide();
        else
            Show();
    }

    private void OnEnable()
    {

        if (_tutorialObject.activeInHierarchy && _canvasGroup.alpha < 1)
            _canvasGroup.alpha = 1;

        if (_tutorialObject.activeInHierarchy && _saveInJson)
            StartCoroutine(HideObjectTimer());
    }

    private void Show()
    {
        if (_isShowed)
            return;

        _canvasGroup.alpha = 1;
        _tutorialObject.SetActive(true);

        if (gameObject.activeInHierarchy)
            StartCoroutine(HideObjectTimer());
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
        {
            _isShowed = true;
            JsonSaveSystem.Instance.SaveTutorialPanel(this);
            Hide();
        }
        else
        {
            StartCoroutine(CanvasGroupAlphaTimer());
        }
    }

    public void LoadData(bool isShowed)
    {
        _isShowed = isShowed;
    }
}
