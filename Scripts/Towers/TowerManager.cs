using UnityEngine;

using _TowerLog;
using _PlayerResources;

namespace _TowerManager
{
    public class TowerManager : MonoBehaviour
    {
        public static TowerManager instance;

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

        public void BuildTower(GameObject place)
        {
            if (TowerLog.instance.selectedTowerPrefab == null)
            {
                Debug.Log("����� �� �������!");
                return;
            }

            if (place.transform.childCount > 0)
            {
                Debug.Log("�� ���� ������� ��� ��������� �����!");
                return;
            }

            if (!PlayerResources.instance.SpendGold(TowerLog.instance.selectedTowerCost))
            {
                Debug.Log("������������ ������!");
                return;
            }

            Instantiate(TowerLog.instance.selectedTowerPrefab, place.transform.position, Quaternion.identity, place.transform);
        }
    }
}
