namespace Assets.Scripts.Items.Achivementitems
{
    class AchivementBooster : AchivementItem
    {
        public delegate void AchiveCreated(AchivementBooster achivementBooster);
        public event AchiveCreated AchivesCreated;

        private readonly Store _store = Store.GetInstance();

        private int _currentLvlBooster;

        private void ChangeStateAchivementAfterBuyBooster(int lvlBooster, int indexBooster)
        {
            if (_indexItem == indexBooster)
            {
                _currentLvlBooster = lvlBooster + 1;
                ChangeCurrentStateText(_currentLvlBooster);
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
