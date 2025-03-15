using UnityEngine;

namespace _PlayerResources
{
    public class PlayerResources : MonoBehaviour
    {
        public static PlayerResources instance;
        public int gold = 100; // Начальное количество золота

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool SpendGold(int amount)
        {
            if (gold >= amount)
            {
                gold -= amount;
                return true;
            }
            return false;
        }

        public void EarnGold(int amount)
        {
            gold += amount;
        }

        public int GetGold()
        {
            return gold;
        }
    }
}