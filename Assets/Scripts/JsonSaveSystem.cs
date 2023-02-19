using Assets.Scripts.Buttonss.PrestigButton;
using Assets.Scripts.Creators;
using Assets.Scripts.Creators.CreatorsAchives;
using Assets.Scripts.ItemBoosts;
using Assets.Scripts.StoreItem;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts
{
    class JsonSaveSystem : MonoBehaviour
    {
        public static JsonSaveSystem Instance { get; private set; }

        [Space]
        [SerializeField] private CreatorItemBooster _creatorItemBooster;
        [SerializeField] private CreatorItemsInStore _creatorItemsInStore;
        [Space]
        [SerializeField] private CreatorAchivesBuy _creatorAchivesBuy;
        [SerializeField] private CreatorAchivesBooster _creatorAchivesBooster;
        [SerializeField] private CreatorAchivesSell _creatorAchivesSell;
        [SerializeField] private CreatorAchivesBalance _creatorAchivesBalance;
        [Space]
        [SerializeField] private CreatorSkinClickItem _creatorSkinItems;
        [Space]

        private SaveData _saveData = new();

        private readonly BankBalance _bankBalance = BankBalance.GetInstance();
        private readonly BankPassiveIncome _bankPassiveIncome = BankPassiveIncome.GetInstance();
        private readonly GemBank _gemBank = GemBank.GetInstance();

        private readonly string _firstRunKey = "isFirstRun";


        [DllImport("__Internal")]
        private static extern void SaveExtern(string date);
        [DllImport("__Internal")]
        private static extern void LoadExtern();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (PlayerPrefs.HasKey(_firstRunKey))
            {
                Debug.Log("Has key");
#if !UNITY_EDITOR
            LoadExtern();
#endif
            }

            PlayerPrefs.SetInt(_firstRunKey, 1);
            PlayerPrefs.Save();

            Save();
        }

        private void Start()
        {
            ButtonRestartScene.Instance.RestartsGame += ClearSaves;
        }

        private void OnDestroy()
        {
            ButtonRestartScene.Instance.RestartsGame -= ClearSaves;
        }

        public void Save()
        {
            SaveBalance();
            SaveItems();
            SaveBoosters();
            SaveAllAchives();
            SaveSkins();

            string dataAsJson = JsonUtility.ToJson(_saveData);
#if !UNITY_EDITOR
            SaveExtern(dataAsJson);
#endif
            Debug.Log($"Game Saved coins balance{_saveData.CoinsBalance}");
        }

        public void Load(string value)
        {
            _saveData = JsonUtility.FromJson<SaveData>(value);

            LoadBalance();
            LoadItems();
            LoadBoosters();
            LoadAllAchives();
            LoadSkins();

            Debug.Log($"Game Loaded coins balance {_bankBalance.CoinsBalance}");
        }

        private void SaveItems()
        {
            for (int i = 0; i < _creatorItemsInStore.CreatedItems.Count; i++)
            {
                _saveData.PricesItems[i] = _creatorItemsInStore.CreatedItems[i].ItemPrice;
                _saveData.CurrentAmountItems[i] = _creatorItemsInStore.CreatedItems[i].ItemCurrentAmount;
                _saveData.PassiveIncomeItems[i] = _creatorItemsInStore.CreatedItems[i].ItemCurrentAmount;
                _saveData.ItemsIsHidden[i] = _creatorItemsInStore.CreatedItems[i].ItemIsHidden;
            }
        }
        private void LoadItems()
        {
            for (int i = 0; i < _creatorItemsInStore.CreatedItems.Count; i++)
            {
                _creatorItemsInStore.CreatedItems[i].LoadData(_saveData.CurrentAmountItems[i], _saveData.PassiveIncomeItems[i],
                    _saveData.PricesItems[i], _saveData.ItemsIsHidden[i]);
            }
        }

        private void SaveBoosters()
        {
            for (int i = 0; i < _creatorItemBooster.CreatedItemsBooster.Count; i++)
                _saveData.IndexLvlsBosters[i] = _creatorItemBooster.CreatedItemsBooster[i].IndexLvl;
        }

        private void LoadBoosters()
        {
            for (int i = 0; i < _creatorItemBooster.CreatedItemsBooster.Count; i++)
                _creatorItemBooster.CreatedItemsBooster[i].LoadData(_saveData.IndexLvlsBosters[i]);
        }

        private void SaveBalance()
        {
            _saveData.CoinsBalance = _bankBalance.CoinsBalance;
            _saveData.BankPassiveIncome = _bankPassiveIncome.PassiveIncomeCoins;
            _saveData.GemBalance = _gemBank.GemsBalance;
        }

        private void LoadBalance()
        {
            _bankBalance.LoadCoinsBalance(_saveData.CoinsBalance);
            _bankPassiveIncome.LoadPassiveIncome(_saveData.BankPassiveIncome);
            _gemBank.LoadGemBalance(_saveData.GemBalance);
        }

        private void SaveAllAchives()
        {
            SaveOneTypeAchives(_creatorAchivesBalance.CreatedAchivesBalance);
            SaveOneTypeAchives(_creatorAchivesBooster.CreatedAchivesBooster);
            SaveOneTypeAchives(_creatorAchivesBuy.CreatedAchivesBuy);
            SaveOneTypeAchives(_creatorAchivesSell.CreatedAchivesSell);
        }

        private void SaveOneTypeAchives<T>(List<T> creatorAchives) where T : AchivementItem
        {
            for (int i = 0; i < creatorAchives.Count; i++)
            {
                _saveData.AchivesIsGetsValue[i] = creatorAchives[i].ItemIsGetValue;
                _saveData.AchivesIsUnlocked[i] = creatorAchives[i].ItemIsUnlocked;
            }
        }

        private void LoadAllAchives()
        {
            LoadOneTypeAchives(_creatorAchivesBalance.CreatedAchivesBalance);
            LoadOneTypeAchives(_creatorAchivesBooster.CreatedAchivesBooster);
            LoadOneTypeAchives(_creatorAchivesBuy.CreatedAchivesBuy);
            LoadOneTypeAchives(_creatorAchivesSell.CreatedAchivesSell);
        }

        private void LoadOneTypeAchives<T>(List<T> creatorAchives) where T : AchivementItem
        {
            for (int i = 0; i < creatorAchives.Count; i++)
                creatorAchives[i].LoadData(_saveData.AchivesIsGetsValue[i], _saveData.AchivesIsUnlocked[i]);
        }

        public void SaveSkins()
        {
            for (int i = 0; i < _creatorSkinItems.CreatedSkinItems.Count; i++)
            {
                _saveData.SkinIsBuying[i] = _creatorSkinItems.CreatedSkinItems[i].ItemIsBuying;
                _saveData.SkinIsSelected[i] = _creatorSkinItems.CreatedSkinItems[i].ItemSelected;
            }
        }

        public void LoadSkins()
        {
            for (int i = 0; i < _creatorSkinItems.CreatedSkinItems.Count; i++)
                _creatorSkinItems.CreatedSkinItems[i].LoadData(_saveData.SkinIsBuying[i], _saveData.SkinIsSelected[i]);
        }

        public void ClearSaves()
        {
            _bankBalance.StopTimerSaveRoutine();
            _saveData = new();
            string dataAsJson = JsonUtility.ToJson(_saveData);
            SaveExtern(dataAsJson);
            LoadExtern();
            Debug.LogError($"Reset game coins balance {_bankBalance.CoinsBalance}");
        }

        [Serializable]
        public class SaveData
        {
            private static readonly int s_maxAmountItems = 10;
            private static readonly int s_maxAmountAchivesItems = 100;

            public long CoinsBalance;
            public long GemBalance;
            public long BankPassiveIncome;

            public long[] PricesItems = new long[s_maxAmountItems];
            public long[] PassiveIncomeItems = new long[s_maxAmountItems];
            public long[] CurrentAmountItems = new long[s_maxAmountItems];
            public bool[] ItemsIsHidden = new bool[s_maxAmountItems];

            public int[] IndexLvlsBosters = new int[s_maxAmountItems];

            public bool[] AchivesIsGetsValue = new bool[s_maxAmountAchivesItems];
            public bool[] AchivesIsUnlocked = new bool[s_maxAmountAchivesItems];

            public bool[] SkinIsBuying = new bool[s_maxAmountItems];
            public bool[] SkinIsSelected = new bool[s_maxAmountItems];
        }
    }
}
