using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

using _Projectile;
using _PlayerResources;
using _TowerManager;
using _UI_Tower;

namespace _TowerLog
{
    public class TowerLog : MonoBehaviour
    {
        public static TowerLog instance;

        [Header("�������� ��������� �����")]
        public float range = 5f;
        public float fireRate = 1f;
        public int damage = 10;
        public GameObject projectilePrefab;
        public float spawnOffsetY = 0.5f;
        public int upgradeCost = 50;
        public int sellValue = 30;
        public int level = 1;

        [Header("��������� �����")]
        public GameObject selectedTowerPrefab;
        public int selectedTowerCost;

        private Transform target;
        private float fireCountdown = 0f;
        private ObjectPool<_Projectile.Projectile> _projectilePool;
        private Transform projectileContainer;

        void Awake()
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

        void Start()
        {
            if (projectilePrefab == null)
            {
                Debug.LogError("[TowerLog] projectilePrefab �� �������� � ����������!");
                return;
            }

            GameObject projectilesContainer = GameObject.FindGameObjectWithTag("Projectiles");
            if (projectilesContainer != null)
            {
                projectileContainer = projectilesContainer.transform;
            }
            else
            {
                Debug.LogWarning("��������� � ����� 'Projectiles' �� ������!");
            }

            _projectilePool = new ObjectPool<_Projectile.Projectile>(
                () =>
                {
                    GameObject instance = Instantiate(projectilePrefab, projectileContainer);
                    _Projectile.Projectile proj = instance.GetComponent<_Projectile.Projectile>();
                    if (proj == null)
                    {
                        Debug.LogError("[TowerLog] � prefab ����������� ��������� Projectile!");
                        return null;
                    }
                    proj.Initialize(_projectilePool);
                    return proj;
                },
                projectile => projectile.gameObject.SetActive(true),
                projectile => projectile.gameObject.SetActive(false),
                projectile => Destroy(projectile.gameObject),
                true, 20, 50);
        }

        void Update()
        {
            FindTarget();
            if (target != null && fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }

        void FindTarget()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
            float shortestDistance = Mathf.Infinity;
            Transform nearestEnemy = null;

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = collider.transform;
                    }
                }
            }

            target = (nearestEnemy != null && shortestDistance <= range) ? nearestEnemy : null;
        }

        void Shoot()
        {
            if (_projectilePool == null)
            {
                Debug.LogError("[TowerLog] ��� �������� �� ������!");
                return;
            }

            _Projectile.Projectile projectile = _projectilePool.Get();
            if (projectile == null)
            {
                Debug.LogError("[TowerLog] �� ������� �������� ������ �� ����!");
                return;
            }

            Vector3 spawnPos = transform.position + new Vector3(0, spawnOffsetY, 0);
            projectile.transform.position = spawnPos;
            projectile.SetTarget(target);
            projectile.damage = damage;
        }

        public void UpgradeTower()
        {
            if (PlayerResources.instance.SpendGold(upgradeCost))
            {
                level++;
                range += 1f;
                fireRate += 0.5f;
                damage += 5;
                upgradeCost += 25;
                Debug.Log("����� ��������! �������: " + level);
            }
            else
            {
                Debug.Log("������������ ������ ��� ���������!");
            }
        }

        public void SellTower()
        {
            PlayerResources.instance.EarnGold(sellValue);
            Destroy(gameObject);
            Debug.Log("����� �������!");
        }

        public void SelectTower(GameObject towerPrefab, int cost)
        {
            selectedTowerPrefab = towerPrefab;
            selectedTowerCost = cost;
        }

        void OnMouseDown()
        {
            UI_Tower.instance.ShowTowerOptions(this);
        }
    }
}
