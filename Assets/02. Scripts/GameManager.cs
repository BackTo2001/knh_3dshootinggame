using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Ready, Run, Over }
    public GameState CurrentState { get; private set; }

    [SerializeField] private UIManager _uiManager;   // UIManager ����
    [SerializeField] private PlayerStat _playerStat; // PlayerStat ����

    private Player _player; // �÷��̾� ĳ��

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        StartCoroutine(GameFlow());
    }

    private IEnumerator GameFlow()
    {
        // Ready ����
        SetGameState(GameState.Ready);
        yield return StartCoroutine(_uiManager.ShowReadyUI());
        yield return new WaitForSeconds(2f);

        // Run ����
        SetGameState(GameState.Run);
        yield return StartCoroutine(_uiManager.ShowRunUI());

        // ���� ����
        while (!IsGameOver())
        {
            yield return null;
        }

        // Over ����
        SetGameState(GameState.Over);
        yield return StartCoroutine(_uiManager.ShowOverUI());
    }

    private void SetGameState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Ready:
                LockCursor(true);
                break;

            case GameState.Run:
                LockCursor(true);
                break;

            case GameState.Over:
                LockCursor(false); // ���� ���¿����� ���콺 Ǯ���ֱ�
                break;
        }
    }

    private void LockCursor(bool isLock)
    {
        Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLock;
    }

    private bool IsGameOver()
    {
        return _player == null || _playerStat.IsDead;
    }
}
