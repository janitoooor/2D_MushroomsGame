﻿using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Buttonss.MenuButtons
{
    class ButtonSound : MonoBehaviour
    {
        [Range(0, 1)]
        [SerializeField] private float _volume;

        [Space]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;
        [Space]
        [SerializeField] Button _buttonSound;
        [SerializeField] Sprite _spriteSoundOff;
        [SerializeField] Sprite _spriteSoundOn;

        private bool _soundIsOff;

        private void Start()
        {
            _buttonSound = GetComponent<Button>();
            _buttonSound.onClick.AddListener(SoundChangeVolume);
            _buttonSound.onClick.AddListener(PlayOneShot);
        }

        private void SoundChangeVolume()
        {
            if (!_soundIsOff)
                SoundChangeSpriteVolumeBool(0, _spriteSoundOff, true);
            else
                SoundChangeSpriteVolumeBool(_volume, _spriteSoundOn, false);
        }

        private void SoundChangeSpriteVolumeBool(float volume, Sprite sprite, bool soundIsOff)
        {
             _audioSource.PlayOneShot(_audioClip);
            _audioSource.volume = volume;
            _buttonSound.image.sprite = sprite;
            _soundIsOff = soundIsOff;
        }

        private void PlayOneShot()
        {
            _audioSource.PlayOneShot(_audioClip);
        }
    }
}
