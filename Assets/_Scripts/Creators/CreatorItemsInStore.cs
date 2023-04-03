using System.Collections.Generic;
using UnityEngine;

class CreatorItemsInStore : CreatorItems
{
    public static CreatorItemsInStore Instance { get; private set; }

    public delegate void StoreItemsIsCreated();
    public event StoreItemsIsCreated StoreItemsCreated;

    [SerializeField] private List<StoreItemsObject> _storeItems;
    [SerializeField] private Transform _transform;
    public List<StoreItemsObject> CreatedItems { get => _createdStoreItems; }
    private readonly List<StoreItemsObject> _createdStoreItems = new();

    private readonly BankBalance _bankBalance = BankBalance.GetInstance();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    private void Start()
    {
        InstantiateStoreItems(_storeItems, _transform);
        ActiveStartItems();
        AddCreatedItemsInPublicList();
    }

    private void AddCreatedItemsInPublicList()
    {
        foreach (var item in _createdItems)
            _createdStoreItems.Add((StoreItemsObject)item);
        StoreItemsCreated?.Invoke();
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
        for (int i = 0; i < _createdStoreItems.Count - 1; i++)
            if (ItemCanActive(bankBalance, i))
                _createdItems[i + 1].gameObject.SetActive(true);
    }

    private bool ItemCanActive(long bankBalance, int i)
    {
        bool isNotActive = !_createdStoreItems[i + 1].gameObject.activeInHierarchy;
        bool isNotCreated = !_createdStoreItems[i + 1].IsCreated;
        bool isCanBuy = bankBalance >= _createdStoreItems[i].ItemPrice;
        bool isOverOne = _createdStoreItems[i].ItemCurrentAmount >= 1;
        return isNotActive && isNotCreated && (isCanBuy || isOverOne);
    }

}
