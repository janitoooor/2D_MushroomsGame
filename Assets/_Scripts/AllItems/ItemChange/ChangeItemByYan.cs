using UnityEngine;
using UnityEngine.UI;

public class ChangeItemByYan : AbstractButtons
{
    [SerializeField] private InApp _inApp;
    [Header("Choose: 200, 600, 2000")]
    [SerializeField] private long _itemAmountGems;

    private void Awake()
    {
        GetComponents();
        AddListeners();
    }

    public override void OnClick()
    {
        base.OnClick();
        BuyGems();
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
