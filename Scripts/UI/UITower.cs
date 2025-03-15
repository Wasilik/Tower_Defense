using UnityEngine;

using _TowerLog;

namespace _UI_Tower
{
    public class UI_Tower : MonoBehaviour
    {
        public static UI_Tower instance;
        public GameObject upgradeButton;
        public GameObject sellButton;
        private TowerLog selectedTower;

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

        public void ShowTowerOptions(TowerLog tower)
        {
            selectedTower = tower;
            upgradeButton.SetActive(true);
            sellButton.SetActive(true);
        }

        public void UpgradeSelectedTower()
        {
            if (selectedTower != null)
            {
                selectedTower.UpgradeTower();
                HideUI();
            }
        }

        public void SellSelectedTower()
        {
            if (selectedTower != null)
            {
                selectedTower.SellTower();
                HideUI();
            }
        }

        public void HideUI()
        {
            upgradeButton.SetActive(false);
            sellButton.SetActive(false);
            selectedTower = null;
        }
    }
}
