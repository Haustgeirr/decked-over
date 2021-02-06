using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoilCard : Card
{
    public override void RollOne()
    {
        user.Spoil(1);
    }

    public override void RollTwo()
    {
        user.Spoil(2);
    }

    public override void RollThree()
    {
        user.Spoil(3);
    }

    public override void RollFour()
    {
        user.Spoil(4);
    }

    public override void RollFive()
    {
        user.Spoil(5);
    }

    public override void RollSix()
    {
        user.Spoil(6);
    }
}
