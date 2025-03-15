using UnityEngine;

namespace _Waypoint
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private Vector3[] points;

        public Vector3[] Points => points;
        public Vector3 CurrentPosition => transform.position;

        private void OnDrawGizmos()
        {
            if (points == null || points.Length == 0) return;

            Gizmos.color = Color.black;

            for (int i = 0; i < points.Length; i++)
            {
                Vector3 worldPoint = CurrentPosition + points[i];

                // Отображаем сферу в каждой точке
                Gizmos.DrawWireSphere(worldPoint, 0.5f);

                // Соединяем линии между точками
                if (i < points.Length - 1)
                {
                    Gizmos.color = Color.gray;
                    Vector3 nextPoint = CurrentPosition + points[i + 1];
                    Gizmos.DrawLine(worldPoint, nextPoint);
                }
            }
        }
    }
}