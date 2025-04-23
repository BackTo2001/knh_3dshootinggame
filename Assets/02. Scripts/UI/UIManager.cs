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

    public Slider StaminaSlider;
    public Slider BombThrowSlider;
    public Slider ReloadSlider;
    public TextMeshProUGUI BombText;
    public TextMeshProUGUI BulletText;
    public TextMeshProUGUI ReloadText;


    private void Start()
    {
        if (StaminaSlider != null)
        {
            StaminaSlider.value = 1f;
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
    }
    private void Update()
    {
        // PlayerMove���� ���¹̳� ���� �޾ƿ� UI ������Ʈ
        if (_playerStat != null && StaminaSlider != null)
        {
            StaminaSlider.value = _playerStat.CurrentStamina / _playerData.MaxStamina; // 0~1�� ����ȭ
        }
    }

    public void RefreshStaminaSlider(float currentStamina, float MaxStamina)
    {
        StaminaSlider.value = currentStamina / MaxStamina;
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
}
