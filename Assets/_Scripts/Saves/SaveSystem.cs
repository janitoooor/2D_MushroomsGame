using Assets.Scripts.Buttonss.PrestigButton;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using Assets.Scripts;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private SaveData _saveData = new();
    public SaveData CurrentSaveData => _saveData;

    [DllImport("__Internal")]
    private static extern void AuthExtern();
    [DllImport("__Internal")]
    private static extern void SaveExtern(string date);
    [DllImport("__Internal")]
    private static extern void LoadExtern();

    private bool _dataIsLoaded;

    private readonly string _pathName = "/" + "GameDataFess";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
                LoadExtern();
#else
        Load("");
#endif

        if (!_dataIsLoaded)
            StartCoroutine(CheckDataIsLodaded());
    }

    public void Save()
    {
        if (!_dataIsLoaded)
            return;

#if UNITY_WEBGL && !UNITY_EDITOR
            string dataAsJson = JsonConvert.SerializeObject(_saveData);
            SaveExtern(dataAsJson);
#else
        string filePath = Application.persistentDataPath + _pathName;

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
        string filePath = Application.persistentDataPath + _pathName;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _saveData = JsonConvert.DeserializeObject<SaveData>(json);
        }
#endif
        Debug.Log("Load");
        _dataIsLoaded = true;
    }

    public void Auth()
    {
        AuthExtern();
    }

    private IEnumerator CheckDataIsLodaded()
    {
        float timeToChekData = 3f;
        yield return new WaitForSeconds(timeToChekData);
        _dataIsLoaded = true;
    }

    public void ClearSaves()
    {
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
        private static readonly int s_maxAmountItems = 20;
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
