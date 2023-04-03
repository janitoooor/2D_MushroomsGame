using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

    class ButtonRestartScene : MonoBehaviour
    {
        public static ButtonRestartScene Instance;
        
        public delegate void RestartGame();

        public event RestartGame RestartsGame;

        private Button _button;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
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
            RestartsGame?.Invoke();
        }
    }
