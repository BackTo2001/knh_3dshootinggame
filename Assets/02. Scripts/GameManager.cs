using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Ready, Run, Over }
    public GameState CurrentState { get; private set; }

    [SerializeField] private Player player; // Player 참조를 위한 필드

    private void Start()
    {
        StartCoroutine(GameFlow());
    }

    private IEnumerator GameFlow()
    {
        // Ready
        SetGameState(GameState.Ready);
        yield return UIManager.Instance.ShowReady();

        // Run
        CurrentState = GameState.Run;

        // Over
        while (!IsGameOver())
        {
            yield return null;
        }

        SetGameState(GameState.Over);
        UIManager.Instance.ShowGameOver();
    }
    private void SetGameState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Ready:
            case GameState.Over:
                Time.timeScale = 0f; // 게임 정지
                if (player != null)
                {
                    //player.EnableInput(false); // 플레이어 입력 비활성화
                }
                break;

            case GameState.Run:
                Time.timeScale = 1f; // 게임 재개
                if (player != null)
                {
                    //player.EnableInput(true); // 플레이어 입력 활성화
                }
                break;
        }
    }
    private bool IsGameOver()
    {
        // TODO: 실제 게임 오버 조건 검사
        return player == null || player.GetComponent<PlayerStat>().CurrentHealth <= 0;
    }
}
