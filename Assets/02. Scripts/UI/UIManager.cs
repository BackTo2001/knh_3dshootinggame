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
    [SerializeField] private Camera _minimapCamera;

    public Slider StaminaSlider;
    public Slider HealthSlider;
    public Slider BombThrowSlider;
    public Slider ReloadSlider;
    public TextMeshProUGUI BombText;
    public TextMeshProUGUI BulletText;
    public TextMeshProUGUI ReloadText;
    public Button ZoomInButton;
    public Button ZoomOutButton;



    private void Start()
    {
        // Cursor 처리
        Cursor.lockState = CursorLockMode.Locked;

        if (StaminaSlider != null)
        {
            StaminaSlider.value = 1f;
        }
        if (HealthSlider != null)
        {
            HealthSlider.value = 1f; // 초기화
        }
        if (BombThrowSlider != null)
        {
            BombThrowSlider.value = 0f; // 초기화
            BombThrowSlider.gameObject.SetActive(false); // 초기에는 비활성화
        }
        if (ReloadSlider != null)
        {
            ReloadSlider.value = 0f; // 초기화
            ReloadSlider.gameObject.SetActive(false); // 초기에는 비활성화
        }

        // Zoom 버튼 클릭 이벤트 등록
        ZoomInButton.onClick.AddListener(ZoomIn);
        ZoomOutButton.onClick.AddListener(ZoomOut);
    }
    private void Update()
    {
        // PlayerMove에서 스태미나 값을 받아와 UI 업데이트
        if (_playerStat != null && StaminaSlider != null)
        {
            StaminaSlider.value = _playerStat.CurrentStamina / _playerData.MaxStamina; // 0~1로 정규화
        }
        if (_playerStat != null && HealthSlider != null)
        {
            HealthSlider.value = _playerStat.CurrentHealth / _playerData.MaxHealth; // 0~1로 정규화
        }

        // ZoomIn / ZoomOut 키보드
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus)) // + 키
        {
            ZoomIn();
        }
        if (Input.GetKeyDown(KeyCode.Minus)) // - 키
        {
            ZoomOut();
        }
    }

    public void RefreshStaminaSlider(float currentStamina, float MaxStamina)
    {
        StaminaSlider.value = currentStamina / MaxStamina;
    }

    public void RefreshHealthSlider(float currentHealth, float maxHealth)
    {
        HealthSlider.value = currentHealth / maxHealth; // 0~1로 정규화
    }

    public void RefreshBombText(int currentBombCount, int maxBombCount)
    {
        BombText.text = $"{currentBombCount} / {maxBombCount}";
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

    public void UpdateReload(float progress)
    {
        if (ReloadSlider != null)
        {
            ReloadSlider.value = progress; // Slider 값 업데이트
        }
    }

    public void ShowReload(bool show)
    {
        if (ReloadSlider != null)
        {
            ReloadSlider.gameObject.SetActive(show); // Slider 활성화/비활성화
        }
    }

    public void UpdateReloadTextWithColor(float progress)
    {
        if (ReloadText != null)
        {
            // 진행 상태를 퍼센트로 변환
            int percentage = Mathf.RoundToInt(progress * 100);

            // 텍스트 업데이트
            ReloadText.text = $"재장전 중... {percentage}%";

            // 색상 업데이트 (흰색에서 빨간색으로)
            Color startColor = Color.white;
            Color endColor = Color.red;
            ReloadText.color = Color.Lerp(startColor, endColor, progress);
        }
    }
    public void ShowReloadText(bool show)
    {
        if (ReloadText != null)
        {
            ReloadText.gameObject.SetActive(show); // 텍스트 활성화/비활성화
            if (!show)
            {
                ReloadText.text = ""; // 비활성화 시 텍스트 초기화
            }
        }
    }

    private void ZoomIn()
    {
        float MinZoom = 5f; // 최소 Zoom 값
        float ZoomStep = 1f; // Zoom 조정 단위

        if (_minimapCamera != null)
        {
            _minimapCamera.orthographicSize = Mathf.Max(_minimapCamera.orthographicSize - ZoomStep, MinZoom);
        }
    }

    private void ZoomOut()
    {
        float MaxZoom = 15f; // 최대 Zoom 값
        float ZoomStep = 1f; // Zoom 조정 단위

        if (_minimapCamera != null)
        {
            _minimapCamera.orthographicSize = Mathf.Min(_minimapCamera.orthographicSize + ZoomStep, MaxZoom);
        }
    }
}
