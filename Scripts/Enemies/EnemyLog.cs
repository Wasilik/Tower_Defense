using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using _Waypoint;

namespace _EnemyLog
{
    public class EnemyLog : MonoBehaviour
    {
        [Header("Movement")]
        public float speed = 2f;
        private float baseSpeed; // ������� ��������
        public Animator animator;
        private Vector3[] points;
        private int currentWaypointIndex = 0;
        private bool isDead = false;

        [Header("Health")]
        public float maxHP = 100f;
        private float currentHP;

        [Header("HP Bar")]
        public Image hpFillImage;
        public Canvas hpBarCanvas;

        [Header("Effects")]
        private bool isSlowed = false;
        private bool isPoisoned = false;

        void Start()
        {
            baseSpeed = speed; // ��������� ����������� ��������
            currentHP = maxHP;
            UpdateHPBar();

            // �������� ������� �� Waypoint
            Waypoint waypointComponent = GameObject.Find("Spawn_Enemis").GetComponent<Waypoint>();
            if (waypointComponent != null)
            {
                Vector3 basePosition = waypointComponent.CurrentPosition;
                points = new Vector3[waypointComponent.Points.Length];

                for (int i = 0; i < waypointComponent.Points.Length; i++)
                {
                    points[i] = basePosition + waypointComponent.Points[i];
                }
            }
            else
            {
                Debug.LogError("Waypoint �� ������ �� Spawn_Enemies!");
            }
        }

        void Update()
        {
            if (isDead || points == null || points.Length == 0) return;

            MoveToWaypoint();

            // ������������ HP ��� � ������
            if (hpBarCanvas != null)
            {
                hpBarCanvas.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            }
        }

        void MoveToWaypoint()
        {
            Vector3 target = points[currentWaypointIndex];
            Vector3 direction = (target - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            // �������� ��������
            if (animator != null)
            {
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    animator.SetTrigger(direction.x > 0 ? "MoveRight" : "MoveLeft");
                }
                else
                {
                    animator.SetTrigger(direction.y > 0 ? "MoveUp" : "MoveDown");
                }
            }

            // ������� � ��������� �����
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= points.Length)
                {
                    Destroy(gameObject); // ���� ����� �� ����� ��������
                }
            }
        }

        // --- ������ �������� ---
        public void TakeDamage(float damage)
        {
            if (isDead) return;

            currentHP -= damage;
            UpdateHPBar();

            if (currentHP <= 0)
            {
                Die();
            }
        }

        void UpdateHPBar()
        {
            if (hpFillImage != null)
            {
                hpFillImage.fillAmount = currentHP / maxHP;
            }
        }

        void Die()
        {
            isDead = true;

            if (animator != null)
            {
                animator.SetTrigger("Die"); // �������� ������
            }

            Destroy(gameObject, 1.5f); // ������� ����� ����� ������ (� ���������)
        }

        // --- ������� ---
        public void ApplySlow(float slowAmount, float duration)
        {
            if (isSlowed) return; // ���� ��� ��������, �� ���������

            isSlowed = true;
            speed *= slowAmount; // ��������� ��������

            StartCoroutine(RemoveSlow(duration));
        }

        private IEnumerator RemoveSlow(float duration)
        {
            yield return new WaitForSeconds(duration);
            speed = baseSpeed; // ���������� ��������
            isSlowed = false;
        }

        public void ApplyPoison(float damagePerSecond, float duration)
        {
            if (isPoisoned) return;

            isPoisoned = true;
            StartCoroutine(PoisonEffect(damagePerSecond, duration));
        }

        private IEnumerator PoisonEffect(float damagePerSecond, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                TakeDamage(damagePerSecond * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            isPoisoned = false;
        }
    }
}