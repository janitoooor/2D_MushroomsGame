using System.Collections.Generic;
using UnityEngine;

public class CreatorItemsBlockAnimation : CreatorItems
{
    [SerializeField] private List<ItemBlockAnimation> _itemsBlock;
    [SerializeField] private Transform _transform;

    private void Start()
    {
        InstantiateStoreItems(_itemsBlock, _transform);
    }

    private void OnEnable()
    {
        _store.BuyItemsIsMades += ActiveItems;
    }

    private void OnDisable()
    {
        _store.BuyItemsIsMades -= ActiveItems;
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
