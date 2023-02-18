using Assets.Scripts.Items.Achivementitems;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creators.CreatorsAchives
{
    class CreatorAchivesSell : CreatorItems
    {
        [SerializeField] private List<AchivementItemSell> _achivesItemSell;


        [SerializeField] private Transform _transform;

        private readonly List<AchivementItemSell> _createdAchives = new List<AchivementItemSell>();

        private void Start()
        {
            InstantiateStoreItems(_achivesItemSell, _transform);

            foreach (var item in _createdItems)
            {
                if (_createdItems != null)
                    _createdAchives.Add((AchivementItemSell)item);
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

        private void ChangeAchivement(AchivementItemSell achivementSell)
        {
            achivementSell.gameObject.SetActive(false);

            for (int i = 0; i < _createdAchives.Count - 1; i++)
            {
                if (achivementSell == _createdAchives[i])
                    _createdAchives[i + 1].gameObject.SetActive(true);
            }
        }
    }
}
