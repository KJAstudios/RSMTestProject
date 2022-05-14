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
        // also listen for when a card is dealt, to lower the counter
        GameEvents.cardDealt.AddListener(RemoveCardFromDeck);
    }

    private void AddCardToDeck(CardManager card)
    {
        cardCount++;
        counterText.text = cardCount.ToString();
    }

    private void RemoveCardFromDeck(CardManager card)
    {
        cardCount--;
        counterText.text = cardCount.ToString();
    }
}