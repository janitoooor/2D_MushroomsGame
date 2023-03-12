using UnityEngine;
using UnityEngine.UI;
using TMPro;

    public abstract class AbstractButtons : MonoBehaviour
    {
        [SerializeField] protected private Sprite _buttonSelectedSprite;
        [SerializeField] protected private AudioClip _audioClip;
        [SerializeField] protected private TMP_FontAsset _font;

        private protected readonly string _nameAnimationTriggerClose = "CloseObject";
        private protected readonly string _audiosourceObjectName = "AudioSource";
        protected private AudioSource _audioSource;

        protected private TextMeshProUGUI _textButton;
        protected private Sprite _baseSprite;
        protected private Button _button;

        public virtual void OnClick()
        {
            _audioSource.PlayOneShot(_audioClip);
            ShowButtonSelected();
        }

        private protected void SetFont()
        {
            _textButton = GetComponentInChildren<TextMeshProUGUI>();
            _textButton.font = _font;
        }

        public virtual void AddListeners()
        {
            _button.onClick.AddListener(OnClick);
        }

        public virtual void RemoveAllListeners()
        {
            _button.onClick.RemoveAllListeners();
        }

        public virtual void GetComponents()
        {
            _button = GetComponent<Button>();
            _audioSource = GameObject.Find(_audiosourceObjectName).GetComponent<AudioSource>();
            _baseSprite = _button.image.sprite;
        }

        public virtual void ShowButtonSelected(long none = 0)
        {

        }

        public virtual void SetButtonPressedState()
        {
            _button.image.sprite = _buttonSelectedSprite;
            _button.interactable = false;

            try
            {
                _textButton.color = Color.white;
            }
            catch
            {

            }
        }
        public virtual void DisabledButtonPressedState()
        {
            _button.image.sprite = _baseSprite;
            _button.interactable = true;
            try
            {
                _textButton.color = Color.black;
            }
            catch
            {

            }
    }
}
