using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Yandex : MonoBehaviour
{
    [SerializeField] private RawImage _userImage;
    [SerializeField] private TextMeshProUGUI _userName;


    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void GetPlayerData();

    public void GetPlayerDataButton()
    {
        GetPlayerData();
    }

    public void SetName(string name)
    {
        _userName.text = name;
    }

    public void SetPhoto(string url)
    {
        StartCoroutine(DownoladImage(url));
    }

    private IEnumerator DownoladImage(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
        else
            _userImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }
}