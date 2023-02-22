using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Buttonss.ButtonsAdv
{
    public abstract class ButtonAdv : MonoBehaviour
    {
        [SerializeField] private protected GameObject _layerToClose;
        [SerializeField] private protected Button _button;
        [SerializeField] private protected AudioClip _audioClip;
        [SerializeField] private protected GameObject _layerToOpen;

        private readonly protected string _nameAnimationTriggerClose = "CloseObject";
        private readonly protected string _audiosourceObjectName = "AudioSource";

        private protected AudioSource _audioSource;
        private protected Animator _animatorLayer;

        public virtual void GetComponents()
        {
            _audioSource = GameObject.Find(_audiosourceObjectName).GetComponent<AudioSource>();
            _animatorLayer = _layerToClose.GetComponent<Animator>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(PlayOneShot);
        }

        public virtual void CloseLayer()
        {
            _animatorLayer.SetTrigger(_nameAnimationTriggerClose);
            _layerToOpen.SetActive(true);
        }

        private void PlayOneShot()
        {
            _audioSource.PlayOneShot(_audioClip);
        }
    }
}
