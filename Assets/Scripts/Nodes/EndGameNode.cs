using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameNode : Node
{
    public CardUserData opponentData;
    public bool _hasBattled = false;

    public override bool OnUse()
    {
        if (_hasBattled)
            return false;

        // var turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
        var map = GameObject.Find("Map").GetComponent<PlayerMovement>();
        map.TriggerFinalBattle(opponentData);

        _hasBattled = true;
        return true;
    }
}
