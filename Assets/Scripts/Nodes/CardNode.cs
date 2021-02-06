using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardNode : Node
{
    public Card cardGranted;
    public bool _hasGrantedCard = false;

    public override bool OnUse()
    {
        if (_hasGrantedCard)
            return false;

        // var player = GameObject.Find("Player").GetComponent<Player>();
        // var obj = Instantiate(cardGranted, Vector3.zero, Quaternion.identity, player.transform);
        // obj.gameObject.SetActive(false);
        // player.AddCard(obj);

        var turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
        turnManager.GrantNewCard(cardGranted);

        _hasGrantedCard = true;
        return true;
    }
}
