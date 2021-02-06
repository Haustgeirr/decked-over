using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [HideInInspector]
    public CardUser user;

    [Tooltip("Order in which cards are evaluated. Lowest number first.")]
    public int priority;

    public void SetCardUser(CardUser user)
    {
        this.user = user;
    }

    public void Init() { }

    public virtual void DealDamage() { }

    public virtual void Heal() { }

    public void RollDie(int roll)
    {
        switch (roll)
        {
            case 1:
                {
                    RollOne();
                    break;
                }
            case 2:
                {
                    RollTwo();
                    break;
                }
            case 3:
                {
                    RollThree();
                    break;
                }
            case 4:
                {
                    RollFour();
                    break;
                }
            case 5:
                {
                    RollFive();
                    break;
                }
            case 6:
                {
                    RollSix();
                    break;
                }
            default:
                {
                    user.PassTurn();
                    break;
                }

        }
    }

    public abstract void RollOne();
    public abstract void RollTwo();
    public abstract void RollThree();
    public abstract void RollFour();
    public abstract void RollFive();
    public abstract void RollSix();
}
