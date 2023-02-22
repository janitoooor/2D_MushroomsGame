namespace Assets.Scripts.Buttonss.ButtonsAdv
{
    public class ButtonAdvOpen : ButtonAdv
    {
        private void Awake()
        {
            GetComponents();
        }

        public override void GetComponents()
        {
            base.GetComponents();
            _button.onClick.AddListener(CloseLayer);
        }
    }
}
