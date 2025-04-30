using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Ready, Run, Over }
    public GameState CurrentState { get; private set; }

    [SerializeField] private Player player; // Player ������ ���� �ʵ�

    private void Start()
    {
        StartCoroutine(GameFlow());
    }

    private IEnumerator GameFlow()
    {
        // Ready
        SetGameState(GameState.Ready);
        Debug.Log("GameState: Ready");
        yield return UIManager.Instance.ShowReady();

        // Run
        CurrentState = GameState.Run;
        SetGameState(GameState.Run);
        Debug.Log("GameState: Run");

        // Over
        while (!IsGameOver())
        {
            yield return null;
        }

        SetGameState(GameState.Over);
        Debug.Log("GameState: Over");
        UIManager.Instance.ShowGameOver();
    }
    private void SetGameState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Ready:
            case GameState.Over:
                Debug.Log("(Time.timeScale = 0)");
                Time.timeScale = 0f; // ���� ����
                break;

            case GameState.Run:
                Debug.Log("���� ���� ���� (Time.timeScale = 1)");
                Time.timeScale = 1f; // ���� �簳
                break;
        }
        Debug.Log($"[SetGameState] Time.timeScale: {Time.timeScale}");
    }
    private bool IsGameOver()
    {
        // TODO: ���� ���� ���� ���� �˻�
        return player == null || player.GetComponent<PlayerStat>().CurrentHealth <= 0;
    }
}
