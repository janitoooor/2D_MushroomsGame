using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    class JsonBalanceSaveSystem : MonoBehaviour
    {
        public static JsonBalanceSaveSystem Instance { get; private set; }
        private BalanceSaveData _saveData = new();

        private readonly BankBalance _bankBalance = BankBalance.GetInstance();
        private readonly BankPassiveIncome _bankPassiveIncome = BankPassiveIncome.GetInstance();
        private readonly GemBank _gemBank = GemBank.GetInstance();

        private readonly string _fileName = "gameData.json";
        private string _filePath;

        private void Awake()
        {
            _filePath = Application.persistentDataPath + "/" + _fileName;

            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(this);
            Instance = this;
            Load();
        }

        public void Save()
        {
            _saveData.CoinsBalance = _bankBalance.CoinsBalance;
            _saveData.BankPassiveIncome = _bankPassiveIncome.PassiveIncomeCoins;
            _saveData.GemBalance = _gemBank.GemsBalance;

            string dataAsJson = JsonUtility.ToJson(_saveData, true);
            File.WriteAllText(_filePath, dataAsJson);
            Debug.Log("Game Saved");
        }

        public void Load()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _saveData = JsonUtility.FromJson<BalanceSaveData>(json);

                _bankBalance.LoadCoinsBalance(_saveData.CoinsBalance);
                _bankPassiveIncome.LoadPassiveIncome(_saveData.BankPassiveIncome);
                _gemBank.LoadGemBalance(_saveData.GemBalance);
                Debug.Log($"Game loaded");
            }
            else
            {
                Debug.Log($"No saved data found");
            }
        }

        public void ClearSaves()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
                Debug.Log($"Save file deleted");
            }
            else
            {
                Debug.Log($"No saved data found");
            }
        }

        [Serializable]
        public class BalanceSaveData
        {
            public long CoinsBalance;
            public long GemBalance;
            public long BankPassiveIncome;
        }
    }
}
