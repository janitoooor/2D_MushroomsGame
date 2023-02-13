using Assets.Scripts.Items.Achivementitems;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creators
{
    class CreatorAchivesBuy : CreatorItems
    {
        [SerializeField] private List<AchivementItemBuy> _achivesItemBuy;

        [SerializeField] private Transform _transform;

        private readonly List<AchivementItemBuy> _createdAchives = new List<AchivementItemBuy>();

        private void Start()
        {
            InstantiateStoreItems(_achivesItemBuy, _transform);
            ActiveStartItems();

            foreach (var item in _createdItems)
            {
                if (_createdItems != null)
                    _createdAchives.Add((AchivementItemBuy)item);
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

        private void ChangeAchivement(AchivementItemBuy achivementBuy)
        {
            achivementBuy.gameObject.SetActive(false);

            for (int i = 0; i < _createdAchives.Count - 1; i++)
            {
                if (achivementBuy == _createdAchives[i])
                    _createdAchives[i + 1].gameObject.SetActive(true);
            }
        }
    }
}
