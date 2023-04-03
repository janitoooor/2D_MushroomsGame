using UnityEngine;
using TMPro;

    class ItemText : MonoBehaviour
    {
        private TextMeshProUGUI _itemNameText;

        private void Awake()
        {
            _itemNameText = GetComponent<TextMeshProUGUI>();
        }

        public void ChangeText(string text)
        {
            _itemNameText.text = $"{text}";
        }

        public void ChangeColorText(Color color)
        {
            _itemNameText.color = color;
        }

        public void ChangeFontStyleText(FontStyles font)
        {
            _itemNameText.fontStyle = font;
        }

        public void ChangeFontText(TMP_FontAsset font)
        {
            _itemNameText.font = font;
        }

        public void ChangeSpriteAsset(TMP_SpriteAsset spriteAsset)
        {
            _itemNameText.spriteAsset = spriteAsset;
        }
    }
