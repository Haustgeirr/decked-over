using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CardUser
{
    public DiceRollUI diceRollUI;

    public void RefillRolls(int amount)
    {
        diceroll.RefillRolls(amount);
    }

    public void AddCard(Card card)
    {
        if (deck.Count < 6)
        {
            deck.Add(card);
        }
        else
        {
            deck.RemoveAt(Random.Range(0, 6));
            deck.Add(card);
        }

        RefreshDeck();
    }

    private IEnumerator StartTurnCoroutine()
    {
        ChooseCardAtRandom();

        var pos = chosenCardPos.position;
        var rot = Quaternion.Euler(0f, 0f, Random.Range(-5f, 5f));

        chosenCard.transform.localScale = Vector3.one;

        // this nested coroutine is to allow the anims to play before ending game
        yield return StartCoroutine(Math3D.CardAnim(chosenCard.gameObject, new Vector3(pos.x, pos.y, -1f), rot, 0.25f));
        yield return new WaitForSeconds(0.5f);

        // check if we have rolls left and end battle if not
        if (diceroll.rollsRemaining.Count <= 0)
            _turnManager.EndBattle(this, true);

    }

    public override void StartTurn()
    {
        base.StartTurn();

        StartCoroutine(StartTurnCoroutine());

        // ChooseCardAtRandom();

        // var pos = chosenCardPos.position;
        // var rot = Quaternion.Euler(0f, 0f, Random.Range(-5f, 5f));

        // chosenCard.transform.localScale = Vector3.one;
        // StartCoroutine(Math3D.CardAnim(chosenCard.gameObject, new Vector3(pos.x, pos.y, -1f), rot, 0.25f));
    }

    public override void TakeTurn()
    {
        base.TakeTurn();

        chosenCard.RollDie(chosenDiceRoll);
        chosenDiceRoll = 0;

        diceRollUI.UpdateUI();
        ReturnCardToDeck();
    }

    public override void ReturnCardToDeck()
    {
        var pos = deckCardPos[chosenCardIndex].position;
        var rot = deckCardPos[chosenCardIndex].rotation;

        chosenCard.transform.localScale = deckScale;
        chosenCard.transform.position = new Vector3(pos.x, pos.y, -1f);
        chosenCard.transform.rotation = rot;
    }

    public void ChooseRoll(int roll)
    {
        if (chosenDiceRoll != 0)
            diceroll.AddRoll(chosenDiceRoll);

        if (roll == chosenDiceRoll)
        {
            chosenDiceRoll = 0;
        }
        else
        {
            diceroll.ConsumeRoll(roll);
            chosenDiceRoll = roll;
        }

        // Debug.Log("Player " + chosenDiceRoll);
    }

    public override void StartBattle()
    {
        // diceroll.RefillRolls(6);
        diceRollUI.UpdateUI();

        base.StartBattle();
    }

    public override void InitCardUser()
    {
        base.InitCardUser();
        // RefreshDeck();
    }
}
