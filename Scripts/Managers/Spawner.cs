using UnityEngine;
using UnityEngine.Pool;

using _Waypoint;

namespace _Spawner
{
    public class Spawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int enemyCount = 10;
        [SerializeField] private GameObject enemyPrefab;

        [Header("Fixed Delay")]
        [SerializeField] private float delayBtwSpawns = 1f;

        private float _spawnTimer;
        private int _enemiesSpawned;

        private ObjectPool<GameObject> _enemyPool;
        private Waypoint _waypoint; // ��������� �� Waypoint

        private void Awake()
        {
            _enemyPool = new ObjectPool<GameObject>(
                CreateEnemy,
                OnGetFromPool,
                OnReleaseToPool,
                OnDestroyEnemy,
                true, 10, 50);

            _waypoint = GameObject.Find("Spawn_Enemis").GetComponent<Waypoint>(); // �������� Waypoint
        }

        private void Update()
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer < 0)
            {
                _spawnTimer = delayBtwSpawns;
                if (_enemiesSpawned < enemyCount)
                {
                    _enemiesSpawned++;
                    SpawnEnemy();
                }
            }
        }

        private GameObject CreateEnemy()
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            return enemy;
        }

        private void OnGetFromPool(GameObject enemy)
        {
            enemy.SetActive(true);
        }

        private void OnReleaseToPool(GameObject enemy)
        {
            enemy.SetActive(false);
        }

        private void OnDestroyEnemy(GameObject enemy)
        {
            Destroy(enemy);
        }

        private void SpawnEnemy()
        {
            GameObject newInstance = _enemyPool.Get();

            if (_waypoint != null && _waypoint.Points.Length > 0)
            {
                Vector3 spawnPos = _waypoint.CurrentPosition + _waypoint.Points[0];
                newInstance.transform.position = spawnPos;

                // ������� ��������� � ����� WeakEnemy
                GameObject weakEnemyContainer = GameObject.FindGameObjectWithTag("WeakEnemy");
                if (weakEnemyContainer != null)
                {
                    newInstance.transform.parent = weakEnemyContainer.transform;
                }
                else
                {
                    Debug.LogWarning("��������� � ����� 'WeakEnemy' �� ������!");
                }
            }
            else
            {
                Debug.LogWarning("Waypoint �� ������ ��� ������ Points ����!");
            }
        }

        public void ReturnEnemyToPool(GameObject enemy)
        {
            _enemyPool.Release(enemy);
        }
    }
}