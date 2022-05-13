using System.Collections;
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
        GameEvents.cardAddedToDeck.AddListener(AddCardToDeck);
    }

    private void AddCardToDeck()
    {
        cardCount += 1;
        counterText.text = cardCount.ToString();
    }
}
