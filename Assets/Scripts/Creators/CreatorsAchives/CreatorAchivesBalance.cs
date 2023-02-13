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

        private void Start()
        {
            InstantiateStoreItems(_achivesBalance, _transform);
            ActiveStartItems();

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
        }

        private protected override void ActiveStartItems()
        {
            _createdItems[0].gameObject.SetActive(true);
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
