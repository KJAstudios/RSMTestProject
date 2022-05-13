using System;
using CardData;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckCreator : MonoBehaviour
{
    [Tooltip("the card base to create when the card is created")]
    public CardInfoUpdater Card;
    [Tooltip("how big we want the deck to be, default 9")]
    public int deckSize = 9;
    private CardList _cardList;

    private void Awake()
    {
        // add the deck generation event as a listener for when the card data is generated
        GameEvents.cardDataLoaded.AddListener(GenerateDeck);
    }

    private void GenerateDeck(CardList cardDataList)
    {
        _cardList = cardDataList;
        int xCoord = 0;
        for (int i = 0; i < deckSize; i ++)
        {
            // spawn the cards hidden under the deck
            CardInfoUpdater curCard = Instantiate(Card, transform.position, Quaternion.identity);
            // we convert the enum to an int right away, because we dont care about the name anymore
            int cardId = (int)PickRandomCard();
            //give the card it's data and tell it to display it
            // we can use i for the render order because we're just stacking them at this point
            curCard.SetCardData(_cardList.cards[cardId], i);
            curCard.UpdateCardText();
            
            // show that a card was added to the deck
            GameEvents.cardAddedToDeck.Invoke();
        }
    }

    // picks a random card with each one having a chance out of 9 with the ratio 4/3/2
    private CardIdNames PickRandomCard()
    {
        int randomCard = Random.Range(0, 8);
        if (randomCard < 4)
        {
            return CardIdNames.Sword;
        }
        if (randomCard < 6)
        {
            return CardIdNames.Shield;
        }

        return CardIdNames.Fortitude;
    }

    //used for an easier way to refer to each card
    private enum CardIdNames
    {
        Sword, Shield, Fortitude
    }
}
