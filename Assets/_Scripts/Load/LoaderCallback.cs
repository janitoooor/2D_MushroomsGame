using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    [SerializeField] private float _timeToStartLoadScene = 2f;

    private void Start()
    {
        StartCoroutine(WaitToLoadScene());
    }

    private IEnumerator WaitToLoadScene()
    {
        yield return new WaitForSeconds(_timeToStartLoadScene );
        Loader.LoaderCallback();
    }
}
