using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.StoreItem
{
    class CreatorsItemsInStore : CreatorItems
    {
        [SerializeField] private List<StoreItemsObject> _storeItems;
        [SerializeField] private Transform _transform;
        public List<StoreItemsObject> CreatedItems { get => _createdStoreItems; }
        private List<StoreItemsObject> _createdStoreItems = new();

        private BankBalance _bankBalance = BankBalance.GetInstance();

        private void Start()
        {
            InstantiateStoreItems(_storeItems, _transform);

            foreach (var item in _createdItems)
                _createdStoreItems.Add((StoreItemsObject)item);

            ActiveStartItems();
        }

        private void OnEnable()
        {
            _store.BuyItemsIsMades += ActiveItems;
            _bankBalance.BalanceSetNewBalance += ActiveItemIfHaveBalance;
        }

        private void OnDisable()
        {
            _store.BuyItemsIsMades -= ActiveItems;
            _bankBalance.BalanceSetNewBalance -= ActiveItemIfHaveBalance;
        }

        private void ActiveItemIfHaveBalance(long bankBalance)
        {
            for (int i = 0; i < _createdStoreItems.Count - 2; i++)
            {
                if (bankBalance >= _createdStoreItems[i].ItemPrice)
                {
                    if (!_createdStoreItems[i + 1].gameObject.activeInHierarchy)
                        _createdItems[i + 1].gameObject.SetActive(true);
                    else if (!_createdStoreItems[i + 2].gameObject.activeInHierarchy)
                        _createdStoreItems[i + 2].gameObject.SetActive(true);
                }
            }
        }
    }
}
