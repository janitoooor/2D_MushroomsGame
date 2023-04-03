using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCloseLayer : AbstractButtons
{
    [SerializeField] private Animator _layerAnimatorToClose;
    [SerializeField] private GameObject _layerToOpen;
    [Space]
    [SerializeField] private ButtonsMenu _buttonOpenLayer;

    private void Awake()
    {
        GetComponents();
    }

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            _layerAnimatorToClose.SetTrigger(_nameAnimationTriggerClose);
            _layerToOpen.SetActive(true);

            if (_buttonOpenLayer != null)
                _buttonOpenLayer.SetActiveHomeLayer();

            OnClick();
        });
    }
}
