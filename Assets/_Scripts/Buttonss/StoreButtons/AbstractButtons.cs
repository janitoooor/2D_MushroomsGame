using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class AbstractButtons : MonoBehaviour
{
    [SerializeField] protected private Sprite _buttonSelectedSprite;
    [SerializeField] protected private AudioClip _audioClip;
    [SerializeField] protected private TMP_FontAsset _font;

    private protected readonly string _nameAnimationTriggerClose = "CloseObject";
    private protected readonly string _audiosourceObjectName = "AudioSource";
    protected private AudioSource _audioSource;

    protected private TextMeshProUGUI _textButton;
    protected private Sprite _baseSprite;
    protected private Button _button;

    public virtual void OnClick()
    {
        PlayOneShot();
        ShowButtonSelected();
    }

    public virtual void PlayOneShot()
    {
        _audioSource.PlayOneShot(_audioClip);
    }

    private protected void SetFont()
    {
        _textButton = GetComponentInChildren<TextMeshProUGUI>();
        _textButton.font = _font;
    }

    public virtual void AddListeners()
    {
        _button.onClick.AddListener(OnClick);
    }

    public virtual void RemoveAllListeners()
    {
        _button.onClick.RemoveAllListeners();
    }

    public virtual void GetComponents()
    {
        _button = GetComponent<Button>();
        _audioSource = GameObject.Find(_audiosourceObjectName).GetComponent<AudioSource>();
        _baseSprite = _button.image.sprite;
    }

    public virtual void ShowButtonSelected(long none = 0)
    {

    }

    public virtual void SetButtonPressedState()
    {
        ChangeSpriteInteractable(_buttonSelectedSprite, false, Color.white);
    }
    public virtual void DisabledButtonPressedState()
    {
        ChangeSpriteInteractable(_baseSprite, true, Color.black);
    }

    private void ChangeSpriteInteractable(Sprite sprite, bool interactable, Color textColor)
    {
        _button.image.sprite = sprite;
        _button.interactable = interactable;

        try
        {
            _textButton.color = textColor;
        }
        catch
        {

        }
    }
}
