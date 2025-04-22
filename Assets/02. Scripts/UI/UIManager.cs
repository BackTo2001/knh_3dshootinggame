using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Slider StaminaBar;
    private PlayerStat _playerStat;


    private void Start()
    {
        // PlayerStat 컴포넌트 찾기
        _playerStat = FindObjectOfType<PlayerStat>();

        if (StaminaBar != null)
        {
            StaminaBar.value = 100f;
        }
    }

    private void Update()
    {
        // PlayerMove에서 스태미나 값을 받아와 UI 업데이트
        if (_playerStat != null && StaminaBar != null)
        {
            StaminaBar.value = _playerStat.CurrentStamina / 100f; // 0~1로 정규화
        }
    }
}
