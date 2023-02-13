using Assets.Scripts.Shop;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ItemBoosts
{
    class CreatorItemBooster : CreatorItems
    {
        [SerializeField] private List<ItemBooster> _boostItems;
        [SerializeField] private Transform _transform;

        private void Start()
        {
            InstantiateStoreItems(_boostItems, _transform);
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
            if (index < _createdItems.Count && _createdItems[index] != null)
            {
                if (!_createdItems[index].IsCreated)
                {
                    _createdItems[index].gameObject.SetActive(true);
                }
            }
            else
            {
                _store.BuyItemsIsMades -= ActiveItems;
            }
        }
    }
}
