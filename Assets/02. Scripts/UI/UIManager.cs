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

    [SerializeField] private PlayerStat _playerStat; // PlayerStat ����
    [SerializeField] private PlayerDataSO _playerData; // PlayerDataSO ����
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
        // Cursor ó��
        Cursor.lockState = CursorLockMode.Locked;

        if (StaminaSlider != null)
        {
            StaminaSlider.value = 1f;
        }
        if (HealthSlider != null)
        {
            HealthSlider.value = 1f; // �ʱ�ȭ
        }
        if (BombThrowSlider != null)
        {
            BombThrowSlider.value = 0f; // �ʱ�ȭ
            BombThrowSlider.gameObject.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
        }
        if (ReloadSlider != null)
        {
            ReloadSlider.value = 0f; // �ʱ�ȭ
            ReloadSlider.gameObject.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
        }

        // Zoom ��ư Ŭ�� �̺�Ʈ ���
        ZoomInButton.onClick.AddListener(ZoomIn);
        ZoomOutButton.onClick.AddListener(ZoomOut);
    }
    private void Update()
    {
        // PlayerMove���� ���¹̳� ���� �޾ƿ� UI ������Ʈ
        if (_playerStat != null && StaminaSlider != null)
        {
            StaminaSlider.value = _playerStat.CurrentStamina / _playerData.MaxStamina; // 0~1�� ����ȭ
        }
        if (_playerStat != null && HealthSlider != null)
        {
            HealthSlider.value = _playerStat.CurrentHealth / _playerData.MaxHealth; // 0~1�� ����ȭ
        }

        // ZoomIn / ZoomOut Ű����
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus)) // + Ű
        {
            ZoomIn();
        }
        if (Input.GetKeyDown(KeyCode.Minus)) // - Ű
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
        HealthSlider.value = currentHealth / maxHealth; // 0~1�� ����ȭ
    }

    public void RefreshBombText(int currentBombCount, int maxBombCount)
    {
        BombText.text = $"{currentBombCount} / {maxBombCount}";
    }

    public void ShowBombThrowSlider(bool show)
    {
        if (BombThrowSlider != null)
        {
            BombThrowSlider.gameObject.SetActive(show); // Slider Ȱ��ȭ/��Ȱ��ȭ
        }
    }
    public void UpdateBombThrowPower(float normalizedPower)
    {
        if (BombThrowSlider != null)
        {
            BombThrowSlider.value = normalizedPower; // 0~1�� ����ȭ�� �� ������Ʈ
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
            ReloadSlider.value = progress; // Slider �� ������Ʈ
        }
    }

    public void ShowReload(bool show)
    {
        if (ReloadSlider != null)
        {
            ReloadSlider.gameObject.SetActive(show); // Slider Ȱ��ȭ/��Ȱ��ȭ
        }
    }

    public void UpdateReloadTextWithColor(float progress)
    {
        if (ReloadText != null)
        {
            // ���� ���¸� �ۼ�Ʈ�� ��ȯ
            int percentage = Mathf.RoundToInt(progress * 100);

            // �ؽ�Ʈ ������Ʈ
            ReloadText.text = $"������ ��... {percentage}%";

            // ���� ������Ʈ (������� ����������)
            Color startColor = Color.white;
            Color endColor = Color.red;
            ReloadText.color = Color.Lerp(startColor, endColor, progress);
        }
    }
    public void ShowReloadText(bool show)
    {
        if (ReloadText != null)
        {
            ReloadText.gameObject.SetActive(show); // �ؽ�Ʈ Ȱ��ȭ/��Ȱ��ȭ
            if (!show)
            {
                ReloadText.text = ""; // ��Ȱ��ȭ �� �ؽ�Ʈ �ʱ�ȭ
            }
        }
    }

    private void ZoomIn()
    {
        float MinZoom = 5f; // �ּ� Zoom ��
        float ZoomStep = 1f; // Zoom ���� ����

        if (_minimapCamera != null)
        {
            _minimapCamera.orthographicSize = Mathf.Max(_minimapCamera.orthographicSize - ZoomStep, MinZoom);
        }
    }

    private void ZoomOut()
    {
        float MaxZoom = 15f; // �ִ� Zoom ��
        float ZoomStep = 1f; // Zoom ���� ����

        if (_minimapCamera != null)
        {
            _minimapCamera.orthographicSize = Mathf.Min(_minimapCamera.orthographicSize + ZoomStep, MaxZoom);
        }
    }
}
