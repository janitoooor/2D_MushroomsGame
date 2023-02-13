using Assets.Scripts.Shop;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.StoreItem
{
    class CreatorsItemsInStore : CreatorItems
    {
        [SerializeField] private List<StoreItemsObject> _storeItems;
        [SerializeField] private Transform _transform;
        public List<StoreItemsObject> CreatedItems { get => _storeItems; }

        private void Start()
        {
            InstantiateStoreItems(_storeItems, _transform);
            ActiveStartItems();
        }

        private void OnEnable()
        {
            _store.BuyItemsIsMades += ActiveItems;
        }

        private void OnDisable()
        {
            _store.BuyItemsIsMades -= ActiveItems;
        }
    }
}
