using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUser : MonoBehaviour
{
    [Header("Data")]
    public CardUserData data;

    [Header("Health")]
    public int maxHealth;
    public int currentHealth;
    public HealthUI healthUI;
    public int armour;

    [Header("Deck")]
    public List<Card> deck;
    public Card chosenCard;
    public int chosenCardIndex;
    public int previousCardIndex;
    public Card previousChosenCard;

    [Header("Deck UI")]
    public Transform chosenCardPos;
    public Transform[] deckCardPos;
    public Vector3 deckScale;

    public int chosenDiceRoll;
    public Diceroll diceroll;

    protected TurnManager _turnManager;
    private CardUser _target;
    private bool _passedLastTurn = false;

    [Header("Audio")]
    public AudioSource source;

    public AudioClip drawCardSound;

    public void PlayDrawCardSound()
    {
        source.clip = drawCardSound;
        source.pitch = Random.Range(1f, 1.1f);
        source.Play();
    }

    public IEnumerator CardAnim(GameObject card, Vector3 endPos, Quaternion endRot)
    {
        var tf = card.transform;
        var startPosition = tf.position;
        var startRotation = tf.rotation;

        var moveTimer = 0f;
        var moveDuration = 0.1f;

        while (moveTimer < moveDuration)
        {
            tf.position = Vector3.Lerp(startPosition, endPos, Math3D.CubicEaseOut(moveTimer, moveDuration));
            tf.rotation = Quaternion.Lerp(startRotation, endRot, Math3D.CubicEaseOut(moveTimer, moveDuration));
            moveTimer += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        yield return null;
    }

    public void Heal(int amount)
    {
        if (amount > 0)
            currentHealth = (currentHealth + amount) < maxHealth ? currentHealth + amount : maxHealth;

        if (healthUI != null)
            healthUI.UpdateHealthUI();
    }

    public void Spoil(int amount)
    {
        // if (_target.chosenDiceRoll <= amount)
        //     _target.chosenDiceRoll = 1;

        _target.chosenDiceRoll = amount;
    }

    // This should be changed to steal the card the opponent is about to use
    public void Steal(int amount)
    {
        for (int i = amount - 1; i >= 0; i--)
        {
            if (_target.diceroll.rollsRemaining.Contains(i + 1))
            {
                if (diceroll.AddRoll(i + 1))
                {
                    _target.diceroll.ConsumeRoll(i + 1);
                    return;
                }
            }
        }
    }

    public virtual void DealDamage(int amount)
    {
        _turnManager.DealDamage(amount, this);
    }

    public virtual void TakeDamage(int amount)
    {
        if (armour > 0)
        {
            if (amount - armour > 0)
            {
                amount -= armour;
                armour = 0;
            }
            else
            {
                armour -= amount;
                amount = 0;
            }
        }

        if (amount > 0)
            currentHealth = (currentHealth - amount) > 0 ? currentHealth - amount : 0;

        if (currentHealth <= 0)
            PassOut();

        if (healthUI != null)
            healthUI.UpdateHealthUI();
    }

    public virtual void AddArmour(int amount)
    {
        this.armour += amount;

        if (healthUI != null)
            healthUI.UpdateHealthUI();
    }

    public virtual void RemoveArmour(int amount)
    {
        armour = (armour - amount) > 0 ? armour - amount : 0;
    }

    public virtual void PassOut()
    {
        Debug.Log(this.transform.name + " has fainted.");
        ReturnCardToDeck();
        _turnManager.EndBattle(this, false);
    }

    public virtual void StartBattle()
    {
        SetTarget();
        healthUI.UpdateHealthUI();

        chosenCardPos.gameObject.SetActive(true);

        foreach (var pos in deckCardPos)
        {
            pos.gameObject.SetActive(true);
        }

        foreach (var card in deck)
        {
            card.gameObject.SetActive(true);
        }
    }

    public virtual void EndBattle()
    {
        armour = 0;
        healthUI.UpdateHealthUI();

        chosenCardPos.gameObject.SetActive(false);

        foreach (var pos in deckCardPos)
        {
            pos.gameObject.SetActive(false);
        }

        foreach (var card in deck)
        {
            card.gameObject.SetActive(false);
        }
    }

    public virtual void StartTurn()
    {
        PlayDrawCardSound();
    }

    public virtual void TakeTurn() { }

    public virtual void ReturnCardToDeck() { }

    public void PassTurn()
    {
        // if already passed, refund a Roll
        if (_passedLastTurn)
        {
            diceroll.AddRoll(Random.Range(1, 7));
            _passedLastTurn = false;
        }

        Debug.Log(this.transform.name + " Pass Turn");
        _passedLastTurn = true;
    }
    public void ChooseCardAtRandom()
    {
        if (deck.Count == 1)
        {
            chosenCardIndex = 0;
        }
        else
        {
            while (chosenCardIndex == previousCardIndex)
            {
                chosenCardIndex = Random.Range(0, deck.Count);
            }
        }

        chosenCard = deck[chosenCardIndex];
        previousCardIndex = chosenCardIndex;
    }

    public int GetPriority()
    {
        return chosenCard.priority;
    }

    protected void RefreshDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].SetCardUser(this);

            var pos = deckCardPos[i].position;

            deck[i].transform.localScale = deckScale;
            deck[i].transform.position = new Vector3(pos.x, pos.y, -1f);
            deck[i].transform.rotation = Quaternion.identity;
        }
    }

    public virtual void InitDeck()
    {
        deck = new List<Card>();

        foreach (var card in data.startingDeckPrefabs)
        {
            var obj = Instantiate(card, Vector3.zero, Quaternion.identity, this.transform);
            deck.Add(obj);
            obj.transform.localScale = deckScale;
            obj.gameObject.SetActive(false);
        }

        RefreshDeck();
    }

    public virtual void InitCardUser()
    {
        maxHealth = data.startingMaxHealth;
        currentHealth = maxHealth;
        armour = 0;

        InitDeck();

        diceroll = new Diceroll();
        diceroll.Init();

        if (healthUI != null)
            healthUI.InitHealthUI(this);
    }

    private void SetTarget()
    {
        _target = (this == _turnManager.player) ? _turnManager.opponent : _turnManager.player;
    }

    private void Awake()
    {
        _turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
    }
}
