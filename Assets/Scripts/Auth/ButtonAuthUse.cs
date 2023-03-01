using Assets.Scripts.Buttonss.ButtonsAdv;
using UnityEngine;

namespace Assets.Scripts.Auth
{
    public class ButtonAuthUse : ButtonBonus
    {
        [SerializeField] private AuthBonus _authBonus;

        private void Awake()
        {
            GetComponents();
        }

        public override void GetComponents()
        {
            base.GetComponents();
            _button.onClick.AddListener(Auth);
            _button.onClick.AddListener(CloseLayer);
        }

        private void Auth()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
        JsonSaveSystem.Instance.Auth();
#endif
        }
    }
}
