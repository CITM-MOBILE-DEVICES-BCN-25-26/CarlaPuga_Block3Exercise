
namespace CleanRefactor
{
    public enum ShopItemType
    {
        Bomb,
        Shield,
        DoubleCoins
    }

    public class ShopItem
    {
        public string Name { get; private set; }
        public int Cost { get; private set; }
        public int MaxCapacity { get; private set; }
        public int CurrentQuantity { get; private set; }

        public ShopItem(string name, int cost, int maxCapacity, int currentQuantity)
        {
            Name = name;
            Cost = cost;
            MaxCapacity = maxCapacity;
            CurrentQuantity = currentQuantity;
        }

        public void AddQuantity()
        {
            CurrentQuantity++;
        }

        public virtual PurchaseItemStatus CanBePurchased(Player player)
        {
            if (player.Coins < Cost)
            {
                return PurchaseItemStatus.NotEnoughCoins;
            }

            if (CurrentQuantity >= MaxCapacity)
            {
                return PurchaseItemStatus.MaxCapacityReached;
            }

            return PurchaseItemStatus.Success;
        }
    }

    public class DoubleCoins : ShopItem
    {
        public DoubleCoins(string name, int cost, int maxCapacity, int currentQuantity)
            : base(name, cost, maxCapacity, currentQuantity) { }

        public override PurchaseItemStatus CanBePurchased(Player player)
        {
            if (player.PlayerLevel < 5)
            {
                return PurchaseItemStatus.InsufficientLevel;
            }

            PurchaseItemStatus baseStatus = base.CanBePurchased(player);

            if (baseStatus == PurchaseItemStatus.MaxCapacityReached)
            {
                return PurchaseItemStatus.AlreadyOwned;
            }

            return baseStatus;
        }
    }
}

