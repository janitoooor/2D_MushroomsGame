using UnityEngine;

namespace Assets.Scripts.AllItems.ClickSkinItems
{
    class PrefabClickSkin : MonoBehaviour
    {
        [SerializeField] private int _index;
        public int IndexItem { get => _index; }
    }
}
