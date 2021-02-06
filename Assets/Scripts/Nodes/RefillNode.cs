using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillNode : Node
{
    public bool _hasRefilled = false;

    public override bool OnUse()
    {
        if (_hasRefilled)
            return false;

        // var turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
        var turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
        turnManager.RefillRolls();

        _hasRefilled = true;
        return true;
    }

}
