class SkinItemStore
{
    public delegate void SkinItemSelect(ClickSkinItem skinItem);
    public event SkinItemSelect SkinItemSelectedInStore;

    private static readonly SkinItemStore s_skinItemStore = new();

    public static SkinItemStore GetInstance()
    {
        return s_skinItemStore;
    }

    public void SetSelectedItem(ClickSkinItem skinItem)
    {
        SkinItemSelectedInStore?.Invoke(skinItem);
    }
}
