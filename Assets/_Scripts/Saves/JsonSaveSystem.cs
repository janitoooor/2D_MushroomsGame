using UnityEngine;
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

    private SaveSystem.SaveData _saveData = new();

    private readonly BankBalance _bankBalance = BankBalance.GetInstance();
    private readonly BankPassiveIncome _bankPassiveIncome = BankPassiveIncome.GetInstance();
    private readonly GemBank _gemBank = GemBank.GetInstance();

    private void Awake()
    {
        Instance = this;
        _saveData = SaveSystem.Instance.CurrentSaveData;
    }

    private void Start()
    {
        ButtonRestartScene.Instance.RestartsGame += ClearSaves;
    }

    private void OnDestroy()
    {
        ButtonRestartScene.Instance.RestartsGame -= ClearSaves;
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
            SaveSystem.Instance.Save();
    }
    public void LoadItems(StoreItemsObject itemStore)
    {
        itemStore.LoadData(_saveData.CurrentAmountItems[itemStore.IndexItem], _saveData.PassiveIncomeItems[itemStore.IndexItem],
            _saveData.PricesItems[itemStore.IndexItem], _saveData.ItemsIsHidden[itemStore.IndexItem]);
    }

    public void SaveBoosters(ItemBooster itemBooster)
    {
        if (_saveData.IndexLvlsBosters[itemBooster.IndexBooster] == itemBooster.IndexLvl)
            return;

        _saveData.IndexLvlsBosters[itemBooster.IndexBooster] = itemBooster.IndexLvl;
        SaveSystem.Instance.Save();
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
            SaveSystem.Instance.Save();
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
            SaveSystem.Instance.Save();
    }

    public void LoadAchives(AchivementItem achives)
    {
        achives.LoadData(_saveData.AchivesIsGetsValue[achives.IndexAchives], _saveData.AchivesIsUnlocked[achives.IndexAchives]);
    }

    public void SaveAchivesBuy(AchivementItemBuy achivesBuy)
    {
        if (_saveData.AchivementBuyAmountItemsBuy[achivesBuy.IndexAchives] == achivesBuy.CurrentAmountItemsBuy)
            return;

        _saveData.AchivementBuyAmountItemsBuy[achivesBuy.IndexAchives] = achivesBuy.CurrentAmountItemsBuy;
        SaveSystem.Instance.Save();
    }

    public void LoadAchivesBuy(AchivementItemBuy achivesBuy)
    {
        achivesBuy.LoadData(_saveData.AchivementBuyAmountItemsBuy[achivesBuy.IndexAchives]);
    }

    public void SaveAchivesSell(AchivementItemSell achivesSell)
    {
        if (_saveData.AchivemenSellyAmountItemsSell[achivesSell.IndexAchives] == achivesSell.CurrentAmountItemsSell)
            return;

        _saveData.AchivemenSellyAmountItemsSell[achivesSell.IndexAchives] = achivesSell.CurrentAmountItemsSell;
        SaveSystem.Instance.Save();
    }

    public void LoadAchivesSell(AchivementItemSell achivesSell)
    {
        achivesSell.LoadData(_saveData.AchivemenSellyAmountItemsSell[achivesSell.IndexAchives]);
    }

    public void SaveAchivesBooster(AchivementBooster achivementBooster)
    {
        if (_saveData.AchivementBoosterLvlBooster[achivementBooster.IndexAchives] == achivementBooster.CurrentLvlBooster)
            return;

        _saveData.AchivementBoosterLvlBooster[achivementBooster.IndexAchives] = achivementBooster.CurrentLvlBooster;
        SaveSystem.Instance.Save();
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
            SaveSystem.Instance.Save();
    }

    public void LoadSkins(ClickSkinItem skinItem)
    {
        skinItem.LoadData(_saveData.SkinIsBuying[skinItem.IndexItem], _saveData.SkinIsSelected[skinItem.IndexItem]);
    }

    public void SaveAuthBonus(AuthBonus authBonus)
    {
        if (_saveData.AuthBonus == authBonus)
            return;

        _saveData.AuthBonus = authBonus.GemsAdded;
        SaveSystem.Instance.Save();
    }

    public void LoadAuthBonus(AuthBonus authBonus)
    {
        authBonus.LoadData(_saveData.AuthBonus);
    }

    public void SaveMaxBalance()
    {
        if (_saveData.MaxCoinsBalance == _bankBalance.MaxBalance)
            return;

        _saveData.MaxCoinsBalance = _bankBalance.MaxBalance;
        SaveSystem.Instance.Save();
    }

    public void LoadMaxBalance()
    {
        _bankBalance.LoadMaxBalance(_saveData.MaxCoinsBalance);
    }

    public void SaveTutorialPanel(TutorialPanel tutorialPanel)
    {
        if (_saveData.TutorialPanelIsShowed[tutorialPanel.IndexItem] == tutorialPanel.IsShowed)
            return;

        _saveData.TutorialPanelIsShowed[tutorialPanel.IndexItem] = tutorialPanel.IsShowed;
        SaveSystem.Instance.Save();
    }

    public void LoadTutorialPanel(TutorialPanel tutorialPanel)
    {
        tutorialPanel.LoadData(_saveData.TutorialPanelIsShowed[tutorialPanel.IndexItem]);
    }

    public void ClearSaves()
    {
        _bankBalance.StopTimerSaveRoutine();
        SaveSystem.Instance.ClearSaves();
    }
}
