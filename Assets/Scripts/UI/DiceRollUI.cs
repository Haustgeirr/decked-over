using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollUI : MonoBehaviour
{
    public Player player;
    public DieUI[] diceIcons;

    public void UpdateUI()
    {
        for (int i = 0; i < diceIcons.Length; i++)
        {
            var dieValue = i + 1;

            if (player.diceroll.rollsRemaining.Contains(dieValue))
            {
                diceIcons[i].gameObject.SetActive(true);
            }
            else
            {
                diceIcons[i].gameObject.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < diceIcons.Length; i++)
        {
            var dieValue = i + 1;

            if (dieValue == player.chosenDiceRoll)
            {
                diceIcons[i].SelectIcon();
            }
            else
            {
                diceIcons[i].DeselectIcon();
            }
        }
    }
}
