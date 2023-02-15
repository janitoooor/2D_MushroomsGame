using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        _audioSource.Play();
    }
}
