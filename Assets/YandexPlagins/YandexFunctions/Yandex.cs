using Assets.Scripts;
using Assets.Scripts.Buttonss.ButtonsAdv;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Yandex : MonoBehaviour
{
    private readonly BankBalance _bankBalance = BankBalance.GetInstance();

    [SerializeField] private RawImage _userImage;
    [SerializeField] private TextMeshProUGUI _userName;
    [Space]
    [SerializeField] private ButtonAdvOpen _buttonAdvOpen;
    [Space]
    [SerializeField] private int _timeToActivateAdv = 60;

    [DllImport("__Internal")]
    private static extern void ShowAdv();

    [DllImport("__Internal")]
    private static extern void AddCoinsExtern(long value);

    [DllImport("__Internal")]
    private static extern void GetPlayerData();

    [DllImport("__Internal")]
    private static extern void RateGame();

    private void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        ShowAdv();
# endif
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

    public void SetName(string name)
    {
        _userName.text = name;
    }

    public void SetPhoto(string url)
    {
        StartCoroutine(DownoladImage(url));
    }

    public void ShowAddButton(long value)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        AddCoinsExtern(value);
#endif
    }

    public void AddCoinsAdv(long value)
    {
        _bankBalance.AddCoins(value);
        DeactivateAdvButtonAfterAdvShow();
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

    private void DeactivateAdvButtonAfterAdvShow()
    {
        _buttonAdvOpen.gameObject.SetActive(false);
        StartCoroutine(ActiveAdvAfterShow());
    }

    private IEnumerator ActiveAdvAfterShow()
    {
        yield return new WaitForSeconds(_timeToActivateAdv);
        _buttonAdvOpen.gameObject.SetActive(true);
    }
}
