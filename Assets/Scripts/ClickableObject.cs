using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    class ClickableObject : MonoBehaviour
    {
        private readonly string _animtaionTrigerName = "Click";

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;

        private Animator _animator;

        private readonly BankBalance _bankBalance = BankBalance.GetInstance();
        private readonly BankPassiveIncome _bankPassiveIncome = BankPassiveIncome.GetInstance();
        private long _coinsAddedByClick { get => _bankPassiveIncome.PassiveIncomeCoins >= 3 ? Mathf.RoundToInt(_bankPassiveIncome.PassiveIncomeCoins / 3f) : 1; }

        private void Start()
        {
            _animator = GetComponent<Animator>();
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
            _bankBalance.AddCoins(100000);
            _audioSource.PlayOneShot(_audioClip);
            _animator.SetBool(_animtaionTrigerName, true);
        }
    }
}
