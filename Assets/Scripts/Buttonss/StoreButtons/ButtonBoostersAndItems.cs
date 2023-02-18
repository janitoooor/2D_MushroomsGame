using Assets.Scripts.Buttons.StoreButtons;
using UnityEngine;

namespace Assets.Scripts.Buttonss.StoreButtons
{
    class ButtonBoostersAndItems : AbstractButtons
    {
        public delegate void ButtonPressed(ButtonBoostersAndItems buttonMenu);
        public event ButtonPressed ButtonPresseds;

        private readonly BoostersAndItems _upgratesAndItems = BoostersAndItems.GetInstance();
        [Space]
        [SerializeField] private GameObject _layer;
        [Space]
        [SerializeField] private GameObject _layerButtons;
        [Space]
        [SerializeField] private int _index;

        private Animator _layerAnimator;
        private Animator _layerButtonsAnimator;

        private void Awake()
        {
            GetComponents();
        }

        private void Start()
        {
            SetBase();
        }

        private void OnEnable()
        {
            ButtonPresseds += _upgratesAndItems.SetPressedButon;
            _upgratesAndItems.ButtonBoostersOrItemsPressed += CloseLayer;
        }

        private void OnDisable()
        {
            ButtonPresseds -= _upgratesAndItems.SetPressedButon;
            _upgratesAndItems.ButtonBoostersOrItemsPressed -= CloseLayer;
        }

        private void OnDestroy()
        {
            RemoveAllListeners();
        }

        public override void OnClick()
        {
            base.OnClick();
            ButtonPresseds?.Invoke(this);
        }

        private void CloseLayer(ButtonBoostersAndItems button)
        {
            if (button == this)
            {
                _layer.SetActive(true);
                SetButtonPressedState();
                if (_index == 0)
                    _layerButtons.SetActive(true);
                else
                    _layerButtonsAnimator.SetTrigger(_nameAnimationTriggerClose);
            }
            else
            {
                _layerAnimator.SetTrigger(_nameAnimationTriggerClose);
                DisabledButtonPressedState();
            }
        }

        public override void GetComponents()
        {
            base.GetComponents();
            _layerAnimator = _layer.GetComponent<Animator>();
            _layerButtonsAnimator = _layerButtons.GetComponent<Animator>();
        }

        private void SetBase()
        {
            AddListeners();
            SetFont();
            ShowButtonSelected();

            Invoke(nameof(SetStartLayer), Time.fixedDeltaTime);
        }

        private void SetStartLayer()
        {
            if (_index == 0)
            {
                SetButtonPressedState();
                _layer.SetActive(true);
                _layerButtons.SetActive(true);
            }
            else
            {
                _layer.SetActive(false);
            }
        }
    }
}
