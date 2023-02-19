using Assets.Scripts.Items.Achivementitems;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creators.CreatorsAchives
{
    class CreatorAchivesBalance : CreatorItems
    {
        [SerializeField] private List<AchivementNewBalance> _achivesBalance;

        [SerializeField] private Transform _transform;

        private readonly List<AchivementNewBalance> _createdAchives = new();
        public List<AchivementNewBalance> CreatedAchivesBalance { get => _createdAchives; }

        private void Start()
        {
            InstantiateStoreItems(_achivesBalance, _transform);


            foreach (var item in _createdItems)
            {
                if (_createdItems != null)
                    _createdAchives.Add((AchivementNewBalance)item);
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

        private void ChangeAchivement(AchivementNewBalance achivementNewBalance)
        {
            achivementNewBalance.gameObject.SetActive(false);

            for (int i = 0; i < _createdAchives.Count - 1; i++)
            {
                if (achivementNewBalance == _createdAchives[i])
                    _createdAchives[i + 1].gameObject.SetActive(true);
            }
        }
    }
}
