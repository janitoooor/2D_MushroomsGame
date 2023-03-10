using Assets.Scripts.Buttonss.PrestigButton;
using Assets.Scripts.Creators;
using Assets.Scripts.Creators.CreatorsAchives;
using Assets.Scripts.ItemBoosts;
using Assets.Scripts.Items.Achivementitems;
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

        [DllImport("__Internal")]
        private static extern void AuthExtern();
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

#if UNITY_WEBGL && !UNITY_EDITOR
                LoadExtern();
#else
            Load("");
#endif
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
            Debug.Log("Save");
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
            Debug.Log("Load");
        }

        public void Auth()
        {
            AuthExtern();
        }

        public void SaveItems(StoreItemsObject itemStore)
        {
            bool dataIsChanged = false;
            if (_saveData.PricesItems[itemStore.IndexItem] != itemStore.ItemPrice)
            {
                _saveData.PricesItems[itemStore.IndexItem] = itemStore.ItemPrice;
                dataIsChanged = true;
            }
            if (_saveData.CurrentAmountItems[itemStore.IndexItem] != itemStore.ItemCurrentAmount)
            {
                _saveData.CurrentAmountItems[itemStore.IndexItem] = itemStore.ItemCurrentAmount;
                dataIsChanged = true;
            }
            if (_saveData.PassiveIncomeItems[itemStore.IndexItem] != itemStore.ItemPassiveIncome)
            {
                _saveData.PassiveIncomeItems[itemStore.IndexItem] = itemStore.ItemPassiveIncome;
                dataIsChanged = true;
            }
            if (_saveData.ItemsIsHidden[itemStore.IndexItem] != itemStore.ItemIsHidden)
            {
                _saveData.ItemsIsHidden[itemStore.IndexItem] = itemStore.ItemIsHidden;
                dataIsChanged = true;
            }

            if (dataIsChanged)
                Save();
        }
        public void LoadItems(StoreItemsObject itemStore)
        {
            itemStore.LoadData(_saveData.CurrentAmountItems[itemStore.IndexItem], _saveData.PassiveIncomeItems[itemStore.IndexItem],
                _saveData.PricesItems[itemStore.IndexItem], _saveData.ItemsIsHidden[itemStore.IndexItem]);
        }

        public void SaveBoosters(ItemBooster itemBooster)
        {
            if (_saveData.IndexLvlsBosters[itemBooster.IndexBooster] != itemBooster.IndexLvl)
            {
                _saveData.IndexLvlsBosters[itemBooster.IndexBooster] = itemBooster.IndexLvl;
                Save();
            }
        }

        public void LoadBoosters(ItemBooster itemBooster)
        {
            itemBooster.LoadData(_saveData.IndexLvlsBosters[itemBooster.IndexBooster]);
        }

        public void SaveBalance()
        {
            bool dataIsChanged = false;
            if (_saveData.CoinsBalance != _bankBalance.CoinsBalance)
            {
                _saveData.CoinsBalance = _bankBalance.CoinsBalance;
                dataIsChanged = true;
            }
            if (_saveData.BankPassiveIncome != _bankPassiveIncome.PassiveIncomeCoins)
            {
                _saveData.BankPassiveIncome = _bankPassiveIncome.PassiveIncomeCoins;
                dataIsChanged = true;
            }
            if (_saveData.GemBalance != _gemBank.GemsBalance)
            {
                _saveData.GemBalance = _gemBank.GemsBalance;
                dataIsChanged = true;
            }

            if (dataIsChanged)
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
            bool dataIsChanged = false;
            if (_saveData.AchivesIsGetsValue[achives.IndexAchives] != achives.ItemIsGetValue)
            {
                _saveData.AchivesIsGetsValue[achives.IndexAchives] = achives.ItemIsGetValue;
                dataIsChanged = true;
            }

            if (_saveData.AchivesIsUnlocked[achives.IndexAchives] != achives.ItemIsUnlocked)
            {
                _saveData.AchivesIsUnlocked[achives.IndexAchives] = achives.ItemIsUnlocked;
                dataIsChanged = true;
            }

            if (dataIsChanged)
                Save();
        }

        public void LoadAchives(AchivementItem achives)
        {
            achives.LoadData(_saveData.AchivesIsGetsValue[achives.IndexAchives], _saveData.AchivesIsUnlocked[achives.IndexAchives]);
        }

        public void SaveAchivesBuy(AchivementItemBuy achivesBuy)
        {
            if (_saveData.AchivementBuyAmountItemsBuy[achivesBuy.IndexAchives] != achivesBuy.CurrentAmountItemsBuy)
            {
                _saveData.AchivementBuyAmountItemsBuy[achivesBuy.IndexAchives] = achivesBuy.CurrentAmountItemsBuy;
                Save();
            }
        }

        public void LoadAchivesBuy(AchivementItemBuy achivesBuy)
        {
            achivesBuy.LoadData(_saveData.AchivementBuyAmountItemsBuy[achivesBuy.IndexAchives]);
        }

        public void SaveAchivesSell(AchivementItemSell achivesSell)
        {
            if (_saveData.AchivemenSellyAmountItemsSell[achivesSell.IndexAchives] != achivesSell.CurrentAmountItemsSell)
            {
                _saveData.AchivemenSellyAmountItemsSell[achivesSell.IndexAchives] = achivesSell.CurrentAmountItemsSell;
                Save();
            }
        }

        public void LoadAchivesSell(AchivementItemSell achivesSell)
        {
            achivesSell.LoadData(_saveData.AchivemenSellyAmountItemsSell[achivesSell.IndexAchives]);
        }

        public void SaveAchivesBooster(AchivementBooster achivementBooster)
        {
            if (_saveData.AchivementBoosterLvlBooster[achivementBooster.IndexAchives] != achivementBooster.CurrentLvlBooster)
            {
                _saveData.AchivementBoosterLvlBooster[achivementBooster.IndexAchives] = achivementBooster.CurrentLvlBooster;
                Save();
            }
        }

        public void LoadAchivesBooster(AchivementBooster achivementBooster)
        {
            achivementBooster.LoadData(_saveData.AchivementBoosterLvlBooster[achivementBooster.IndexAchives]);
        }

        public void SaveSkins(ClickSkinItem skinItem)
        {
            bool dataIsChanged = false;
            if (_saveData.SkinIsBuying[skinItem.IndexItem] != skinItem.ItemIsBuying)
            {
                _saveData.SkinIsBuying[skinItem.IndexItem] = skinItem.ItemIsBuying;
                dataIsChanged = true;
            }
            if (_saveData.SkinIsSelected[skinItem.IndexItem] != skinItem.ItemSelected)
            {
                _saveData.SkinIsSelected[skinItem.IndexItem] = skinItem.ItemSelected;
                dataIsChanged = true;
            }

            if (dataIsChanged)
                Save();
        }

        public void LoadSkins(ClickSkinItem skinItem)
        {
            skinItem.LoadData(_saveData.SkinIsBuying[skinItem.IndexItem], _saveData.SkinIsSelected[skinItem.IndexItem]);
        }

        public void SaveAuthBonus(AuthBonus authBonus)
        {
            if (_saveData.AuthBonus != authBonus)
            {
                _saveData.AuthBonus = authBonus.GemsAdded;
                Save();
            }
        }

        public void LoadAuthBonus(AuthBonus authBonus)
        {
            authBonus.LoadData(_saveData.AuthBonus);
        }

        public void SaveMaxBalance()
        {
            if (_saveData.MaxCoinsBalance != _bankBalance.MaxBalance)
            {
                _saveData.MaxCoinsBalance = _bankBalance.MaxBalance;
                Save();
            }
        }

        public void LoadMaxBalance()
        {
            _bankBalance.LoadMaxBalance(_saveData.MaxCoinsBalance);
        }

        public void SaveTutorialPanel(TutorialPanel tutorialPanel)
        {
            if (_saveData.TutorialPanelIsShowed[tutorialPanel.IndexItem] != tutorialPanel.IsShowed)
            {
                _saveData.TutorialPanelIsShowed[tutorialPanel.IndexItem] = tutorialPanel.IsShowed;
                Save();
            }
        }

        public void LoadTutorialPanel(TutorialPanel tutorialPanel)
        {
            tutorialPanel.LoadData(_saveData.TutorialPanelIsShowed[tutorialPanel.IndexItem]);
        }

        public void ClearSaves()
        {
            _bankBalance.StopTimerSaveRoutine();
            bool authBonus = _saveData.AuthBonus;
            long gemBalance = _saveData.GemBalance;
            bool[] skinsIsBuying = new bool[11];
            bool[] skinsIsSelected = new bool[11];

            for (int i = 0; i < _saveData.SkinIsBuying.Length; i++)
                skinsIsBuying[i] = _saveData.SkinIsBuying[i];

            for (int i = 0; i < _saveData.SkinIsSelected.Length; i++)
                skinsIsSelected[i] = _saveData.SkinIsSelected[i];

            _saveData = new();
            _saveData.GemBalance = gemBalance;
            _saveData.AuthBonus = authBonus;

            for (int i = 0; i < skinsIsBuying.Length; i++)
                _saveData.SkinIsBuying[i] = skinsIsBuying[i];

            for (int i = 0; i < skinsIsSelected.Length; i++)
                _saveData.SkinIsSelected[i] = skinsIsSelected[i];

            Save();
        }

        [Serializable]
        public class SaveData
        {
            private static readonly int s_maxAmountItems = 11;
            private static readonly int s_maxAmountAchivesItems = 100;

            public bool AuthBonus;

            public long MaxCoinsBalance;
            public long CoinsBalance;
            public long GemBalance;
            public long BankPassiveIncome;

            public long[] PricesItems = new long[s_maxAmountItems];
            public long[] PassiveIncomeItems = new long[s_maxAmountItems];
            public long[] CurrentAmountItems = new long[s_maxAmountItems];
            public bool[] ItemsIsHidden = new bool[s_maxAmountItems];

            public int[] IndexLvlsBosters =
            {
                -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            };

            public bool[] AchivesIsGetsValue = new bool[s_maxAmountAchivesItems];
            public bool[] AchivesIsUnlocked = new bool[s_maxAmountAchivesItems];

            public bool[] SkinIsBuying = new bool[s_maxAmountItems];
            public bool[] SkinIsSelected = new bool[s_maxAmountItems];

            public long[] AchivementBuyAmountItemsBuy = new long[s_maxAmountAchivesItems];
            public long[] AchivemenSellyAmountItemsSell = new long[s_maxAmountAchivesItems];

            public int[] AchivementBoosterLvlBooster = new int[s_maxAmountAchivesItems];

            public bool[] TutorialPanelIsShowed = new bool[s_maxAmountItems];
        }
    }
}
