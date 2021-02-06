using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Diceroll
{
    public List<int> rollsRemaining;

    public void Init()
    {
        rollsRemaining = new List<int>();

        // for (int i = 1; i <= 6; i++)
        // {
        //     rollsRemaining.Add(i);
        // }

        RefillRolls(6);
    }

    public void RefillRolls(int rolls)
    {
        for (int i = 1; i <= rolls; i++)
        {
            if (!rollsRemaining.Contains(i))
                rollsRemaining.Add(i);
        }
    }

    public bool AddRoll(int roll)
    {
        if (!rollsRemaining.Contains(roll))
        {
            rollsRemaining.Add(roll);
            return true;
        }

        return false;
    }

    public int RollRandom()
    {
        if (rollsRemaining.Count == 0)
            return 0;

        var index = Random.Range(0, rollsRemaining.Count);
        var r = rollsRemaining[index];

        ConsumeRoll(r);

        return r;
    }

    public void ConsumeRoll(int roll)
    {
        rollsRemaining.Remove(roll);
    }

    // public void ConsumeRoll(int roll)
    // {
    //     var index = rollsRemaining.Find(x => x == roll);
    //     rollsRemaining.RemoveAt(index);
    // }
}
