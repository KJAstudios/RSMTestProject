using System.Collections.Generic;
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
        // move the card to the back of the layer so you can still look at your deck
        card.SetSortOrder(-5);
        card.MoveToDiscard();
    }

    // sends the entire discard back over to the deck
    private void SendDiscardToDeck()
    {
        GameEvents.sendDiscardToDeck.Invoke(cardStack);
        cardStack.Clear();
    }
}