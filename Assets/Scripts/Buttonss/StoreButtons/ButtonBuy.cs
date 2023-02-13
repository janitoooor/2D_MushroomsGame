using Assets.Scripts.Buttons.StoreButtons;
using UnityEngine;

namespace Assets.Scripts.Shop
{
    public class ButtonBuy : AbstractButtons
    {
        private readonly Store _store = Store.GetInstance();
        public delegate void PressButtonBuy();
        public event PressButtonBuy PressBuy;

        private void Start()
        {
            SetBaseComponents();
            AddListeners();
            SetFont();
            ShowButtonSelected();
        }

        private void OnEnable()
        {
            PressBuy += _store.ButtonBuyIsPressed;
            _store.PressedButtonSell += ShowButtonSelected;
        }

        private void OnDisable()
        {
            PressBuy -= _store.ButtonBuyIsPressed;
            _store.PressedButtonSell -= ShowButtonSelected;
        }

        public override void OnClick()
        {
            base.OnClick();
            _store.ChangePressedButton(true, false);
            ShowButtonSelected();
            PressBuy?.Invoke();
        }

        public override void ShowButtonSelected(long balance = 0)
        {
            if (_store.PressedBuy)
                SetButtonPressedState();
            else
                DisabledButtonPressedState();
        }

        public override void SetBaseComponents()
        {
            base.SetBaseComponents();
            _store.ChangePressedButton(true, false);
        }
    }
}
