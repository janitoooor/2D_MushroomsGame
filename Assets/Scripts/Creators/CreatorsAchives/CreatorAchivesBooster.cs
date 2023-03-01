using Assets.Scripts.Items.Achivementitems;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creators
{
    class CreatorAchivesBooster : CreatorItems
    {
        [SerializeField] private List<AchivementBooster> _achivesBooster;

        [SerializeField] private Transform _transform;

        private readonly List<AchivementBooster> _createdAchives = new List<AchivementBooster>();
        public List<AchivementBooster> CreatedAchivesBooster { get => _createdAchives; }

        private void Start()
        {
            InstantiateStoreItems(_achivesBooster, _transform);

            foreach (var item in _createdItems)
            {
                if (_createdItems != null)
                {
                    _createdAchives.Add((AchivementBooster)item);
                }
            }

            foreach (var item in _createdAchives)
            {
                if (_createdItems != null)
                    item.AchivesCreated += ChangeAchivement;
            }

            SetStartAchivesItems();
            ActiveStartItems();
        }

        private protected override void ActiveStartItems()
        {
            foreach (var item in _createdAchives)
            {
                if (!item.ItemIsGetValue)
                {
                    item.gameObject.SetActive(true);
                    break;
                }
            }
        }

        private void SetStartAchivesItems()
        {
            foreach (var item in _createdAchives)
            {
                if (item.ItemIsGetValue)
                    item.gameObject.SetActive(false);
            }
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
