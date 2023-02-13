using System.Collections.Generic;
using UnityEngine;

    public abstract class CreatorItems : MonoBehaviour
    {
        private protected List<Items> _createdItems = new();

        private protected int _indexNumberInList = 2;
        private protected readonly Store _store = Store.GetInstance();

        private protected virtual void InstantiateStoreItems<T>(List<T> storeItems, Transform transform) where T : Items
        {
            foreach (T item in storeItems)
            {
                T createdItem = Instantiate(item, transform.parent);
                createdItem.gameObject.SetActive(false);
                _createdItems.Add(createdItem);
            }
        }

        private protected virtual void ActiveStartItems()
        {
            for (int i = 0; i < 2; i++)
                _createdItems[i].gameObject.SetActive(true);
        }

        private protected virtual void ActiveItems(int indexItem)
        {
            if (_indexNumberInList < _createdItems.Count)
            {
                if (indexItem == _createdItems[_indexNumberInList - 2].IndexItem || indexItem == _createdItems[_indexNumberInList - 1].IndexItem)
                {
                    if (!_createdItems[_indexNumberInList].gameObject.activeInHierarchy)
                    {
                        _createdItems[_indexNumberInList].gameObject.SetActive(true);
                        _indexNumberInList++;
                    }
                }
            }
            else
            {
                _store.BuyItemsIsMades -= ActiveItems;
            }
        }
    }
