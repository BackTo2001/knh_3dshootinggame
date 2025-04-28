using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Ready, Run, Over }
    public GameState CurrentState { get; private set; }

    [SerializeField] private UIManager _uiManager;   // UIManager 참조
    [SerializeField] private PlayerStat _playerStat; // PlayerStat 참조

    private Player _player; // 플레이어 캐싱

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        StartCoroutine(GameFlow());
    }

    private IEnumerator GameFlow()
    {
        // Ready 상태
        SetGameState(GameState.Ready);
        yield return StartCoroutine(_uiManager.ShowReadyUI());
        yield return new WaitForSeconds(2f);

        // Run 상태
        SetGameState(GameState.Run);
        yield return StartCoroutine(_uiManager.ShowRunUI());

        // 게임 진행
        while (!IsGameOver())
        {
            yield return null;
        }

        // Over 상태
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
                LockCursor(false); // 오버 상태에서는 마우스 풀어주기
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
