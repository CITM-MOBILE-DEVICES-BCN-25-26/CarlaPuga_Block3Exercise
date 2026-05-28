using System;

namespace CleanRefactor
{
    public sealed class Player
    {
        public int Coins { get; private set; }
        public int PlayerLevel { get; private set; }

        public Player(int coins, int playerLevel)
        {
            if (coins < 0)
                throw new ArgumentOutOfRangeException(nameof(coins), "Coins cannot be negative.");

            Coins = coins;
            PlayerLevel = playerLevel;
        }

        public void SpendCoins(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be grater than zero.");

            Coins -= amount;
        }
    }
}

