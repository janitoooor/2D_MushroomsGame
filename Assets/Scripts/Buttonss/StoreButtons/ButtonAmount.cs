using Assets.Scripts.Buttons.StoreButtons;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shop
{
    class ButtonAmount : AbstractButtons
    {
        public delegate void PressButtonAmount(long amount);
        public event PressButtonAmount PressedButtonAmount;

        private readonly Store _store = Store.GetInstance();
        [SerializeField] private long _amount;

        private void Start()
        {
            SetBaseComponents();
            AddListeners();
            SetFont();
            ShowButtonSelected(1);
        }

        private void OnEnable()
        {
            _store.PressedButtonAmount += ShowButtonSelected;
            PressedButtonAmount += _store.TakeButtonAmount;
        }

        private void OnDisable()
        {
            _store.PressedButtonAmount -= ShowButtonSelected;
            PressedButtonAmount -= _store.TakeButtonAmount;
        }

        public override void OnClick()
        {
            ShowButtonSelected(_amount);
            _audioSource.PlayOneShot(_audioClip);
            PressedButtonAmount?.Invoke(_amount);
        }

        public override void ShowButtonSelected(long amount)
        {
            if (_amount == amount)
                SetButtonPressedState();
            else
                DisabledButtonPressedState();
        }
    }
}
