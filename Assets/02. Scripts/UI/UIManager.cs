using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private PlayerStat _playerStat;
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private Camera _minimapCamera;

    [Header("UI Elements")]
    public Slider StaminaSlider;
    public Slider HealthSlider;
    public Slider BombThrowSlider;
    public Slider ReloadSlider;
    public TextMeshProUGUI BombText;
    public TextMeshProUGUI BulletText;
    public TextMeshProUGUI ReloadText;
    public Button ZoomInButton;
    public Button ZoomOutButton;

    private const float MinZoom = 5f;
    private const float MaxZoom = 15f;
    private const float ZoomStep = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (StaminaSlider != null) StaminaSlider.value = 1f;
        if (HealthSlider != null) HealthSlider.value = 1f;
        if (BombThrowSlider != null)
        {
            BombThrowSlider.value = 0f;
            BombThrowSlider.gameObject.SetActive(false);
        }
        if (ReloadSlider != null)
        {
            ReloadSlider.value = 0f;
            ReloadSlider.gameObject.SetActive(false);
        }

        if (ZoomInButton != null) ZoomInButton.onClick.AddListener(ZoomIn);
        if (ZoomOutButton != null) ZoomOutButton.onClick.AddListener(ZoomOut);
    }

    private void Update()
    {
        UpdateStaminaUI();
        UpdateHealthUI();
        HandleZoomInput();
    }

    private void UpdateStaminaUI()
    {
        if (_playerStat != null && StaminaSlider != null)
        {
            StaminaSlider.value = _playerStat.CurrentStamina / _playerData.MaxStamina;
        }
    }

    private void UpdateHealthUI()
    {
        if (_playerStat != null && HealthSlider != null)
        {
            HealthSlider.value = _playerStat.CurrentHealth / _playerData.MaxHealth;
        }
    }

    private void HandleZoomInput()
    {
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus))
        {
            ZoomIn();
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            ZoomOut();
        }
    }

    public void RefreshStaminaSlider(float currentStamina, float MaxStamina)
    {
        if (StaminaSlider != null)
            StaminaSlider.value = currentStamina / MaxStamina;
    }

    public void RefreshHealthSlider(float currentHealth, float maxHealth)
    {
        if (HealthSlider != null)
            HealthSlider.value = currentHealth / maxHealth;
    }

    public void RefreshBombText(int currentBombCount, int maxBombCount)
    {
        if (BombText != null)
            BombText.text = $"{currentBombCount} / {maxBombCount}";
    }

    public void ShowBombThrowSlider(bool show)
    {
        if (BombThrowSlider != null)
            BombThrowSlider.gameObject.SetActive(show);
    }

    public void UpdateBombThrowPower(float normalizedPower)
    {
        if (BombThrowSlider != null)
            BombThrowSlider.value = normalizedPower;
    }

    public void RefreshBulletText(int currentBulletCount, int maxBulletCount)
    {
        if (BulletText != null)
            BulletText.text = $"{currentBulletCount} / {maxBulletCount}";
    }

    public void UpdateReload(float progress)
    {
        if (ReloadSlider != null)
            ReloadSlider.value = progress;
    }

    public void ShowReload(bool show)
    {
        if (ReloadSlider != null)
            ReloadSlider.gameObject.SetActive(show);
    }

    public void UpdateReloadTextWithColor(float progress)
    {
        if (ReloadText != null)
        {
            int percentage = Mathf.RoundToInt(progress * 100);
            ReloadText.text = $"������ ��... {percentage}%";

            Color startColor = Color.white;
            Color endColor = Color.red;
            ReloadText.color = Color.Lerp(startColor, endColor, progress);
        }
    }

    public void ShowReloadText(bool show)
    {
        if (ReloadText != null)
        {
            ReloadText.gameObject.SetActive(show);
            if (!show)
                ReloadText.text = "";
        }
    }

    private void ZoomIn()
    {
        if (_minimapCamera != null)
        {
            _minimapCamera.orthographicSize = Mathf.Max(_minimapCamera.orthographicSize - ZoomStep, MinZoom);
        }
    }

    private void ZoomOut()
    {
        if (_minimapCamera != null)
        {
            _minimapCamera.orthographicSize = Mathf.Min(_minimapCamera.orthographicSize + ZoomStep, MaxZoom);
        }
    }

    // �ڷ�ƾ���� ������ Ready/Run/Over UI �Լ���
    public IEnumerator ShowReadyUI()
    {
        ToggleMainUI(false);
        Debug.Log("Ready UI is now active.");
        yield return null;
    }

    public IEnumerator ShowRunUI()
    {
        ToggleMainUI(true);
        Debug.Log("Run UI is now active.");
        yield return null;
    }

    public IEnumerator ShowOverUI()
    {
        ToggleMainUI(false);
        Debug.Log("Over UI is now active.");
        yield return null;
    }

    private void ToggleMainUI(bool isActive)
    {
        if (HealthSlider != null) HealthSlider.gameObject.SetActive(isActive);
        if (StaminaSlider != null) StaminaSlider.gameObject.SetActive(isActive);
        if (BombText != null) BombText.gameObject.SetActive(isActive);
        if (BulletText != null) BulletText.gameObject.SetActive(isActive);
    }
}
