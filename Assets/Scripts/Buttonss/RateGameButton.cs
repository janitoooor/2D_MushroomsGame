using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Buttonss
{
    class RateGameButton : MonoBehaviour
    {
        [SerializeField] private Yandex _yandex;
        [SerializeField] private Button _button;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private GameObject _playerIcon;

        private AudioSource _audioSource;
        private protected readonly string _audiosourceObjectName = "AudioSource";

        private void Awake()
        {
            _audioSource = GameObject.Find(_audiosourceObjectName).GetComponent<AudioSource>();
        }

        private void Start()
        {
            AddButtonListeners();
        }

        private void RateGame()
        {
            _yandex.RateGameOnButton();
            _yandex.SetPlayerNameAndPhoto();
            gameObject.SetActive(false);
            _playerIcon.SetActive(true);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void AddButtonListeners()
        {
            _button.onClick.AddListener(RateGame);
            _button.onClick.AddListener(PlayeOneShot);
        }

        private void PlayeOneShot()
        {
            _audioSource.PlayOneShot(_audioClip);
        }
    }
}
