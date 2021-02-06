using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static readonly string[] BATTLE_MESSAGES = {
        " taunts you!",
        " shows-off their self-actualisation!",
        " twiddles their fingers menacingly!",
        " demonstrates their ability to choose!",
        " will regret this.",
        " is minding their own business...",
        " attempts to flee. Key word 'attempts'.",
        " tries to inspect your magic dice. You offer a demonstration.",
        " forgets that not all Wizards are wise.",
        " tries to grab your deck.",
        " looks pretty harmless.",
        " is confused. In general."
        };

    public static readonly string[] VICTORY_MESSAGES = {
        " demonstrates how 'free-will' can only get you so far.",
        " is turned inside-out.",
        " needs medical attention.",
        " will probably claim for this.",
        " is probably, no, definitely dead.",
        " has had marginally better days.",
        " briefly knew Kung-Fu before passing out.",
        " got dirt on their clothes. They're going home.",
        " is killed. You monster.",
        " regrets nothing!",
        " tried really, really hard.",
        };

    public static readonly string[] DEFEAT_MESSAGES = {
        "In a shocking turn of event, you were killed.",
        "Unbeknownst to your opponent, you're rubbish at card games.",
        "Pff, game is rubbish anyway.",
        "Winning is for people who try harder.",
        "Success is a poor teacher. But she is less painful.",
        "...",
        "Oh, I'm not angry. I'm dissapointed.",
        };

    [Header("Panels")]
    public GameObject playerHPPanel;
    public GameObject opponentHPPanel;
    public GameObject diceRollUI;
    public GameObject backgroundMask;
    public GameObject battleMessagePanel;
    public GameObject newCardPanel;
    public GameObject menuPanel;
    public GameObject pausePanel;
    public GameObject endGamePanel;

    [Header("Buttons")]
    public GameObject moveButton;
    public GameObject takeTurnButton;
    public GameObject passButton;
    public GameObject okButton;

    [Header("Text")]
    // public TextMeshProUGUI turnIndicatorText;
    // public TextMeshProUGUI turnNumberText;
    public TextMeshProUGUI battleMessageText;
    public TextMeshProUGUI battleStatusText;

    [Header("Audio")]
    public AudioSource source;

    public AudioClip selectDie;
    public AudioClip buttonPress;
    public AudioClip startBattleSound;
    public AudioClip endBattleSound;
    public AudioClip drawCardSound;

    private CameraController _cam;

    public void SetGoButtonActive(bool value)
    {
        takeTurnButton.GetComponent<Button>().interactable = value;
    }

    public void SetMoveButtonActive(bool value)
    {
        moveButton.GetComponent<Button>().interactable = value;
    }

    private void ShowBackgroundMask()
    {
        var backgroundMaskPos = new Vector3(_cam.transform.position.x, _cam.transform.position.y, backgroundMask.transform.position.z);
        backgroundMask.transform.position = backgroundMaskPos;
        backgroundMask.SetActive(true);
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        pausePanel.SetActive(true);
    }
    public void HidePauseMenu()
    {
        pausePanel.SetActive(false);
    }

    public void ShowEndGameScreen()
    {
        pausePanel.SetActive(false);
        menuPanel.SetActive(false);
        moveButton.SetActive(true);
        takeTurnButton.SetActive(false);
        okButton.SetActive(false);

        diceRollUI.SetActive(false);
        playerHPPanel.SetActive(false);
        opponentHPPanel.SetActive(false);

        backgroundMask.SetActive(false);
        newCardPanel.SetActive(false);
        battleMessagePanel.SetActive(false);
        endGamePanel.SetActive(true);
    }

    public void ShowMovingUI()
    {
        pausePanel.SetActive(false);
        menuPanel.SetActive(false);
        moveButton.SetActive(true);
        takeTurnButton.SetActive(false);
        okButton.SetActive(false);

        diceRollUI.SetActive(true);

        playerHPPanel.SetActive(false);
        opponentHPPanel.SetActive(false);

        backgroundMask.SetActive(false);
        newCardPanel.SetActive(false);
        battleMessagePanel.SetActive(false);
    }

    public void ShowBattleUI()
    {
        pausePanel.SetActive(false);
        moveButton.SetActive(false);
        ShowBackgroundMask();
        playerHPPanel.SetActive(true);
        opponentHPPanel.SetActive(true);
        takeTurnButton.SetActive(true);
    }

    public void ShowNewCardUI()
    {
        ShowBackgroundMask();

        newCardPanel.SetActive(true);

        diceRollUI.SetActive(false);
        moveButton.SetActive(false);
        takeTurnButton.SetActive(false);


        okButton.SetActive(true);
    }

    public void HideNewCardUI()
    {
        okButton.SetActive(false);
        diceRollUI.SetActive(true);
        moveButton.SetActive(true);
        takeTurnButton.SetActive(false);

        newCardPanel.SetActive(false);
        backgroundMask.SetActive(false);
    }

    public void PlayDieSelectSound()
    {
        source.clip = selectDie;
        source.pitch = Random.Range(1f, 1.1f);
        source.Play();
    }

    public void PlayButtonPressSound()
    {
        source.clip = buttonPress;
        source.pitch = Random.Range(1f, 1.1f);
        source.Play();
    }

    public IEnumerator AnimateBattleStart(string opponent)
    {
        moveButton.SetActive(false);

        battleStatusText.text = "Battle start!";
        battleMessageText.text = opponent + BATTLE_MESSAGES[Random.Range(0, BATTLE_MESSAGES.Length)];

        ShowBackgroundMask();

        battleMessagePanel.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        battleMessagePanel.SetActive(false);

        playerHPPanel.SetActive(true);
        opponentHPPanel.SetActive(true);

        takeTurnButton.SetActive(true);
    }

    public IEnumerator AnimateBattleEnd(string opponent, bool playerWins)
    {
        playerHPPanel.SetActive(false);
        opponentHPPanel.SetActive(false);
        takeTurnButton.SetActive(false);
        moveButton.SetActive(false);
        okButton.SetActive(false);

        if (playerWins)
        {
            battleStatusText.text = "Victory!";
            battleMessageText.text = opponent + VICTORY_MESSAGES[Random.Range(0, VICTORY_MESSAGES.Length)];
        }
        else
        {
            diceRollUI.SetActive(false);
            if (opponent == "OutOfRolls")
            {
                battleStatusText.text = "Game Over!";
                battleMessageText.text = "You ran out of dice rolls.";
            }
            else
            {
                battleStatusText.text = "Defeat!";
                battleMessageText.text = DEFEAT_MESSAGES[Random.Range(0, DEFEAT_MESSAGES.Length)];
            }
        }

        ShowBackgroundMask();

        battleMessagePanel.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        battleMessagePanel.SetActive(false);

        moveButton.SetActive(true);
        backgroundMask.SetActive(false);
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        _cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }
}
