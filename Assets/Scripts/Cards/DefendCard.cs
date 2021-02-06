using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendCard : Card
{
    public override void RollOne()
    {
        user.AddArmour(1);
    }

    public override void RollTwo()
    {
        user.AddArmour(1);
    }

    public override void RollThree()
    {
        user.AddArmour(2);
    }

    public override void RollFour()
    {
        user.AddArmour(2);
    }

    public override void RollFive()
    {
        user.AddArmour(3);
    }

    public override void RollSix()
    {
        user.AddArmour(3);
    }
}
