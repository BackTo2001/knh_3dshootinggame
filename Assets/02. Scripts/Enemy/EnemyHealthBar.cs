using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider; // ü�� �����̴�
    private Transform cameraTransform;

    private void Start()
    {
        // ī�޶� ����
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // ������ ȿ��: �׻� ī�޶� ���ϵ��� ȸ��
        transform.LookAt(transform.position + cameraTransform.forward);
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        // ü�� �����̴� ������Ʈ
        healthSlider.value = currentHealth / maxHealth;
    }
}
