using System.Collections.Generic;
using UnityEngine;

class CreatorItemBooster : CreatorItems
{
    public static CreatorItemBooster Instance { get; private set; }

    [SerializeField] private List<ItemBooster> _boostItems;
    [SerializeField] private Transform _transform;

    private readonly List<ItemBooster> _createdItemsBooster = new();
    public List<ItemBooster> CreatedItemsBooster { get => _createdItemsBooster; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

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

    private void OnDestroy()
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
            if (ItemCanActive(item))
                _createdItems[item.IndexItem].gameObject.SetActive(true);
    }

    private bool ItemCanActive(StoreItemsObject item)
    {
        bool itemNotNull = item.IndexItem < _createdItems.Count && _createdItems[item.IndexItem] != null;
        bool isMaxLvlBooster = _createdItemsBooster[item.IndexItem].IsMaxLvlBooster;
        bool isHidden = _createdItems[item.IndexItem].IsCreated && item.ItemIsHidden;
        return itemNotNull && !isMaxLvlBooster && !isHidden;
    }

    private protected override bool ItemCanActive(int index)
    {
        bool itemNotNull = index < _createdItems.Count && _createdItems[index] != null;
        bool isMaxLvlBooster = _createdItemsBooster[index].IsMaxLvlBooster;
        bool isCreated = _createdItems[index].IsCreated;
        return itemNotNull && !isMaxLvlBooster && !isCreated;
    }

    private protected override void ActiveItems(int index)
    {
        if (ItemCanActive(index))
        {
            _createdItems[index].gameObject.SetActive(true);
            return;
        }

        _store.BuyItemsIsMades -= ActiveItems;
    }
}
