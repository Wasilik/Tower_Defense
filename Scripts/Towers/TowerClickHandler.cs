using UnityEngine;
using UnityEngine.EventSystems;

using _TowerLog;
using _UI_Tower;

public class TowerClickHandler : MonoBehaviour, IPointerClickHandler
{
    private TowerLog tower;

    private void Awake()
    {
        tower = GetComponent<TowerLog>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (tower != null && UI_Tower.instance != null)
        {
            UI_Tower.instance.ShowTowerOptions(tower);

            Debug.Log("Показано");
        }
        Debug.Log("Показано");
    }
}