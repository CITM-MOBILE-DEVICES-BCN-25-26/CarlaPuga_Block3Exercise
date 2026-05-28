using System;

namespace CleanRefactor
{
    public sealed class ShopPresenter : IDisposable
    {
        private readonly IShopView view;
        private readonly PurchaseItemUseCase purchaseItemUseCase;
        private readonly IPlayerRepository playerRepository;

        private readonly ShopItem bomb;
        private readonly ShopItem shield;
        private readonly ShopItem doubleCoins;

        public ShopPresenter(
            IShopView view,
            PurchaseItemUseCase purchaseItemUseCase,
            IPlayerRepository playerRepository,
            ShopItem bomb,
            ShopItem shield,
            ShopItem doubleCoins)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.purchaseItemUseCase = purchaseItemUseCase ?? throw new ArgumentNullException(nameof(purchaseItemUseCase));
            this.playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            this.bomb = bomb;
            this.shield = shield;
            this.doubleCoins = doubleCoins;
        }

        public void Initialize()
        {
            view.PurchaseItemClicked += OnPurchaseItemClicked;
            RefreshUI();
        }

        public void Dispose()
        {
            view.PurchaseItemClicked -= OnPurchaseItemClicked;
        }

        void OnPurchaseItemClicked(ShopItemType itemType)
        {
            PurchaseItemResponse response = null;

            switch (itemType)
            {
                case ShopItemType.Bomb:
                    response = purchaseItemUseCase.Execute(bomb, "BombUses");
                    break;
                case ShopItemType.Shield:
                    response = purchaseItemUseCase.Execute(shield, "ShieldUses");
                    break;
                case ShopItemType.DoubleCoins:
                    response = purchaseItemUseCase.Execute(doubleCoins, "HasDoubleCoins");
                    break;
            }

            if (response != null)
            {
                view.SetFeedback(response.Message);
                if (response.Success)
                {
                    view.PlayPurchaseSound();
                }
                RefreshUI();
            }
        }

        void RefreshUI()
        {
            int currentCoins = playerRepository.GetCoins();
            view.SetCoins(currentCoins);

            Player player = new Player(currentCoins, playerRepository.GetPlayerLevel());

            view.SetBombButtonInteractable(bomb.CanBePurchased(player) == PurchaseItemStatus.Success);
            view.SetShieldButtonInteractable(shield.CanBePurchased(player) == PurchaseItemStatus.Success);
            view.SetDoubleCoinsButtonInteractable(doubleCoins.CanBePurchased(player) == PurchaseItemStatus.Success);
        }
    }
}
