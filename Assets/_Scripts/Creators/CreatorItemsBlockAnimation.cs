using System.Collections.Generic;
using UnityEngine;

public class CreatorItemsBlockAnimation : CreatorItems
{
    [SerializeField] private List<ItemBlockAnimation> _itemsBlock;
    [SerializeField] private Transform _transform;
    private void Start()
    {
        CreatorItemsInStore.Instance.StoreItemsCreated += ActiveItemsInStart;
        InstantiateStoreItems(_itemsBlock, _transform);
    }
    private void OnEnable()
    {
        _store.BuyItemsIsMades += ActiveItems;
    }

    private void OnDisable()
    {
        _store.BuyItemsIsMades -= ActiveItems;
        CreatorItemsInStore.Instance.StoreItemsCreated -= ActiveItemsInStart;
    }

    private void ActiveItemsInStart()
    {
        foreach (var item in CreatorItemsInStore.Instance.CreatedItems)
            if (ItemCanActive(item))
                _createdItems[item.IndexItem].gameObject.SetActive(true);
    }

    private bool ItemCanActive(StoreItemsObject item)
    {
        bool isNotNull = item.IndexItem < _createdItems.Count && _createdItems[item.IndexItem] != null;
        bool isNotCreated = !_createdItems[item.IndexItem].IsCreated;

        return isNotNull && isNotCreated && item.ItemIsHidden;
    }

    private protected override void ActiveItems(int index)
    {
        if (ItemCanActive(index))
            _createdItems[index].gameObject.SetActive(true);
    }

    private protected override bool ItemCanActive(int index)
    {
        bool isDeactive = !_createdItems[index].gameObject.activeInHierarchy;
        bool isNotNull = index < _createdItems.Count && _createdItems[index] != null;
        return isDeactive && isNotNull;
    }
}
