public class ButtonOpenAuth : ButtonBonus
{
    private void Awake()
    {
        GetComponents();
    }

    public override void GetComponents()
    {
        base.GetComponents();
        _button.onClick.AddListener(CloseLayer);
    }
}
