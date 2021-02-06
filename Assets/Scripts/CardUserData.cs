using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardUserData", menuName = "Data/CardUserData", order = 0)]
public class CardUserData : ScriptableObject
{
    public string userName;
    public int startingMaxHealth;
    public List<Card> startingDeckPrefabs;
    public Card cardReward;
}
