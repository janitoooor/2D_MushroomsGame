using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.StoreItem
{
    class ItemImage : MonoBehaviour
    {
        private Image _itemImage;

        private void Awake()
        {
            _itemImage = GetComponent<Image>();
        }

        public void ChangeImageSprite(Sprite newSprite)
        {
            _itemImage.sprite = newSprite;
        }

        public void ChangeImageColor(Color color)
        {
            _itemImage.color = color;
        }
    }
}
