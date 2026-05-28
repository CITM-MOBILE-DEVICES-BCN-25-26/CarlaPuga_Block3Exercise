using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CleanRefactor.Tests
{
    public sealed class PurchaseItemUseCasesTests
    {
        [Test]
        public void When_BombPurchaseSucceeds_Expect_CoinsUpdated()
        {
            TestContext context = CreateContext(
                coins: 500,
                level: 1,
                bombUses: 0,
                shieldUses: 0,
                hasDoubleCoins: false);

            var bomb = new ShopItem("Bomb", 100, 3, context.Repository.BombUses);
            PurchaseItemResponse response = context.PurchaseItemUseCase.Execute(bomb, "BombUses");

            Assert.IsTrue(response.Success);
            Assert.AreEqual(PurchaseItemStatus.Success, response.Status);
            Assert.AreEqual(400, context.Repository.Coins);
            Assert.AreEqual(1, context.Repository.BombUses);
            Assert.AreEqual(1, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_BombPurchaseFailsByLackOfCoins_Expect_PurchaseFails()
        {
            TestContext context = CreateContext(
                coins: 50,
                level: 1,
                bombUses: 0,
                shieldUses: 0,
                hasDoubleCoins: false);

            var bomb = new ShopItem("Bomb", 100, 3, context.Repository.BombUses);
            PurchaseItemResponse response = context.PurchaseItemUseCase.Execute(bomb, "BombUses");

            Assert.IsFalse(response.Success);
            Assert.AreEqual(PurchaseItemStatus.NotEnoughCoins, response.Status);
            Assert.AreEqual(50, context.Repository.Coins);
            Assert.AreEqual(0, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_BombPurchaseFailsWhenMaximumUsesReached_Expect_PurchaseFails()
        {
            TestContext context = CreateContext(
                coins: 500,
                level: 1,
                bombUses: 3,
                shieldUses: 0,
                hasDoubleCoins: false);

            var bomb = new ShopItem("Bomb", 100, 3, context.Repository.BombUses);
            PurchaseItemResponse response = context.PurchaseItemUseCase.Execute(bomb, "BombUses");

            Assert.IsFalse(response.Success);
            Assert.AreEqual(PurchaseItemStatus.MaxCapacityReached, response.Status);
            Assert.AreEqual(0, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_ShieldPurchaseSucceeds_Expect_CoinsUpdated()
        {
            TestContext context = CreateContext(
                coins: 500,
                level: 1,
                bombUses: 0,
                shieldUses: 0,
                hasDoubleCoins: false);

            var shield = new ShopItem("Shield", 150, 2, context.Repository.ShieldUses);
            PurchaseItemResponse response = context.PurchaseItemUseCase.Execute(shield, "ShieldUses");

            Assert.IsTrue(response.Success);
            Assert.AreEqual(PurchaseItemStatus.Success, response.Status);
            Assert.AreEqual(350, context.Repository.Coins);
            Assert.AreEqual(1, context.Repository.ShieldUses);
            Assert.AreEqual(1, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_ShieldPurchaseFailsWhenMaximumUsesReached_Expect_PurchaseFails()
        {
            TestContext context = CreateContext(
                coins: 500,
                level: 1,
                bombUses: 0,
                shieldUses: 2,
                hasDoubleCoins: false);

            var shield = new ShopItem("Shield", 150, 2, context.Repository.ShieldUses);
            PurchaseItemResponse response = context.PurchaseItemUseCase.Execute(shield, "ShieldUses");

            Assert.IsFalse(response.Success);
            Assert.AreEqual(PurchaseItemStatus.MaxCapacityReached, response.Status);
            Assert.AreEqual(0, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_DoubleCoinsPurchaseSucceeds_Expect_CoinsUpdated()
        {
            TestContext context = CreateContext(
                coins: 500,
                level: 5,
                bombUses: 0,
                shieldUses: 0,
                hasDoubleCoins: false);

            var doubleCoins = new DoubleCoins("DoubleCoins", 300, 1, 0);
            PurchaseItemResponse response = context.PurchaseItemUseCase.Execute(doubleCoins, "HasDoubleCoins");

            Assert.IsTrue(response.Success);
            Assert.AreEqual(PurchaseItemStatus.Success, response.Status);
            Assert.AreEqual(200, context.Repository.Coins);
            Assert.IsTrue(context.Repository.HasDoubleCoinsPurchased);
            Assert.AreEqual(1, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_DoubleCoinsLevelIsTooLow_Expect_PurchaseFails()
        {
            TestContext context = CreateContext(
                coins: 500,
                level: 1,
                bombUses: 0,
                shieldUses: 0,
                hasDoubleCoins: false);

            var doubleCoins = new DoubleCoins("DoubleCoins", 300, 1, 0);
            PurchaseItemResponse response = context.PurchaseItemUseCase.Execute(doubleCoins, "HasDoubleCoins");

            Assert.IsFalse(response.Success);
            Assert.AreEqual(PurchaseItemStatus.InsufficientLevel, response.Status);
            Assert.AreEqual(0, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_DoubleCoinsAlreadyOwned_Expect_PurchaseFails()
        {
            TestContext context = CreateContext(
                coins: 500,
                level: 5,
                bombUses: 0,
                shieldUses: 0,
                hasDoubleCoins: true);

            var doubleCoins = new DoubleCoins("DoubleCoins", 300, 1, 1);
            PurchaseItemResponse response = context.PurchaseItemUseCase.Execute(doubleCoins, "HasDoubleCoins");

            Assert.IsFalse(response.Success);
            Assert.AreEqual(PurchaseItemStatus.AlreadyOwned, response.Status);
            Assert.AreEqual(0, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_PurchaseSuccesful_Expect_PlayerCoinsUpdated()
        {
            TestContext context = CreateContext(
                coins: 500,
                level: 1,
                bombUses: 0,
                shieldUses: 0,
                hasDoubleCoins: false);

            var bomb = new ShopItem("Bomb", 100, 1, 0);
            context.PurchaseItemUseCase.Execute(bomb, "BombUses");
            Assert.AreEqual(400, context.Repository.Coins);
        }

        [Test]
        public void When_PurchaseSuccessful_Expect_PlayerSaved()
        {
            TestContext context = CreateContext(
                coins: 500,
                level: 1,
                bombUses: 0,
                shieldUses: 0,
                hasDoubleCoins: false);

            var bomb = new ShopItem("Bomb", 100, 1, 0);
            context.PurchaseItemUseCase.Execute(bomb, "BombUses");
            Assert.AreEqual(1, context.Repository.SaveCallsCount);
        }

        sealed class TestContext
        {
            public InMemoryPlayerRepository Repository { get; }
            public PurchaseItemUseCase PurchaseItemUseCase { get; }

            public TestContext(
                InMemoryPlayerRepository repository,
                PurchaseItemUseCase purchaseItemUseCase)
            {
                Repository = repository;
                PurchaseItemUseCase = purchaseItemUseCase;
            }
        }

        static TestContext CreateContext(int coins, int level, int bombUses, int shieldUses, bool hasDoubleCoins)
        {
            var repository = new InMemoryPlayerRepository(coins, level, bombUses, shieldUses, hasDoubleCoins);
            var useCase = new PurchaseItemUseCase(repository);
            return new TestContext(repository, useCase);
        }

        sealed class InMemoryPlayerRepository : IPlayerRepository
        {
            public int Coins { get; private set; }
            public int Level { get; private set; }
            public int BombUses { get; private set; }
            public int ShieldUses { get; private set; }
            public bool HasDoubleCoinsPurchased { get; private set; }
            public int SaveCallsCount { get; private set; }

            public InMemoryPlayerRepository(int coins, int level, int bombUses, int shieldUses, bool hasDoubleCoins)
            {
                Coins = coins;
                Level = level;
                BombUses = bombUses;
                ShieldUses = shieldUses;
                HasDoubleCoinsPurchased = hasDoubleCoins;
                SaveCallsCount = 0;
            }

            public int GetCoins() => Coins;

            public void SaveCoins(int coins)
            {
                Coins = coins;
                SaveCallsCount++;
            }

            public int GetPlayerLevel() => Level;

            public int GetItemUses(string itemId)
            {
                if (itemId == "BombUses")
                {
                    return BombUses;
                }
                    
                if (itemId == "ShieldUses")
                {
                    return ShieldUses;
                }
                    
                return 0;
            }

            public void SaveItemUses(string itemId, int uses)
            {
                if (itemId == "BombUses")
                {
                    BombUses = uses;
                }
                else if (itemId == "ShieldUses")
                {
                    ShieldUses = uses;
                }   
            }

            public bool HasDoubleCoins() => HasDoubleCoinsPurchased;
            public void SaveDoubleCoins() => HasDoubleCoinsPurchased = true;
        }

    }
}

