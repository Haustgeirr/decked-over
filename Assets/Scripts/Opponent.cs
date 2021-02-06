using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : CardUser
{
    private int _nextCardToChoose;

    public override void StartTurn()
    {
        base.StartTurn();

        ChooseCardInOrder();
        chosenDiceRoll = diceroll.RollRandom();

        var pos = chosenCardPos.position;
        var rot = Quaternion.Euler(0f, 0f, Random.Range(-5f, 5f));

        chosenCard.transform.localScale = Vector3.one;

        StartCoroutine(Math3D.CardAnim(chosenCard.gameObject, new Vector3(pos.x, pos.y, -1f), rot, 0.25f));
    }

    public void ChooseCardInOrder()
    {
        chosenCardIndex = _nextCardToChoose;
        chosenCard = deck[chosenCardIndex];

        _nextCardToChoose = (chosenCardIndex + 1 < deck.Count) ? chosenCardIndex + 1 : 0;
    }

    public override void TakeTurn()
    {
        base.TakeTurn();

        chosenCard.RollDie(chosenDiceRoll);

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

    public override void StartBattle()
    {
        base.StartBattle();
        healthUI.gameObject.SetActive(true);
    }

    public override void EndBattle()
    {
        base.EndBattle();
        healthUI.gameObject.SetActive(false);
    }

    public override void InitCardUser()
    {
        base.InitCardUser();

        chosenCardIndex = 0;
        _nextCardToChoose = 0;

        // RefreshDeck();
    }
}
