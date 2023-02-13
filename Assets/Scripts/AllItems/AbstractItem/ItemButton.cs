using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.StoreItem
{
    class ItemButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void AddListeners(UnityAction action)
        {
            _button.onClick.AddListener(action);
        }

        public void ChangeButtonInteractable(bool interactable)
        {
            _button.interactable = interactable;
        }
    }
}
