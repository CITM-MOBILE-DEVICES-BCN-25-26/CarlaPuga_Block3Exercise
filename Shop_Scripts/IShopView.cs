using System;

namespace CleanRefactor
{
    public interface IShopView
    {
        event Action<ShopItemType> PurchaseItemClicked;

        void SetCoins(int coins);
        void SetFeedback(string message);
        void SetBombButtonInteractable(bool interactable);
        void SetShieldButtonInteractable(bool interactable);
        void SetDoubleCoinsButtonInteractable(bool interactable);
        void PlayPurchaseSound();
    }
}

