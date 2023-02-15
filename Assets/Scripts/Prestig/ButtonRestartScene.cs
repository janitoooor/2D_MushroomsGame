using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Buttonss.PrestigButton
{
    class ButtonRestartScene : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            SetButton();
        }

        private void SetButton()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(RestartScene);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
