using Assets.Scripts.Buttonss.ButtonsAdv;

namespace Assets.Scripts.Auth
{
    public class ButtonAuthClose : ButtonBonus
    {
        private void Awake()
        {
            GetComponents();
        }

        private void Start()
        {
            _layerToClose.SetActive(false);
        }

        public override void GetComponents()
        {
            base.GetComponents();
            _button.onClick.AddListener(CloseLayer);
        }
    }
}
