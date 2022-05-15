using System.Collections;
using System.Collections.Generic;
using CardData;
using UnityEngine;

public class DiscardStorage : MonoBehaviour
{
    private List<CardManager> cardStack;

    private void Awake()
    {
        cardStack = new List<CardManager>();

        //listen for cards being added and removed to the deck
        GameEvents.cardDiscarded.AddListener(CardAdded);
        GameEvents.deckOutOfCards.AddListener(SendDiscardToDeck);
    }

    // add a card to the deck when it gets created or moved from discard
    private void CardAdded(CardManager card)
    {
        cardStack.Add(card);
        card.MoveToDiscard();
    }

    private void SendDiscardToDeck()
    {
        GameEvents.sendDiscardToDeck.Invoke(cardStack);
        cardStack.Clear();
    }
}