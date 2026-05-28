using System;
using UnityEngine;

namespace CleanRefactor
{
    public sealed class PlayerPrefsPlayerRepository : IPlayerRepository
    {
        public int GetCoins()
        {
            return PlayerPrefs.GetInt("Coins", 500);
        }

        public void SaveCoins(int coins)
        {
            PlayerPrefs.SetInt("Coins", coins);
            PlayerPrefs.Save();
        }

        public int GetPlayerLevel()
        {
            return PlayerPrefs.GetInt("PlayerLevel", 1);
        }

        public int GetItemUses(string itemId)
        {
            return PlayerPrefs.GetInt(itemId, 0);
        }

        public void SaveItemUses(string itemId, int uses)
        {
            PlayerPrefs.SetInt(itemId, uses);
            PlayerPrefs.Save();
        }

        public bool HasDoubleCoins()
        {
            return PlayerPrefs.GetInt("HasDoubleCoins", 0) == 1;
        }

        public void SaveDoubleCoins()
        {
            PlayerPrefs.SetInt("HasDoubleCoins", 1);
            PlayerPrefs.Save();
        }
    }

}
