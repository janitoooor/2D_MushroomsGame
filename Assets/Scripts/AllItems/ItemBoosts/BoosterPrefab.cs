using UnityEngine;

namespace Assets.Scripts.ItemBoosts
{
    class BoosterPrefab : MonoBehaviour
    {
        [SerializeField] private int _prefabLvl;
        public int PrefabLvl { get => _prefabLvl; }
    }
}
