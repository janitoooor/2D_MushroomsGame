using TMPro;
using UnityEngine;

public class Items : MonoBehaviour
{
    [SerializeField] protected private int _indexItem;
    [Space]
    [SerializeField] protected private TMP_FontAsset _font;

    private protected AudioSource _audioSource;
    private protected readonly string _audiosourceObjectName = "AudioSource";
    private protected bool _isCreated;
    public bool IsCreated { get => _isCreated; }

    [SerializeField] protected private AudioClip _audioClip;

    public int IndexItem { get => _indexItem; }

    public void PlayOneShot()
    {
        _audioSource.PlayOneShot(_audioClip);
    }
}
