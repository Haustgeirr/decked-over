using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertCard : Card
{
    public override void RollOne()
    {
        user.DealDamage(1);
    }

    public override void RollTwo()
    {
        user.DealDamage(2);
    }

    public override void RollThree()
    {
        user.DealDamage(3);
    }

    public override void RollFour()
    {
        user.DealDamage(4);
    }

    public override void RollFive()
    {
        user.DealDamage(5);
    }

    public override void RollSix()
    {
        user.DealDamage(6);
    }
}
