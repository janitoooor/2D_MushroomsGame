using Assets.Scripts.Items.Achivementitems;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creators
{
    class CreatorAchivesBooster : CreatorItems
    {
        [SerializeField] private List<AchivementBooster> _achivesBooster;

        [SerializeField] private Transform _transform;

        private readonly List<AchivementBooster> _createdAchives = new List<AchivementBooster>();

        private void Start()
        {
            InstantiateStoreItems(_achivesBooster, _transform);
            ActiveStartItems();

            foreach (var item in _createdItems)
            {
                if (_createdItems != null)
                    _createdAchives.Add((AchivementBooster)item);
            }

            foreach (var item in _createdAchives)
            {
                if (_createdItems != null)
                    item.AchivesCreated += ChangeAchivement;
            }
        }

        private protected override void ActiveStartItems()
        {
            _createdItems[0].gameObject.SetActive(true);
        }

        private void ChangeAchivement(AchivementBooster achivementBooster)
        {
            achivementBooster.gameObject.SetActive(false);

            for (int i = 0; i < _createdAchives.Count - 1; i++)
            {
                if (achivementBooster == _createdAchives[i])
                    _createdAchives[i + 1].gameObject.SetActive(true);
            }
        }
    }
}
