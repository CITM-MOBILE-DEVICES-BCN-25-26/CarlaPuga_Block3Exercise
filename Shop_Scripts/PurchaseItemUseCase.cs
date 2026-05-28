using System;

namespace CleanRefactor
{
    public sealed class PurchaseItemUseCase
    {
        readonly IPlayerRepository playerRepository;

        public PurchaseItemUseCase(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public PurchaseItemResponse Execute(ShopItem item, string itemId)
        {
            Player player = new Player(playerRepository.GetCoins(), playerRepository.GetPlayerLevel());

            PurchaseItemStatus status = item.CanBePurchased(player);

            if (status == PurchaseItemStatus.Success)
            {
                player.SpendCoins(item.Cost);
                item.AddQuantity();

                playerRepository.SaveCoins(player.Coins);

                if (itemId == "HasDoubleCoins")
                {
                    playerRepository.SaveDoubleCoins();
                }
                else
                {
                    playerRepository.SaveItemUses(itemId, item.CurrentQuantity);
                }

                return new PurchaseItemResponse(PurchaseItemStatus.Success, $"{item.Name} purchased!");
            }

            switch (status)
            {
                case PurchaseItemStatus.NotEnoughCoins:
                    return new PurchaseItemResponse(status, $"Not enough coins for {item.Name}");
                case PurchaseItemStatus.MaxCapacityReached:
                    return new PurchaseItemResponse(status, $"{item.Name} already at max uses");
                case PurchaseItemStatus.InsufficientLevel:
                    return new PurchaseItemResponse(status, $"Insufficient level for {item.Name}");
                case PurchaseItemStatus.AlreadyOwned:
                    return new PurchaseItemResponse(status, $"{item.Name} already purchased");
                default:
                    return new PurchaseItemResponse(status, "Cannot purchase item");
            }
        }
    }
}

