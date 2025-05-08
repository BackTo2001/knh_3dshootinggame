using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState { Ready, Run, Pause, Over }

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    public UI_OptionPopup OptionPopup; // ���߿� ����

    public GameState CurrentState { get; private set; }

    [SerializeField] private Player player;

    private void Awake()
    {
        // ���� ������Ʈ�� ������ ���, ���� ������Ʈ�� ������ ������
        // static ������ ���� �־ ������ ����� ��찡 �ִ�.
        // �̷� ���, ���� ������Ʈ�� �������� �ʵ���
        // DontDestroyOnLoad�� ����Ͽ� ���� �ٲ� �������� �ʵ��� �Ѵ�.

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        SetGameState(GameState.Run);

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
            case GameState.Pause:
                Time.timeScale = 0f; // ���� ����
                break;

            case GameState.Run:
                Time.timeScale = 1f; // ���� �簳
                break;
        }
    }
    private bool IsGameOver()
    {
        // ���� ���� ���� ���� �˻�
        return player == null || player.GetComponent<PlayerStat>().CurrentHealth <= 0;
    }


    public void Pause()
    {
        if (CurrentState == GameState.Run)
        {
            SetGameState(GameState.Pause);
            Cursor.lockState = CursorLockMode.None;
            PopupManager.Instance.Open(PopupType.UI_OptionPopup, closeCallback: Continue);
        }
    }

    public void Continue()
    {
        if (CurrentState == GameState.Pause)
        {
            SetGameState(GameState.Run);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void Restart()
    {
        // ���� ����� ����
        // ���� ���, ���� �ٽ� �ε��ϰų� �ʱ�ȭ�ϴ� ����� ����� �� �ֽ��ϴ�.
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SetGameState(GameState.Run);

        Cursor.lockState = CursorLockMode.Locked;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        // Instance ��� �� ������ ��� ����
    }
}
