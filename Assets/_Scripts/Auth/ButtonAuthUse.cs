using Assets.Scripts.Buttonss.ButtonsAdv;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Auth
{
    public class ButtonAuthUse : ButtonBonus
    {
        [SerializeField] private AuthBonus _authBonus;
        [SerializeField] private TextMeshProUGUI _textArea;
        [Space]
        [Multiline]
        [SerializeField] private string _textWarning;

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
        SaveSystem.Instance.Auth();
#endif
            _textArea.text = _textWarning;
        }
    }
}
