using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Slider StaminaBar;
    private PlayerMove _playerMove;

    private void Start()
    {
        _playerMove = FindObjectOfType<PlayerMove>();

        if (StaminaBar != null)
        {
            StaminaBar.value = 100f;
        }
    }

    private void Update()
    {
        // PlayerMove에서 스태미나 값을 받아와 UI 업데이트
        if (_playerMove != null && StaminaBar != null)
        {
            StaminaBar.value = _playerMove.Stamina / 100f; // 0~1로 정규화
        }
    }
}
