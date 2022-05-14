using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardCounter : MonoBehaviour
{
    public Text counterText;

    private int cardCount;

    // Start is called before the first frame update
    void Awake()
    {
        // tell the game where the discard pile is
        UIpositions.discardPosition = transform.position;
        // listen for when a card is added to the deck, then add one
        GameEvents.cardDiscarded.AddListener(AddCardToDiscard);
        UpdateCounterText();
    }

    // increment the discard counter, but wait two seconds for the card to move there
    private void AddCardToDiscard(CardManager card)
    {
        cardCount++;
        StartCoroutine(WaitForUpdate());
    }

    // decrement the counter when a card is returned to the deck
    private void RemoveCardFromDiscard(CardManager card)
    {
        cardCount--;
        UpdateCounterText();
    }

    private void UpdateCounterText()
    {
        counterText.text = cardCount.ToString();
    }

    IEnumerator WaitForUpdate()
    {
        yield return new WaitForSeconds(2);
        UpdateCounterText();
    }
}
