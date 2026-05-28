using UnityEngine;

namespace CleanRefactor
{
    public interface IPlayerRepository
    {
        int GetCoins();
        void SaveCoins(int coins);

        int GetPlayerLevel();

        int GetItemUses(string itemId);
        void SaveItemUses(string itemId, int uses);

        bool HasDoubleCoins();
        void SaveDoubleCoins();
    }
}

