using Assets.Scripts.StoreItem;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ItemBoosts
{
    class CreatorItemBooster : CreatorItems
    {
        [SerializeField] private List<ItemBooster> _boostItems;
        [SerializeField] private Transform _transform;

        private List<ItemBooster> _createdItemsBooster = new();

        private void Start()
        {
            CreatorItemsInStore.Instance.StoreItemsCreated += ActiveItemsInStart;
            InstantiateStoreItems(_boostItems, _transform);
            AddCreatedItemsInPublicList();
            ActiveItemsInStart();
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

        private void AddCreatedItemsInPublicList()
        {
            foreach (var item in _createdItems)
                _createdItemsBooster.Add((ItemBooster)item);
        }

        private void ActiveItemsInStart()
        {
            foreach (var item in CreatorItemsInStore.Instance.CreatedItems)
            {
                if (item.IndexItem < _createdItems.Count && _createdItems[item.IndexItem] != null && !_createdItemsBooster[item.IndexItem].IsMaxLvlBooster)
                {
                    if (!_createdItems[item.IndexItem].IsCreated && !item.ItemIsHidden)
                        _createdItems[item.IndexItem].gameObject.SetActive(true);
                }
            }
        }

        private protected override void ActiveItems(int index)
        {
            if (index < _createdItems.Count && _createdItems[index] != null)
            {
                if (!_createdItems[index].IsCreated && !_createdItemsBooster[index].IsMaxLvlBooster)
                    _createdItems[index].gameObject.SetActive(true);
            }
            else
            {
                _store.BuyItemsIsMades -= ActiveItems;
            }
        }
    }
}
