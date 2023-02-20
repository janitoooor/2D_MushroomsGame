using Assets.Scripts;
using Assets.Scripts.ItemBoosts;
using Assets.Scripts.StoreItem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class ItemBlockAnimation : Items
{
    [SerializeField] private List<BoosterPrefab> _itemPrefabs;
    [SerializeField] private Transform _transform;
    [Space]
    [SerializeField] private bool _automatedPool;
    [SerializeField] private int _amountCreatedPrefabs = 20;

    private Stack<BoosterPrefab> _itemPrefabsActives = new Stack<BoosterPrefab>();

    private Queue<BoosterPrefab>[] _itemPrefabsCreatedDeActivitesLvls =
    {
            new Queue<BoosterPrefab>(),
            new Queue<BoosterPrefab>(),
            new Queue<BoosterPrefab>()
    };

    private int _lvlItem;

    private readonly Store _store = Store.GetInstance();
    private Button _button;

    private void Awake()
    {
        GetComponents();
    }

    private void Start()
    {
        SetBaseOptions();
        if (_itemPrefabsActives.Count == 0)
            ActivatePrefab();
    }

    private void OnDestroy()
    {
        RemoveAllSubcriptions();
        Dispose();
    }

    private void SetBaseOptions()
    {
        CreateStartsPrefabsMaxCount();
        SetSubscriptions();
    }

    private void CreateStartsPrefabsMaxCount()
    {
        for (int i = 0; i < _itemPrefabsCreatedDeActivitesLvls.Length; i++)
            CreateStartsPrefabs(i, _itemPrefabsCreatedDeActivitesLvls[i]);
    }

    private void SetSubscriptions()
    {
        _store.BuyItemIsMadeBlockCanCreates += AddIcon;
        _store.SellItemIsMadeBlockCanCreates += RemoveIcon;
        _store.BoosterSetNewLevels += ChangeItemLvlbAfterBuyBooster;

        CreatorItemsInStore.Instance.StoreItemsCreated += AddIconOnStart;
    }

    private void RemoveAllSubcriptions()
    {
        _store.BuyItemIsMadeBlockCanCreates -= AddIcon;
        _store.SellItemIsMadeBlockCanCreates -= RemoveIcon;
        _store.BoosterSetNewLevels -= ChangeItemLvlbAfterBuyBooster;
        _button.onClick.RemoveAllListeners();

        CreatorItemsInStore.Instance.StoreItemsCreated -= AddIconOnStart;
    }

    private void AddIconOnStart()
    {
        foreach (var item in CreatorItemsInStore.Instance.CreatedItems)
        {
            AddIcon(item.ItemCurrentAmount, item.IndexItem);
            CreatorItemsInStore.Instance.StoreItemsCreated -= AddIconOnStart;
        }
    }

    private void GetComponents()
    {
        _button = GetComponent<Button>();
        _audioSource = GameObject.Find(_audiosourceObjectName).GetComponent<AudioSource>();
        _button.onClick.AddListener(PlayOneShot);
    }

    private void ActivatePrefab()
    {
        switch (_lvlItem)
        {
            case 0:
                ActiveCurrentLvlPrefabs(0, _itemPrefabsCreatedDeActivitesLvls[0]);
                break;
            case 1:
                ActiveCurrentLvlPrefabs(1, _itemPrefabsCreatedDeActivitesLvls[1]);
                break;
            case 2:
                ActiveCurrentLvlPrefabs(2, _itemPrefabsCreatedDeActivitesLvls[2]);
                break;
        }
    }

    private void ActiveCurrentLvlPrefabs(int prefabNumber, Queue<BoosterPrefab> listCreatedPrefabsLevelNumber)
    {
    ChekPrefab:
        switch (listCreatedPrefabsLevelNumber.Count != 0)
        {
            case true:
                GetCreatedPrefabAndPushInActivePrefabs(listCreatedPrefabsLevelNumber);
                break;
            case false:
                switch (_automatedPool)
                {
                    case true:
                        CreateDeactivatePrefab(prefabNumber, listCreatedPrefabsLevelNumber);
                        goto ChekPrefab;
                    case false:
                        break;
                }
                break;
        }
    }

    private void GetCreatedPrefabAndPushInActivePrefabs(Queue<BoosterPrefab> listCreatedPrefabsLevelNumber)
    {
        var prefab = listCreatedPrefabsLevelNumber.Dequeue();
        prefab.gameObject.SetActive(true);
        _itemPrefabsActives.Push(prefab);
    }

    private void DeactivatePrefab()
    {
        if (_itemPrefabsActives.Count != 0)
        {
            var prefab = _itemPrefabsActives.Pop();
            prefab.gameObject.SetActive(false);
            switch (_lvlItem)
            {
                case 0:
                    if (prefab.PrefabLvl == 0)
                        _itemPrefabsCreatedDeActivitesLvls[0].Enqueue(prefab);
                    break;
                case 1:
                    if (prefab.PrefabLvl == 1)
                        _itemPrefabsCreatedDeActivitesLvls[1].Enqueue(prefab);
                    break;
                case 2:
                    if (prefab.PrefabLvl == 2)
                        _itemPrefabsCreatedDeActivitesLvls[2].Enqueue(prefab);
                    break;
            }
        }
    }

    private void CreateStartsPrefabs(int prefabNumber, Queue<BoosterPrefab> listCreatedPrefabsLevelNumber)
    {
        for (int i = 0; i < _amountCreatedPrefabs; i++)
            CreateDeactivatePrefab(prefabNumber, listCreatedPrefabsLevelNumber);
    }

    private void CreateDeactivatePrefab(int prefabNumber, Queue<BoosterPrefab> listCreatedPrefabsLevelNumber)
    {
        var prefab = Instantiate(_itemPrefabs[prefabNumber], _transform.parent);
        listCreatedPrefabsLevelNumber.Enqueue(prefab);
        prefab.gameObject.SetActive(false);
    }

    private void AddIcon(long currentAmount, int indexItem)
    {
        if (indexItem == _indexItem)
        {
            if (currentAmount >= 1 && currentAmount > _itemPrefabsActives.Count)
            {
                int needCreate = (int)currentAmount - _itemPrefabsActives.Count;
                for (int i = 0; i < needCreate; i++)
                    ActivatePrefab();
            }
        }
    }

    private void RemoveIcon(long currentAmount, int indexItem)
    {
        if (indexItem == _indexItem)
        {
            if (currentAmount < _itemPrefabsActives.Count)
            {
                int needRemove = _itemPrefabsActives.Count - (int)currentAmount;
                for (int i = 0; i < needRemove; i++)
                    DeactivatePrefab();
            }
        }
    }

    private void ChangeItemLvlbAfterBuyBooster(int lvlBooster, int indexBooster)
    {
        if (_indexItem == indexBooster)
            _lvlItem = lvlBooster + 1;

        long amount = _itemPrefabsActives.Count;

        RemoveIcon(0, _indexItem);
        AddIcon(amount, _indexItem);
    }

    private void Dispose()
    {
        _itemPrefabsActives.Clear();
        _itemPrefabsActives = null;

        for (int i = 0; i < _itemPrefabsCreatedDeActivitesLvls.Length; i++)
        {
            _itemPrefabsCreatedDeActivitesLvls[i].Clear();
            _itemPrefabsCreatedDeActivitesLvls[i] = null;
        }
    }
}
