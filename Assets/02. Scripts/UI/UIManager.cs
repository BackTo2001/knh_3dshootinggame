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
        // PlayerMove���� ���¹̳� ���� �޾ƿ� UI ������Ʈ
        if (_playerMove != null && StaminaBar != null)
        {
            StaminaBar.value = _playerMove.Stamina / 100f; // 0~1�� ����ȭ
        }
    }
}
