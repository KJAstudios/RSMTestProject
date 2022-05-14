using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckStorage : MonoBehaviour
{
    public float timeBetweenDeals = 1;
    private Stack<CardManager> cardStack;

    private void Awake()
    {
        cardStack = new Stack<CardManager>();

        //listen for cards being added and removed to the deck
        GameEvents.cardAddedToDeck.AddListener(CardAdded);
        GameEvents.dealCards.AddListener(DealCards);
    }

    // add a card to the deck when it gets created or moved from discard
    private void CardAdded(CardManager card)
    {
        cardStack.Push(card);
    }

    // function to deal cards to the hand
    private void DealCards(int cardsToDeal)
    {
        StartCoroutine(DealCardsOverTime(cardsToDeal));
    }

    // deals a card every timeBetweenDeals seconds
    IEnumerator DealCardsOverTime(int cardsToDeal)
    {
        while (cardsToDeal > 0)
        {
            GameEvents.cardDealt.Invoke(cardStack.Pop());
            cardsToDeal--;
            yield return new WaitForSeconds(timeBetweenDeals);
        }
    }
}