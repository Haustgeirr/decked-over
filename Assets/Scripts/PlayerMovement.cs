using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Node currentNode;
    public int stepsToTake;

    public AudioSource source;
    public AudioClip moveClip;
    public AudioClip refillClip;

    [Header("Enemy Types")]
    public CardUserData nextOpponent;
    public CardUserData twoCardEnemy;
    public CardUserData threeCardEnemy;
    public CardUserData fourCardEnemy;
    public CardUserData fiveCardEnemy;
    public CardUserData sixCardEnemy;

    [Header("Node Sprites")]
    public Sprite ActiveNodeSprite;
    public Sprite VisitedNodeSprite;

    private Player _player;
    private TurnManager _turnManager;
    private CameraController _cam;
    // private bool _canSelectTarget = false;

    private int _movePitch = 0;

    public void PlayRefillRollSound(int roll)
    {
        source.clip = refillClip;
        source.pitch = 1f + roll * 0.2f;
        source.Play();
    }

    private void PlayMoveSound()
    {
        source.clip = moveClip;
        source.pitch = 1f + _movePitch * 0.2f;
        source.Play();
    }


    public void TriggerFinalBattle(CardUserData opponent)
    {
        SetNextOpponent();
        _turnManager.TriggerFinalBattle(opponent);
    }

    public void TriggerBattle()
    {
        SetNextOpponent();
        _turnManager.TriggerBattle(nextOpponent);
    }

    public void Move()
    {
        stepsToTake = _player.chosenDiceRoll;
        _player.diceroll.ConsumeRoll(stepsToTake);
        _player.diceRollUI.UpdateUI();
        _player.chosenDiceRoll = 0;

        StartCoroutine(TraverseNodePath());
    }

    private IEnumerator TraverseNodePath()
    {
        // _canSelectTarget = false;
        _movePitch = 0;

        for (int i = stepsToTake - 1; i >= 0; i--)
        {
            var nextNode = 0;

            if (currentNode.children.Length == 1)
            {
                nextNode = 0;
            }
            else if (currentNode.children.Length > 1)
            {
                nextNode = Random.Range(0, currentNode.children.Length);
            }
            else
            {
                break;
            }

            currentNode.DisableNode();

            if (currentNode.children[nextNode] != null)
                currentNode.children[nextNode].SetHighlight(true);

            PlayMoveSound();
            _movePitch++;

            yield return StartCoroutine(_cam.MoveToNode(currentNode.children[nextNode]));
            currentNode = currentNode.children[nextNode];
        }

        var hasUsed = currentNode.OnUse();

        if (!hasUsed)
            CheckRemainingRolls();

        // _canSelectTarget = true;
        yield return null;
    }

    private void SetNextOpponent()
    {
        switch (_player.deck.Count)
        {
            case 1:
                {
                    nextOpponent = twoCardEnemy;
                    break;
                }
            case 2:
                {
                    nextOpponent = twoCardEnemy;
                    break;
                }
            case 3:
                {
                    nextOpponent = threeCardEnemy;
                    break;
                }
            case 4:
                {
                    nextOpponent = fourCardEnemy;
                    break;
                }
            case 5:
                {
                    nextOpponent = fiveCardEnemy;
                    break;
                }
            case 6:
                {
                    nextOpponent = sixCardEnemy;
                    break;
                }
        }
    }

    public void CheckRemainingRolls()
    {
        if (_player.diceroll.rollsRemaining.Count <= 0)
        {
            // TriggerBattle();
            GameManager.Instance.TransitionToState(GameManager.GameState.GameOver);
        }
    }

    private void Awake()
    {
        _turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
        source = GetComponent<AudioSource>();
    }
}
