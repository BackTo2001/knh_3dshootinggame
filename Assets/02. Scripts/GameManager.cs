using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    public UI_OptionPopup OptionPopup;

    private void Awake()
    {
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

    public enum GameState { Ready, Run, Pause, Over }
    public GameState CurrentState { get; private set; }

    [SerializeField] private Player player;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (CurrentState == GameState.Run)
        {
            SetGameState(GameState.Pause);
            OptionPopup.Open();
        }
        else if (CurrentState == GameState.Pause)
        {
            SetGameState(GameState.Run);
            OptionPopup.Close();
        }
    }
}
