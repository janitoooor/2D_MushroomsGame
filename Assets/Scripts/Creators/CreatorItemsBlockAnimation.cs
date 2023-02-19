using Assets.Scripts.StoreItem;
using System.Collections.Generic;
using UnityEngine;

public class CreatorItemsBlockAnimation : CreatorItems
{
    [SerializeField] private List<ItemBlockAnimation> _itemsBlock;
    [SerializeField] private Transform _transform;
    private void Awake()
    {
        InstantiateStoreItems(_itemsBlock, _transform);
    }

    private void Start()
    {
        CreatorItemsInStore.Instance.StoreItemsCreated += ActiveItemsInStart;
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
        {
            if (item.IndexItem < _createdItems.Count && _createdItems[item.IndexItem] != null)
            {
                if (!_createdItems[item.IndexItem].IsCreated && !item.ItemIsHidden)
                    _createdItems[item.IndexItem].gameObject.SetActive(true);
            }
        }
    }

    private protected override void ActiveItems(int index)
    {
        if (index < _createdItems.Count)
        {
            if (_createdItems[index] != null)
            {
                if (!_createdItems[index].gameObject.activeInHierarchy)
                    _createdItems[index].gameObject.SetActive(true);
            }
        }
    }
}
