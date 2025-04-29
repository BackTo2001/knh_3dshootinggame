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
                Time.timeScale = 0f; // ���� ����
                if (player != null)
                {
                    //player.EnableInput(false); // �÷��̾� �Է� ��Ȱ��ȭ
                }
                break;

            case GameState.Run:
                Time.timeScale = 1f; // ���� �簳
                if (player != null)
                {
                    //player.EnableInput(true); // �÷��̾� �Է� Ȱ��ȭ
                }
                break;
        }
    }
    private bool IsGameOver()
    {
        // TODO: ���� ���� ���� ���� �˻�
        return player == null || player.GetComponent<PlayerStat>().CurrentHealth <= 0;
    }
}
