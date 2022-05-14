using System.Collections;
using System.Collections.Generic;
using CardData;
using UnityEngine;

public class DiscardStorage : MonoBehaviour
{
    private Stack<CardManager> cardStack;

    private void Awake()
    {
        cardStack = new Stack<CardManager>();

        //listen for cards being added and removed to the deck
        GameEvents.cardDiscarded.AddListener(CardAdded);
    }

    // add a card to the deck when it gets created or moved from discard
    private void CardAdded(CardManager card)
    {
        cardStack.Push(card);
        card.MoveToDiscard();
    }
}