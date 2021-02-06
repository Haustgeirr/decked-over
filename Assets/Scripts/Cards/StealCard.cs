using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealCard : Card
{
    public override void RollOne()
    {
        user.Steal(1);
    }

    public override void RollTwo()
    {
        user.Steal(2);
    }

    public override void RollThree()
    {
        user.Steal(3);
    }

    public override void RollFour()
    {
        user.Steal(4);
    }

    public override void RollFive()
    {
        user.Steal(5);
    }

    public override void RollSix()
    {
        user.Steal(6);
    }
}
