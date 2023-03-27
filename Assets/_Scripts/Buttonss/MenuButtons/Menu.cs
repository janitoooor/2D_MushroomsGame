using Assets.Scripts.Buttons.StoreButtons;

namespace Assets.Scripts.Buttonss.StoreButtons
{
    class Menu
    {
        private readonly static Menu _menu = new();
        private readonly BankBalance _bankBalance = BankBalance.GetInstance();

        public delegate void PressButtonMenu(ButtonsMenu buttonMenu);
        public event PressButtonMenu ButtonMenuPressed;

        public delegate void PressButtonStore(long balance);
        public event PressButtonStore ButtonStorePressed;

        public static Menu GetInstance()
        {
            return _menu;
        }

        public void SetPressedButon(ButtonsMenu buttonMenu)
        {
            ButtonMenuPressed?.Invoke(buttonMenu);
            ButtonStorePressed?.Invoke(_bankBalance.CoinsBalance);
        }
    }
}
