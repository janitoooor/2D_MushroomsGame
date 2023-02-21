using Assets.Scripts.Buttonss.PrestigButton;
using Assets.Scripts.Creators;
using Assets.Scripts.Creators.CreatorsAchives;
using Assets.Scripts.ItemBoosts;
using Assets.Scripts.StoreItem;
using Newtonsoft.Json;
using System;
using System.IO;
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

        private readonly string _firstRunKey = "IsFirstRun";

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
#if UNITY_WEBGL && !UNITY_EDITOR
                LoadExtern();
#else
                Load("");
#endif
            }

            PlayerPrefs.SetInt(_firstRunKey, 1);
            PlayerPrefs.Save();
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
#if UNITY_WEBGL && !UNITY_EDITOR
            string dataAsJson = JsonConvert.SerializeObject(_saveData);
            SaveExtern(dataAsJson);
#else
            string filePath = Application.persistentDataPath + "/" + "GameData";

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
            }

            string dataAsJson = JsonConvert.SerializeObject(_saveData);
            File.WriteAllText(filePath, dataAsJson);
#endif
        }

        public void Load(string value)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            _saveData = JsonConvert.DeserializeObject<SaveData>(value);
#else
            string filePath = Application.persistentDataPath + "/" + "GameData";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                _saveData = JsonConvert.DeserializeObject<SaveData>(json);
            }
#endif
        }

        public void SaveItems(StoreItemsObject itemStore)
        {
            _saveData.PricesItems[itemStore.IndexItem] = itemStore.ItemPrice;
            _saveData.CurrentAmountItems[itemStore.IndexItem] = itemStore.ItemCurrentAmount;
            _saveData.PassiveIncomeItems[itemStore.IndexItem] = itemStore.ItemPassiveIncome;
            _saveData.ItemsIsHidden[itemStore.IndexItem] = itemStore.ItemIsHidden;
            Save();
        }
        public void LoadItems(StoreItemsObject itemStore)
        {
            itemStore.LoadData(_saveData.CurrentAmountItems[itemStore.IndexItem], _saveData.PassiveIncomeItems[itemStore.IndexItem],
                _saveData.PricesItems[itemStore.IndexItem], _saveData.ItemsIsHidden[itemStore.IndexItem]);
        }

        public void SaveBoosters(ItemBooster itemBooster)
        {
            _saveData.IndexLvlsBosters[itemBooster.IndexBooster] = itemBooster.IndexLvl;
            Save();
        }

        public void LoadBoosters(ItemBooster itemBooster)
        {
            itemBooster.LoadData(_saveData.IndexLvlsBosters[itemBooster.IndexBooster]);

        }

        public void SaveBalance()
        {
            _saveData.CoinsBalance = _bankBalance.CoinsBalance;
            _saveData.BankPassiveIncome = _bankPassiveIncome.PassiveIncomeCoins;
            _saveData.GemBalance = _gemBank.GemsBalance;
            Save();
        }

        public void LoadBalance()
        {
            _bankBalance.LoadCoinsBalance(_saveData.CoinsBalance);
            _bankPassiveIncome.LoadPassiveIncome(_saveData.BankPassiveIncome);
            _gemBank.LoadGemBalance(_saveData.GemBalance);
        }

        public void SaveAchives(AchivementItem achives)
        {
            _saveData.AchivesIsGetsValue[achives.IndexAchives] = achives.ItemIsGetValue;
            _saveData.AchivesIsUnlocked[achives.IndexAchives] = achives.ItemIsUnlocked;
            Save();
        }

        public void LoadAchives(AchivementItem achives)
        {
            achives.LoadData(_saveData.AchivesIsGetsValue[achives.IndexAchives], _saveData.AchivesIsUnlocked[achives.IndexAchives]);
        }

        public void SaveSkins(ClickSkinItem skinItem)
        {
            _saveData.SkinIsBuying[skinItem.IndexItem] = skinItem.ItemIsBuying;
            _saveData.SkinIsSelected[skinItem.IndexItem] = skinItem.ItemSelected;
            Save();
        }

        public void LoadSkins(ClickSkinItem skinItem)
        {
            skinItem.LoadData(_saveData.SkinIsBuying[skinItem.IndexItem], _saveData.SkinIsSelected[skinItem.IndexItem]);
        }

        public void ClearSaves()
        {
            _bankBalance.StopTimerSaveRoutine();
            _saveData = new();
            Save();
            PlayerPrefs.DeleteAll();
#if !UNITY_EDITOR && UNITY_WEBGL
            //LoadExtern();
#else
            Load("");
#endif
        }

        [Serializable]
        public class SaveData
        {
            private static readonly int s_maxAmountItems = 11;
            private static readonly int s_maxAmountAchivesItems = 100;

            public long CoinsBalance;
            public long GemBalance;
            public long BankPassiveIncome;

            public long[] PricesItems = new long[s_maxAmountItems];
            public long[] PassiveIncomeItems = new long[s_maxAmountItems];
            public long[] CurrentAmountItems = new long[s_maxAmountItems];
            public bool[] ItemsIsHidden = new bool[s_maxAmountItems];

            public int[] IndexLvlsBosters =
            {
                -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
            };

            public bool[] AchivesIsGetsValue = new bool[s_maxAmountAchivesItems];
            public bool[] AchivesIsUnlocked = new bool[s_maxAmountAchivesItems];

            public bool[] SkinIsBuying = new bool[s_maxAmountItems];
            public bool[] SkinIsSelected = new bool[s_maxAmountItems];
        }
    }
}
