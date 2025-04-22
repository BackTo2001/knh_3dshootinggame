using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Slider StaminaBar;
    private PlayerStat _playerStat;


    private void Start()
    {
        // PlayerStat ������Ʈ ã��
        _playerStat = FindObjectOfType<PlayerStat>();

        if (StaminaBar != null)
        {
            StaminaBar.value = 100f;
        }
    }

    private void Update()
    {
        // PlayerMove���� ���¹̳� ���� �޾ƿ� UI ������Ʈ
        if (_playerStat != null && StaminaBar != null)
        {
            StaminaBar.value = _playerStat.CurrentStamina / 100f; // 0~1�� ����ȭ
        }
    }
}
