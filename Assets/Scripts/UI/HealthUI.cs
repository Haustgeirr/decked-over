using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [HideInInspector]
    public CardUser user;

    public Transform currentHealthPanel;
    public Transform maxHealthPanel;

    public TextMeshProUGUI nameText;

    public GameObject currentHealthPrefab;
    public GameObject maxHealthPrefab;
    public GameObject armourPrefab;

    public List<GameObject> maxHealthIcons;
    public List<GameObject> currentHealthIcons;
    public List<GameObject> armourIcons;

    public void UpdateHealthUI()
    {
        // Debug.Log("update hp ui");
        if (user.maxHealth > maxHealthIcons.Count)
        {
            var amount = user.maxHealth - maxHealthIcons.Count;

            for (int i = 0; i < amount; i++)
            {
                var mhp = Instantiate(maxHealthPrefab, Vector3.zero, Quaternion.identity, maxHealthPanel);
                maxHealthIcons.Add(mhp);
            }
        }
        else if (user.maxHealth < maxHealthIcons.Count)
        {
            for (int i = maxHealthIcons.Count - 1; i >= user.maxHealth; i--)
            {
                var mhp = maxHealthIcons[i];
                maxHealthIcons.RemoveAt(i);
                Destroy(mhp);
            }
        }

        if (user.currentHealth > currentHealthIcons.Count)
        {
            var amount = user.currentHealth - currentHealthIcons.Count;

            for (int i = 0; i < amount; i++)
            {
                var ar = Instantiate(currentHealthPrefab, Vector3.zero, Quaternion.identity, currentHealthPanel);
                currentHealthIcons.Add(ar);
            }
        }
        else if (user.currentHealth < currentHealthIcons.Count)
        {
            for (int i = currentHealthIcons.Count - 1; i >= user.currentHealth; i--)
            {
                var hp = currentHealthIcons[i];
                currentHealthIcons.RemoveAt(i);
                Destroy(hp);
            }
        }

        if (user.armour > armourIcons.Count)
        {
            var amount = user.armour - armourIcons.Count;

            for (int i = 0; i < amount; i++)
            {
                var ar = Instantiate(armourPrefab, Vector3.zero, Quaternion.identity, currentHealthPanel);
                armourIcons.Add(ar);
            }
        }
        else if (user.armour < armourIcons.Count)
        {
            for (int i = armourIcons.Count - 1; i >= user.armour; i--)
            {
                var ar = armourIcons[i];
                armourIcons.RemoveAt(i);
                Destroy(ar);
            }
        }
    }

    public void InitHealthUI(CardUser user)
    {
        // Debug.Log("init hp ui");
        this.user = user;
        nameText.text = user.data.userName;

        // reset max health icons
        if (maxHealthIcons.Count > 0)
        {
            for (int i = maxHealthIcons.Count - 1; i >= 0; i--)
            {
                var mhp = maxHealthIcons[i];
                maxHealthIcons.RemoveAt(i);
                Destroy(mhp, 0.01f);
            }
        }

        maxHealthIcons = new List<GameObject>();
        currentHealthIcons = new List<GameObject>();
        armourIcons = new List<GameObject>();

        for (int i = 0; i < user.maxHealth; i++)
        {
            var mh = Instantiate(maxHealthPrefab, Vector3.zero, Quaternion.identity, maxHealthPanel);
            var ch = Instantiate(currentHealthPrefab, Vector3.zero, Quaternion.identity, currentHealthPanel);

            maxHealthIcons.Add(mh);
            currentHealthIcons.Add(ch);
        }
    }
}
