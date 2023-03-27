using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class InApp : MonoBehaviour
{
    [SerializeField] private long _gemAmount200 = 200;
    [SerializeField] private long _gemAmount600 = 600;
    [SerializeField] private long _gemAmount2000 = 2000;
    [Space]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private readonly GemBank _gemBank = GemBank.GetInstance();

    [DllImport("__Internal")]
    private static extern void BuyItemGems200Extern();
    [DllImport("__Internal")]
    private static extern void BuyItemGems600Extern();
    [DllImport("__Internal")]
    private static extern void BuyItemGems2000Extern();

    [DllImport("__Internal")]
    private static extern void CheckPaymentsExtern();

    public static InApp Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        StartCoroutine(CheckPayments());
    }

    public void BuyItemInButton200()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        BuyItemGems200Extern();
#endif
    }

    public void BuyItemInButton600()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        BuyItemGems600Extern();
#endif
    }

    public void BuyItemInButton2000()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        BuyItemGems2000Extern();
#endif
    }

    public void GetGemsAfterBuying200()
    {
        AddGemsPlaySound(_gemAmount200);
    }

    public void GetGemsAfterBuying600()
    {
        AddGemsPlaySound(_gemAmount600);
    }

    public void GetGemsAfterBuying2000()
    {
        AddGemsPlaySound(_gemAmount2000);
    }

    private void AddGemsPlaySound(long amountGems)
    {
        _gemBank.AddGems(amountGems);
        _audioSource.PlayOneShot(_audioClip);
    }

    private IEnumerator CheckPayments()
    {
        yield return new WaitForSeconds(1);
#if !UNITY_EDITOR && UNITY_WEBGL
        CheckPaymentsExtern();
#endif
    }
}
