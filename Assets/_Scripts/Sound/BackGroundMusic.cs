using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _audioClips;

    private AudioSource _audioSource;
    private AudioClip _playedAudioClip;
    private readonly float _timeToChekSoundEnd = 0.1f;

    private bool _isOnPause;
    private bool _isOnFocus = true;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        GetRandomAudioClip();
        _audioSource.PlayOneShot(_playedAudioClip);
        StartCoroutine(CheckSoundIsPlay());
    }
    private void OnApplicationPause(bool pause)
    {
        _isOnPause = pause;
    }

    private void OnApplicationFocus(bool focus)
    {
        _isOnFocus = focus;
    }

    public void StopPlay()
    {
        _isOnPause = true;
    }

    public void StartPlay()
    {
        _isOnPause = false;
    }

    private void GetRandomAudioClip()
    {
    GetNewAudioClip:
        AudioClip newAudioClip = _audioClips[Random.Range(0, _audioClips.Count - 1)];
        if (newAudioClip != _playedAudioClip)
            _playedAudioClip = newAudioClip;
        else
            goto GetNewAudioClip;
    }

    private IEnumerator CheckSoundIsPlay()
    {
        yield return new WaitForSeconds(_timeToChekSoundEnd);
        if (!_audioSource.isPlaying && !_isOnPause && _isOnFocus)
        {
            GetRandomAudioClip();
            _audioSource.PlayOneShot(_playedAudioClip);
        }

        if ((!_isOnFocus || _isOnPause) && _audioSource.isPlaying)
            _audioSource.Stop();

        StartCoroutine(CheckSoundIsPlay());
    }
}
