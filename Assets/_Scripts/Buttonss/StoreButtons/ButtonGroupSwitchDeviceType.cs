using UnityEngine;

public class ButtonGroupSwitchDeviceType : MonoBehaviour
{
    [SerializeField] private Yandex _yandex;
    [Space]
    [Header("Desktop")]
    [SerializeField] private GameObject _buttonsDesktop;
    [Space]
    [Header("Mobile")]
    [SerializeField] private GameObject _buttonsMobile;

    private DeviceTypeWeb _currentDeviceType;

    private void Awake()
    {
        _yandex.OnGetTypeDevice += Yandex_OnGetTypeDevice;
    }

    private void Yandex_OnGetTypeDevice(object sender, System.EventArgs e)
    {
        _currentDeviceType = _yandex.CurrentDeviceType;

    }

    private void OnEnable()
    {
        switch (_currentDeviceType)
        {
            case DeviceTypeWeb.Desktop:
                OnDeivceTypeDescktop();
                break;
            case DeviceTypeWeb.Mobile:
                OnDeviceTypeMobile();
                break;
            default:
                OnDeviceTypeMobile();
                break;
        }
    }

    private void OnDeivceTypeDescktop()
    {
        SetActiveGroupGameobject(true, false);
    }

    private void OnDeviceTypeMobile()
    {
        SetActiveGroupGameobject(false, true);
    }

    private void SetActiveGroupGameobject(bool desctop, bool handheld)
    {
        _buttonsDesktop.SetActive(desctop);
        _buttonsMobile.SetActive(handheld);
    }
}
