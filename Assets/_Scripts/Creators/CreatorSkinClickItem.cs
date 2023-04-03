using System.Collections.Generic;
using UnityEngine;

class CreatorSkinClickItem : CreatorItems
{
    [SerializeField] private List<ClickSkinItem> _skinItems;
    [SerializeField] private Transform _transform;

    private readonly List<ClickSkinItem> _createdSkinItems = new();
    public List<ClickSkinItem> CreatedSkinItems { get => _createdSkinItems; }

    private void Awake()
    {

        CreateAllItems();
    }
    private void CreateAllItems()
    {
        foreach (var item in _skinItems)
        {
            var obj = Instantiate(item, _transform.parent);
            obj.gameObject.SetActive(true);
            _createdSkinItems.Add(obj);
        }
    }
}
