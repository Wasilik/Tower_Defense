using UnityEngine;
using UnityEngine.UI;

namespace _HPBar
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        public void UpdateHP(float currentHP, float maxHP)
        {
            fillImage.fillAmount = currentHP / maxHP;
        }

        private void LateUpdate()
        {
            // HP ��� ������ ������� �� ������
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }
}