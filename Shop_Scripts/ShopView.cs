using System;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRefactor
{
    public sealed class ShopView : MonoBehaviour, IShopView
    {
        [Header("UI")]
        [SerializeField] private Text coinsText;
        [SerializeField] private Text feedbackText;
        [SerializeField] private Button bombButton;
        [SerializeField] private Button shieldButton;
        [SerializeField] private Button doubleCoinsButton;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;

        public event Action<ShopItemType> PurchaseItemClicked;

        void Awake()
        {
            bombButton.onClick.AddListener(OnBombButtonClicked);
            shieldButton.onClick.AddListener(OnShieldButtonClicked);
            doubleCoinsButton.onClick.AddListener(OnDoubleCoinsButtonClicked);
        }

        void OnDestroy()
        {
            bombButton.onClick.RemoveListener(OnBombButtonClicked);
            shieldButton.onClick.RemoveListener(OnShieldButtonClicked);
            doubleCoinsButton.onClick.RemoveListener(OnDoubleCoinsButtonClicked);
        }

        public void SetCoins(int coins)
        {
            coinsText.text = $"Coins: {coins}";
        }

        public void SetFeedback(string message)
        {
            feedbackText.text = message;
        }

        public void SetBombButtonInteractable(bool interactable)
        {
            bombButton.interactable = interactable;
        }

        public void SetShieldButtonInteractable(bool interactable)
        {
            shieldButton.interactable = interactable;
        }

        public void SetDoubleCoinsButtonInteractable(bool interactable)
        {
            doubleCoinsButton.interactable = interactable;
        }

        void OnBombButtonClicked()
        {
            PurchaseItemClicked?.Invoke(ShopItemType.Bomb);
        }

        void OnShieldButtonClicked()
        {
            PurchaseItemClicked?.Invoke(ShopItemType.Shield);
        }

        void OnDoubleCoinsButtonClicked()
        {
            PurchaseItemClicked?.Invoke(ShopItemType.DoubleCoins);
        }

        public void PlayPurchaseSound()
        {
            audioSource.Play();
        }
    }
}

