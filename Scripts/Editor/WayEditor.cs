using UnityEngine;
using UnityEditor;

using _Waypoint;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
    private void OnSceneGUI()
    {
        Waypoint waypoint = (Waypoint)target;
        if (waypoint.Points == null || waypoint.Points.Length == 0) return;

        Handles.color = Color.cyan;

        for (int i = 0; i < waypoint.Points.Length; i++)
        {
            EditorGUI.BeginChangeCheck();

            // ���������� ������� ������� ������� �����
            Vector3 currentWaypointPoint = waypoint.CurrentPosition + waypoint.Points[i];

            // ������ ����� � ����� �����
            Handles.SphereHandleCap(0, currentWaypointPoint, Quaternion.identity, 0.2f, EventType.Repaint);

            // ����� ����� ����������� ����� (PositionHandle)
            Vector3 newWaypointPoint = Handles.PositionHandle(currentWaypointPoint, Quaternion.identity);

            // ���������� ����� �����
            Handles.Label(currentWaypointPoint + new Vector3(0.35f, -0.35f, 0), $"{i + 1}", new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                fontSize = 16,
                normal = { textColor = Color.white }
            });

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(waypoint, "Move Waypoint");
                waypoint.Points[i] = newWaypointPoint - waypoint.CurrentPosition;
                EditorUtility.SetDirty(waypoint);
            }
        }
    }
}