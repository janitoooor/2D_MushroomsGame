using Assets.Scripts.ItemBoosts;

namespace Assets.Scripts.Items.Achivementitems
{
    class AchivementBooster : AchivementItem
    {
        public delegate void AchiveCreated(AchivementBooster achivementBooster);
        public event AchiveCreated AchivesCreated;

        private readonly Store _store = Store.GetInstance();

        private int _currentLvlBooster;
        public int CurrentLvlBooster { get => _currentLvlBooster; }

        private void Start()
        {
            JsonSaveSystem.Instance.LoadAchivesBooster(this);
            SetSubscriptions();
            LockAchivement();
            ChangeCurrentStateText(_currentLvlBooster);
            foreach (var item in CreatorItemBooster.Instance.CreatedItemsBooster)
                ChangeStateAchivementAfterBuyBooster(item.IndexLvl, item.IndexBooster);
        }

        public void LoadData(int currentLvlBooster)
        {
            _currentLvlBooster = currentLvlBooster;
        }

        private void ChangeStateAchivementAfterBuyBooster(int lvlBooster, int indexBooster)
        {
            if (_indexItem == indexBooster)
            {
                _currentLvlBooster = lvlBooster + 1;
                ChangeCurrentStateText(_currentLvlBooster);
                JsonSaveSystem.Instance.SaveAchivesBooster(this);

                if (_currentLvlBooster >= _goal)
                {
                    UnlockAchivement();
                    _store.BoosterSetNewLevels -= ChangeStateAchivementAfterBuyBooster;
                }
            }
        }

        private protected override void GetValueOnClickUnlockedItem()
        {
            base.GetValueOnClickUnlockedItem();
            AchivesCreated?.Invoke(this);
        }

        private protected override void RemoveAllSubscriptions()
        {
            _store.BoosterSetNewLevels -= ChangeStateAchivementAfterBuyBooster;

        }

        private protected override void SetSubscriptions()
        {
            _store.BoosterSetNewLevels += ChangeStateAchivementAfterBuyBooster;
        }
    }
}
