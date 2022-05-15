using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckCounter : MonoBehaviour
{
    public Text counterText;

    private int cardCount;

    // Start is called before the first frame update
    void Awake()
    {
        // listen for when a card is added to the deck, then add one
        GameEvents.sendDeckToDeckStorage.AddListener(ReceiveDeck);
        // also listen for when a card is dealt, to lower the counter
        GameEvents.cardDealt.AddListener(RemoveCardFromDeck);
        // listen for when the discard is returned to the deck
        GameEvents.sendDiscardToDeck.AddListener(ReceiveDeck);
    }

    private void ReceiveDeck(List<CardManager> cardList)
    {
        cardCount += cardList.Count;
        counterText.text = cardCount.ToString();
    }

    private void RemoveCardFromDeck(CardManager card)
    {
        cardCount--;
        counterText.text = cardCount.ToString();
    }
}