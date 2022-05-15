using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckStorage : MonoBehaviour
{
    [Tooltip("the number of seconds between cards being dealt")]
    public float timeBetweenDeals = 1;
    private List<CardManager> deckList;
    // used to keep track of how many cards were not dealt if the deck runs out of cards
    private int remaingsCardToDeal;
    private int cardsPhysicallyHere;

    private void Awake()
    {
        deckList = new List<CardManager>();

        //listen for cards being added and removed to the deck
        GameEvents.sendDeckToDeckStorage.AddListener(DeckReceived);
        GameEvents.dealCards.AddListener(DealCards);
        GameEvents.sendDiscardToDeck.AddListener(DiscardedCardsReceived);
        GameEvents.cardMovedToDeck.AddListener(ContinueDealingCards);
    }

    // add a card to the deck when it gets created or moved from discard
    private void DeckReceived(List<CardManager> cardList)
    {
        ShuffleDeck(cardList);
        deckList.AddRange(cardList);
        cardsPhysicallyHere = cardList.Count;
        GameEvents.deckGenerated.Invoke();
    }

    private void ShuffleDeck(List<CardManager> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            // pick a random place in the card list
            int randomPlace = Random.Range(i, cardList.Count - 1);
            
            // and swap that card with the card at the current index
            CardManager tempCard = cardList[randomPlace];
            cardList[randomPlace] = cardList[i];
            cardList[i] = tempCard;
        }
    }
    
    // called when the cards are sent back to the deck from the discard
    private void DiscardedCardsReceived(List<CardManager> newCards)
    {
        deckList.AddRange(newCards);
        ShuffleDeck(deckList);
    }

    // called once the cards have been physically moved over to the discard pile
    private void ContinueDealingCards()
    {
        cardsPhysicallyHere++;
        if (cardsPhysicallyHere == deckList.Count)
        {
            StartCoroutine(DealCardsOverTime(remaingsCardToDeal));
            remaingsCardToDeal = 0;
        }
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
            if (deckList.Count == 0)
            {
                remaingsCardToDeal = cardsToDeal;
                GameEvents.deckOutOfCards.Invoke();
                yield break;
            }
            CardManager card = deckList[0];
            deckList.Remove(card);
            GameEvents.cardDealt.Invoke(card);
            cardsToDeal--;
            cardsPhysicallyHere--;
            yield return new WaitForSeconds(timeBetweenDeals);
        }
        GameEvents.cardsAreDoneBeingDealt.Invoke();
    }
}