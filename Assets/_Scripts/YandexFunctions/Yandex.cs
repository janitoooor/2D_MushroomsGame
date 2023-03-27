using Assets.Scripts;
using Assets.Scripts.Buttonss.ButtonsAdv;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Yandex : MonoBehaviour
{
    public event EventHandler OnGetTypeDevice;

    [SerializeField] private RawImage _userImage;
    [SerializeField] private TextMeshProUGUI _userName;

    [DllImport("__Internal")]
    private static extern void ShowAdv();

    [DllImport("__Internal")]
    private static extern void GetPlayerData();

    [DllImport("__Internal")]
    private static extern void RateGame();

    [DllImport("__Internal")]
    private static extern void GetTypeDevice();

    public DeviceTypeWeb CurrentDeviceType { get; private set; } = DeviceTypeWeb.Mobile;

    private void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        ShowAdv();
        GetTypeDevice();
#endif
    }

    public void SetPlayerNameAndPhoto()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        GetPlayerData();
#endif
    }

    public void RateGameOnButton()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        RateGame();
#endif
    }

    public void SetTargetDeviceType(string deviceType)
    {
        switch (deviceType)
        {
            case "desktop":
                CurrentDeviceType = DeviceTypeWeb.Desktop;
                break;
            case "mobile":
                CurrentDeviceType = DeviceTypeWeb.Mobile;
                break;
            default:
                CurrentDeviceType = DeviceTypeWeb.Mobile;
                break;
        }

        OnGetTypeDevice?.Invoke(this, EventArgs.Empty);
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
