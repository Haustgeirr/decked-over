using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        Moving,
        Battle,
        Pause,
        Menu,
        GameOver,
        EndGame,
    }

    [SerializeField]
    private UIController uiController;
    private PlayerMovement playerMovement;

    public bool IsFinalBattle = false;

    public GameState currentGameState;
    public GameState previousGameState;

    public void RestartGame()
    {
        TransitionToState(GameState.Menu);
    }

    public void StartGame()
    {
        TransitionToState(GameState.Moving);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator ReturnToMenu(IEnumerator transition)
    {
        yield return StartCoroutine(transition);
        TransitionToState(GameState.Menu);
    }

    public void TransitionToState(GameState newState)
    {
        // GameState tmpCurrentState = currentGameState;
        previousGameState = currentGameState;
        OnStateExit(previousGameState, newState);
        currentGameState = newState;
        OnStateEnter(newState, previousGameState);
    }

    private void OnStateEnter(GameState state, GameState fromState)
    {
        switch (state)
        {
            case GameState.Battle:
                {
                    // uiController.ShowBattleUI();
                    break;
                }
            case GameState.Moving:
                {
                    uiController.ShowMovingUI();
                    playerMovement.CheckRemainingRolls();
                    break;
                }
            case GameState.Pause:
                {
                    uiController.ShowPauseMenu();
                    break;
                }
            case GameState.Menu:
                {
                    SceneManager.LoadScene("Main", LoadSceneMode.Single);
                    break;
                }
            case GameState.GameOver:
                {
                    var turnManager = FindObjectOfType<TurnManager>();
                    StartCoroutine(ReturnToMenu(uiController.AnimateBattleEnd("OutOfRolls", false)));
                    break;
                }
            case GameState.EndGame:
                {
                    uiController.ShowEndGameScreen();
                    break;
                }

        }
    }

    private void OnStateExit(GameState state, GameState toState)
    {
        switch (state)
        {
            case GameState.Battle:
                { break; }
            case GameState.Moving:
                { break; }
            case GameState.Pause:
                {
                    if (state == GameState.Battle)
                    { uiController.ShowBattleUI(); }

                    if (state == GameState.Moving)
                    { uiController.ShowMovingUI(); }

                    uiController.HidePauseMenu();

                    break;
                }
            case GameState.Menu:
                { break; }
            case GameState.GameOver:
                { break; }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        uiController = GameObject.Find("UI").GetComponent<UIController>();
        playerMovement = GameObject.Find("Map").GetComponent<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGameState = GameState.Menu;
        uiController.ShowMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (currentGameState == GameState.Menu || currentGameState == GameState.GameOver)
                return;

            if (currentGameState != GameState.Pause)
            {
                TransitionToState(GameState.Pause);
            }
            else
            {
                TransitionToState(previousGameState);
            }
        }
    }
}
