using Assets.Scripts.Shop;
using Assets.Scripts.StoreItem;
using UnityEngine;

public class ItemStats : Items
{
    private readonly Store _store = Store.GetInstance();

    private ItemText _itemNameText;
    private ItemText _itemDescriptionText;
    private ItemImage _itemImageButton;
    [Space]
    [SerializeField] private GameObject _iconLocked;
    [SerializeField] private GameObject _iconUnlocked;
    [Space]
    [SerializeField] private Color _colorText;
    [Space]
    [SerializeField] private string _itemName;
    [Space]
    [Multiline]
    [SerializeField] private string _itemDescription;

    private void Awake()
    {
        _store.BuyItemsIsMades += UnlockItem;
    }

    private void OnDestroy()
    {
        _store.BuyItemsIsMades -= UnlockItem;
    }

    private void Start()
    {
        GetComponents();
        SetFont();
        ChangelockItem("???", "???", Color.black, Color.black, true, false);
    }

    private void GetComponents()
    {
        var name = transform.GetChild(1);
        var description = transform.GetChild(2);

        name.gameObject.AddComponent<ItemText>();
        description.gameObject.AddComponent<ItemText>();
        gameObject.AddComponent<ItemImage>();

        _itemImageButton = GetComponent<ItemImage>();
        _itemNameText = name.GetComponent<ItemText>();
        _itemDescriptionText = description.GetComponent<ItemText>();
    }

    private void SetFont()
    {
        _itemDescriptionText.ChangeFontText(_font);
        _itemNameText.ChangeFontText(_font);
    }

    private void UnlockItem(int indexItem)
    {
        if (indexItem == _indexItem)
        {
            ChangelockItem(_itemName, _itemDescription, Color.white, _colorText, false, true);
            _store.BuyItemsIsMades -= UnlockItem;
        }
    }

    private void ChangelockItem(string itemName, string itemDescription, Color colorButton, Color colorText, bool iconLocked, bool iconUnlocked)
    {
        _itemNameText.ChangeText(itemName);
        _itemNameText.ChangeColorText(colorText);

        _itemDescriptionText.ChangeColorText(Color.black);
        _itemDescriptionText.ChangeText(itemDescription);

        _itemImageButton.ChangeImageColor(colorButton);
        _iconLocked.SetActive(iconLocked);

        _iconUnlocked.SetActive(iconUnlocked);
    }
}
