using Assets.Scripts.Buttonss.StoreButtons;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Buttons.StoreButtons
{
    class ButtonsMenu : AbstractButtons
    {
        public delegate void ButtonPressed(ButtonsMenu buttonMenu);

        public event ButtonPressed ButtonPresseds;
        private readonly Menu _menu = Menu.GetInstance();

        [SerializeField] private bool _withoutPressedState;
        [Space]
        [SerializeField] private GameObject _layer;
        [Space]
        [SerializeField] private int _indexLayer;

        private Animator _layerAnimator;

        private void Awake()
        {
            GetComponents();
        }

        private void Start()
        {
            AddListeners();

            Invoke(nameof(SetActiveHomeLayer), Time.fixedDeltaTime);
        }

        private void OnEnable()
        {
            ButtonPresseds += _menu.SetPressedButon;
            _menu.ButtonMenuPressed += CloseLayer;
        }

        private void OnDestroy()
        {
            RemoveAllListeners();
        }

        public override void GetComponents()
        {
            base.GetComponents();
            _layerAnimator = _layer.GetComponent<Animator>();
        }

        public override void RemoveAllListeners()
        {
            base.RemoveAllListeners();
            _menu.ButtonMenuPressed -= CloseLayer;
            ButtonPresseds -= _menu.SetPressedButon;
        }

        public override void OnClick()
        {
            base.OnClick();
            ButtonPresseds?.Invoke(this);
        }

        public void SetActiveHomeLayer()
        {
            if (_indexLayer == 0)
                SetButtonPressedState();
            else
                _layer.SetActive(false);
        }

        private void CloseLayer(ButtonsMenu buttonMenu)
        {
            if (buttonMenu == this)
            {
                _layer.SetActive(true);
                if (!_withoutPressedState)
                    SetButtonPressedState();
            }
            else
            {
                _layerAnimator.SetTrigger(_nameAnimationTriggerClose);
                DisabledButtonPressedState();
            }
        }
    }
}
