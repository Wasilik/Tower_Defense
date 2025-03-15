using UnityEngine;
using UnityEngine.Pool;

using _EnemyLog;

namespace _Projectile
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 5f;
        public int damage = 10;
        public float lifetime = 5f;
        private Transform target;

        public bool slowEffect = false;
        public bool poisonEffect = false;
        public float SlowAmount = 0;
        public float SlowDuration = 0;
        public float PoisonAmount = 0;
        public float PoisonDuration = 0;

        private ObjectPool<Projectile> _pool;
        private bool _isReleased = false;
        private float _timeAlive;

        [Header("Настройки спавна пули")]
        public float spawnOffsetX = 0f;
        public float spawnOffsetY = 0.5f;

        public void Initialize(ObjectPool<Projectile> pool)
        {
            _pool = pool;
            _isReleased = false;
            _timeAlive = 0f;
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            _isReleased = false;
            _timeAlive = 0f;
            transform.position += new Vector3(spawnOffsetX, spawnOffsetY, 0);
        }

        void Update()
        {
            if (target == null || _timeAlive >= lifetime)
            {
                ReturnToPool();
                return;
            }

            _timeAlive += Time.deltaTime;
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isReleased) return;

            if (collision.transform == target)
            {
                _EnemyLog.EnemyLog enemy = collision.GetComponent<_EnemyLog.EnemyLog>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    if (slowEffect) enemy.ApplySlow(SlowAmount, SlowDuration);
                    if (poisonEffect) enemy.ApplyPoison(PoisonAmount, PoisonDuration);
                }
                ReturnToPool();
            }
        }

        private void ReturnToPool()
        {
            if (_isReleased || !gameObject.activeSelf) return;
            _isReleased = true;
            gameObject.SetActive(false);
            _pool.Release(this);
        }
    }
}