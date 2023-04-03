using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class ItemButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void RemoveAllListeners()
    {
        _button.onClick.RemoveAllListeners();
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
