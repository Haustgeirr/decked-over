using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillCard : Card
{
    public override void RollOne()
    {
        user.diceroll.RefillRolls(1);
    }

    public override void RollTwo()
    {
        user.diceroll.RefillRolls(2);
    }

    public override void RollThree()
    {
        user.diceroll.RefillRolls(3);
    }

    public override void RollFour()
    {
        user.diceroll.RefillRolls(4);
    }

    public override void RollFive()
    {
        user.diceroll.RefillRolls(5);
    }

    public override void RollSix()
    {
        user.diceroll.RefillRolls(6);
    }
}
