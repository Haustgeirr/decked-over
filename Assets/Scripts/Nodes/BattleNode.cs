using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleNode : Node
{
    // public CardUserData opponentData;
    public bool _hasBattled = false;

    // public GameObject battleIcon;

    // public override void DisableNode()
    // {
    //     battleIcon.SetActive(false);
    //     nodeIcon.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    // }

    public override bool OnUse()
    {
        if (_hasBattled)
            return false;

        // var turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
        var map = GameObject.Find("Map").GetComponent<PlayerMovement>();
        map.TriggerBattle();

        _hasBattled = true;
        return true;
    }

}
