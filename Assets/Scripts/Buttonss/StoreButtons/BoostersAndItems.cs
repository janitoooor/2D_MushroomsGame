namespace Assets.Scripts.Buttonss.StoreButtons
{
    class BoostersAndItems
    {
        private static readonly BoostersAndItems _upgratesAndItems = new BoostersAndItems();
        private readonly BankBalance _bankBalance = BankBalance.GetInstance();

        public delegate void PressedButtonBoostersOrItems(ButtonBoostersAndItems button);
        public event PressedButtonBoostersOrItems ButtonBoostersOrItemsPressed;

        public delegate void PressedButtonItems(long balance);
        public event PressedButtonItems ButtonItemsPressed;

        public static BoostersAndItems GetInstance()
        {
            return _upgratesAndItems;
        }

        public void SetPressedButon(ButtonBoostersAndItems button)
        {
            ButtonBoostersOrItemsPressed?.Invoke(button);
            ButtonItemsPressed?.Invoke(_bankBalance.CoinsBalance);
        }
    }
}
