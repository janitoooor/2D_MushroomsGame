using Assets.Scripts.AllItems.ClickSkinItems;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class ClickableObject : MonoBehaviour
    {
        private readonly string _animtaionTrigerName = "Click";

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private List<PrefabClickSkin> _prefabs;
        [SerializeField] private ParticleSystem _particleSystem;

        private Animator _animator;

        private readonly SkinItemStore _skinItemStore = SkinItemStore.GetInstance();
        private readonly BankBalance _bankBalance = BankBalance.GetInstance();
        private readonly BankPassiveIncome _bankPassiveIncome = BankPassiveIncome.GetInstance();
        private long _coinsAddedByClick { get => _bankPassiveIncome.PassiveIncomeCoins * _itemIncome >= 1 ? Mathf.RoundToInt(_bankPassiveIncome.PassiveIncomeCoins * _itemIncome) : 1; }
        private float _itemIncome = 0.2f;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _skinItemStore.SkinItemSelectedInStore += ChangeClickableObject;
        }

        private void Start()
        {

        }

        private void OnDestroy()
        {
            _skinItemStore.SkinItemSelectedInStore -= ChangeClickableObject;
        }

        private void OnMouseDown()
        {
            ClickOnObject();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ClickOnObject();
        }

        private void ClickOnObject()
        {
            _bankBalance.AddCoins(_coinsAddedByClick);
            _audioSource.PlayOneShot(_audioClip);
            _animator.SetBool(_animtaionTrigerName, true);
            _particleSystem.Play();
        }

        private void ChangeClickableObject(ClickSkinItem skinItem)
        {
            foreach (var item in _prefabs)
            {
                if (skinItem.IndexItem == item.IndexItem)
                {
                    item.gameObject.SetActive(true);
                    ChangeIncomeByClick(skinItem);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }

        private void ChangeIncomeByClick(ClickSkinItem skinItem)
        {
            _itemIncome = skinItem.IncomeSkinItem;
        }
    }
}
