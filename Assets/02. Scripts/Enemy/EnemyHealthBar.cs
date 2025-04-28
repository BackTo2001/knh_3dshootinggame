using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider; // 체력 슬라이더
    private Transform cameraTransform;

    private void Start()
    {
        // 카메라 참조
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // 빌보드 효과: 항상 카메라를 향하도록 회전
        transform.LookAt(transform.position + cameraTransform.forward);
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        // 체력 슬라이더 업데이트
        healthSlider.value = currentHealth / maxHealth;
    }
}
