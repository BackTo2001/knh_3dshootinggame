using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    [SerializeField] private PlayerStat _playerStat; // PlayerStat 참조
    [SerializeField] private PlayerDataSO _playerData; // PlayerDataSO 참조

    public Slider StaminaSlider;
    public Slider BombThrowSlider;
    public TextMeshProUGUI BombText;
    public TextMeshProUGUI BulletText;


    private void Start()
    {
        if (StaminaSlider != null)
        {
            StaminaSlider.value = 1f;
        }
        if (BombThrowSlider != null)
        {
            BombThrowSlider.value = 0f; // 초기화
            BombThrowSlider.gameObject.SetActive(false); // 초기에는 비활성화
        }
    }
    public void RefreshStaminaSlider(float currentStamina, float MaxStamina)
    {
        StaminaSlider.value = currentStamina / MaxStamina;
    }
    public void ShowBombThrowSlider(bool show)
    {
        if (BombThrowSlider != null)
        {
            BombThrowSlider.gameObject.SetActive(show); // Slider 활성화/비활성화
        }
    }
    public void UpdateBombThrowPower(float normalizedPower)
    {
        if (BombThrowSlider != null)
        {
            BombThrowSlider.value = normalizedPower; // 0~1로 정규화된 값 업데이트
        }
    }

    public void RefreshBulletText(int currentBulletCount, int maxBulletCount)
    {
        BulletText.text = $"{currentBulletCount} / {maxBulletCount}";
    }

    public void RefreshBombText(int currentBombCount, int maxBombCount)
    {
        BombText.text = $"{currentBombCount} / {maxBombCount}";
    }


    private void Update()
    {
        // PlayerMove에서 스태미나 값을 받아와 UI 업데이트
        if (_playerStat != null && StaminaSlider != null)
        {
            StaminaSlider.value = _playerStat.CurrentStamina / _playerData.MaxStamina; // 0~1로 정규화
        }
    }
}
