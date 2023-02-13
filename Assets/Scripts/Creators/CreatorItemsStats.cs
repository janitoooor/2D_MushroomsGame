using Assets.Scripts.Shop;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creators
{
    class CreatorItemsStats : CreatorItems
    {

        [SerializeField] private List<ItemStats> _statsItems;
        [SerializeField] private Transform _transform;

        private void Start()
        {
            foreach (var item in _statsItems)
            {
                var obj = Instantiate(item, _transform.parent);
                obj.gameObject.SetActive(true);
            }
        }
    }
}
