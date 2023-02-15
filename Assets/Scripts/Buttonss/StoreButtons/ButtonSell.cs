using Assets.Scripts.Buttons.StoreButtons;

namespace Assets.Scripts.Shop
{
    public class ButtonSell : AbstractButtons
    {
        private readonly Store _store = Store.GetInstance();

        public delegate void PressButtonSell();
        public event PressButtonSell PressSell;

        private void Awake()
        {
            GetComponents();
        }

        private void Start()
        {
            AddListeners();
            SetFont();
        }

        private void OnEnable()
        {
            PressSell += _store.ButtonSellIsPressed;
            _store.PressedButtonBuy += ShowButtonSelected;
        }
        private void OnDisable()
        {
            PressSell -= _store.ButtonSellIsPressed;
            _store.PressedButtonBuy -= ShowButtonSelected;
        }

        private void OnDestroy()
        {
            RemoveAllListeners();
        }

        public override void OnClick()
        {
            base.OnClick();
            _store.ChangePressedButton(false, true);
            ShowButtonSelected();
            PressSell?.Invoke();
        }

        public override void ShowButtonSelected(long none = 0)
        {
            if (_store.PressedSell)
                SetButtonPressedState();
            else
                DisabledButtonPressedState();
        }
    }
}
