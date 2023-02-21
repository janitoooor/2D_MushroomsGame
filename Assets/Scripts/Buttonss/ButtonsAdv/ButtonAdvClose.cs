using Assets.Scripts.Buttonss.ButtonsAdv;

public class ButtonAdvClose : ButtonAdv
{
    private void Awake()
    {
        GetComponents();
    }

    private void Start()
    {
        _layerToClose.SetActive(false);
    }
}






