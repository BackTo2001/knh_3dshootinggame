using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState { Ready, Run, Pause, Over }

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    public UI_OptionPopup OptionPopup; // 나중에 삭제

    public GameState CurrentState { get; private set; }

    [SerializeField] private Player player;

    private void Awake()
    {
        // 게임 오브젝트가 삭제될 경우, 게임 오브젝트의 참조는 잃지만
        // static 변수가 남아 있어서 오류가 생기는 경우가 있다.
        // 이럴 경우, 게임 오브젝트가 삭제되지 않도록
        // DontDestroyOnLoad를 사용하여 씬이 바뀌어도 삭제되지 않도록 한다.

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
                Time.timeScale = 0f; // 게임 정지
                break;

            case GameState.Run:
                Time.timeScale = 1f; // 게임 재개
                break;
        }
    }
    private bool IsGameOver()
    {
        // 실제 게임 오버 조건 검사
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
        // 게임 재시작 로직
        // 예를 들어, 씬을 다시 로드하거나 초기화하는 방법을 사용할 수 있습니다.
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SetGameState(GameState.Run);

        Cursor.lockState = CursorLockMode.Locked;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        // Instance 사용 시 망가질 우려 있음
    }
}
