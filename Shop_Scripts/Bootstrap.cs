using UnityEngine;

namespace CleanRefactor
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] private ShopView shopView;

        [Header("Costs")]
        [SerializeField] private int bombCost = 100;
        [SerializeField] private int shieldCost = 150;
        [SerializeField] private int doubleCoinsCost = 300;

        private ShopPresenter presenter;

        private void Awake()
        {
            IPlayerRepository playerRepository = new PlayerPrefsPlayerRepository();

            var bomb = new ShopItem("Bomb", bombCost, 3, playerRepository.GetItemUses("BombUses"));
            var shield = new ShopItem("Shield", shieldCost, 2, playerRepository.GetItemUses("ShieldUses"));
            var doubleCoins = new DoubleCoins("DoubleCoins", doubleCoinsCost, 1, playerRepository.HasDoubleCoins() ? 1 : 0);

            var purchaseItemUseCase = new PurchaseItemUseCase(playerRepository);

            presenter = new ShopPresenter(
                shopView,
                purchaseItemUseCase,
                playerRepository,
                bomb,
                shield,
                doubleCoins);

            presenter.Initialize();
        }

        void OnDestroy()
        {
            presenter?.Dispose();
        }
    }
}

