using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCard : Card
{
    public override void RollOne()
    {
        user.Heal(1);
    }

    public override void RollTwo()
    {
        user.Heal(2);
    }

    public override void RollThree()
    {
        user.Heal(3);
    }

    public override void RollFour()
    {
        user.Heal(4);
    }

    public override void RollFive()
    {
        user.Heal(5);
    }

    public override void RollSix()
    {
        user.Heal(6);
    }
}
