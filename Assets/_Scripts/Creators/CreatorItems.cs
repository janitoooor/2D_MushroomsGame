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
        if (ItemCanActive(indexItem))
        {
            _createdItems[_indexNumberInList].gameObject.SetActive(true);
            _indexNumberInList++;
            return;
        }

        _store.BuyItemsIsMades -= ActiveItems;
    }

    private protected virtual bool ItemCanActive(int indexItem)
    {
        bool isCorrectIndex = _indexNumberInList < _createdItems.Count;
        bool secondPreviousItemIsCreated = indexItem == _createdItems[_indexNumberInList - 2].IndexItem;
        bool previousItemIsCreated = indexItem == _createdItems[_indexNumberInList - 1].IndexItem;
        bool isDeactive = !_createdItems[_indexNumberInList].gameObject.activeInHierarchy;

        return isCorrectIndex && (secondPreviousItemIsCreated || previousItemIsCreated) && isDeactive;
    }
}
