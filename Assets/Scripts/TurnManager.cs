using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public Player player;
    public CardUser opponent;
    public UIController uiController;

    public bool isInBattle;

    public bool playerGoesFirst = true;
    public int turnNumber;

    private CameraController _cam;
    private Card grantedCard = null;

    public void DealDamage(int damage, CardUser user)
    {
        // get target and then apply
        if (user == player)
        {
            opponent.TakeDamage(damage);
        }
        else
        {
            player.TakeDamage(damage);
        }
    }

    public void TriggerFinalBattle(CardUserData opponentData)
    {
        opponent.data = opponentData;
        opponent.InitCardUser();

        GameManager.Instance.IsFinalBattle = true;
        StartBattle();
    }

    public void TriggerBattle(CardUserData opponentData)
    {
        opponent.data = opponentData;
        opponent.InitCardUser();

        StartBattle();
    }

    public IEnumerator RefillRollCoroutine(int amount)
    {
        var playerMovement = GameObject.Find("Map").GetComponent<PlayerMovement>();

        for (int i = 1; i <= amount; i++)
        {
            player.RefillRolls(i);
            player.diceRollUI.UpdateUI();
            playerMovement.PlayRefillRollSound(i);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void RefillRolls()
    {
        StartCoroutine(RefillRollCoroutine(6));
        // player.RefillRolls(6);
    }

    public void AcceptCard()
    {
        uiController.HideNewCardUI();
        grantedCard.gameObject.SetActive(false);
        player.AddCard(grantedCard);
        grantedCard = null;

        GameManager.Instance.TransitionToState(GameManager.GameState.Moving);
    }

    public void GrantNewCard(Card card)
    {
        uiController.ShowNewCardUI();
        var camPos = new Vector3(_cam.transform.position.x, _cam.transform.position.y, 0f);

        player.transform.position = camPos;
        opponent.transform.position = camPos;

        Quaternion newCardStartRot = Quaternion.Euler(0f, 0f, Random.Range(-15f, 15f));
        Quaternion newCardEndRot = Quaternion.Euler(0f, 0f, Random.Range(-5f, 5f));
        Vector3 newCardStartPos = new Vector3(Random.Range(-8f, 8f), Random.Range(-5f, 5f), 0f) + camPos;

        var obj = Instantiate(card, newCardStartPos, newCardStartRot, player.transform);
        grantedCard = obj;

        StartCoroutine(Math3D.CardAnim(obj.gameObject, camPos, newCardEndRot, 0.15f));
    }

    public void StartBattle()
    {
        GameManager.Instance.TransitionToState(GameManager.GameState.Battle);
        StartCoroutine(StartBattleAnim());
    }

    private IEnumerator StartBattleAnim()
    {
        isInBattle = true;

        var camPos = new Vector3(_cam.transform.position.x, _cam.transform.position.y, 0f);

        player.transform.position = camPos;
        opponent.transform.position = camPos;

        yield return StartCoroutine(uiController.AnimateBattleStart(opponent.data.userName));

        player.StartBattle();
        opponent.StartBattle();

        turnNumber = 0;
        NextTurn();

        yield return null;
    }

    public void EndBattle(CardUser loser, bool outOfRolls)
    {
        StartCoroutine(EndBattleCoroutine(loser, outOfRolls));
    }

    private IEnumerator EndBattleCoroutine(CardUser loser, bool outOfRolls)
    {
        isInBattle = false;

        player.EndBattle();
        opponent.EndBattle();

        if (loser == player)
        {
            if (outOfRolls)
                yield return StartCoroutine(uiController.AnimateBattleEnd("OutOfRolls", false));
            else
                yield return StartCoroutine(uiController.AnimateBattleEnd(opponent.data.userName, false));

            GameManager.Instance.TransitionToState(GameManager.GameState.Menu);
        }
        else
        {
            yield return StartCoroutine(uiController.AnimateBattleEnd(opponent.data.userName, true));

            if (GameManager.Instance.IsFinalBattle)
            {
                GameManager.Instance.TransitionToState(GameManager.GameState.EndGame);
            }
            else
            {
                if (opponent.data.cardReward != null)
                {
                    GrantNewCard(opponent.data.cardReward);
                }
                else
                {
                    GameManager.Instance.TransitionToState(GameManager.GameState.Moving);
                }
            }
        }

        turnNumber = -1;

        yield return null;
    }

    public void NextTurn()
    {
        turnNumber++;

        opponent.StartTurn();
        player.StartTurn();
    }

    public void TakeTurn()
    {
        if (opponent == null)
        {
            StartBattle();
        }

        if (player.GetPriority() <= opponent.GetPriority())
        {
            player.TakeTurn();

            if (isInBattle)
                opponent.TakeTurn();
        }
        else
        {
            opponent.TakeTurn();

            if (isInBattle)
            {

                player.TakeTurn();
            }
        }

        // if (isInBattle && player.diceroll.rollsRemaining.Count > 0)
        if (isInBattle)
            NextTurn();
        // else
        // EndBattle(player, true);
    }

    private void Init()
    {
        player.InitCardUser();
        // opponent.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        // StartBattle();
    }

    // Update is called once per frame
    void Update()
    {
        uiController.SetGoButtonActive(player.chosenDiceRoll != 0);
        uiController.SetMoveButtonActive(player.chosenDiceRoll != 0);


        if (turnNumber > 0)
        {
            // turnNumberText.text = turnNumber.ToString();
        }
    }

    private void Awake()
    {
        _cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }
}
