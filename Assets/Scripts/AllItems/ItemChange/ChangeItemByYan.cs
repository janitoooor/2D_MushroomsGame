using UnityEngine;
using UnityEngine.UI;

public class ChangeItemByYan : AbstractButtons
{
    [SerializeField] private InApp _inApp;
    [Header("Choose: 200, 600, 2000")]
    [SerializeField] private long _itemAmountGems;

    private new Button _button;

    private void Awake()
    {
        _audioSource = GameObject.Find(_audiosourceObjectName).GetComponent<AudioSource>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(BuyGems);
        _button.onClick.AddListener(PlayOneShot);
    }

    private void BuyGems()
    {
        switch (_itemAmountGems)
        {
            case 200:
                _inApp.BuyItemInButton200();
                break;
            case 600:
                _inApp.BuyItemInButton600();
                break;
            case 2000:
                _inApp.BuyItemInButton2000();
                break;
            default:
                Debug.Log($"type of ChangeItemByYan no founded!");
                break;
        }

    }
}
