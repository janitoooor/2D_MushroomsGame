using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creators
{
    class CreatorSkinClickItem : CreatorItems
    {
        [SerializeField] private List<ClickSkinItem> _statsItems;
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
