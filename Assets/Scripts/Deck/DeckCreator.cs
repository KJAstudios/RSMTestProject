using System.Collections.Generic;
using CardData;
using UnityEngine;

public class DeckCreator : MonoBehaviour
{
    [Tooltip("the card base to create when the card is created")]
    public CardManager Card;

    [Header("Random Deck Generation Settings")]
    [Tooltip("check if a randomly generated deck is wanted, otherwise use the numbers below")]
    public bool randomizeDeck;
    [Tooltip("how big we want the deck to be if we randomize it")]
    public int randomDeckSize = 9;

    [Header("Determined Deck Generation Settings")]
    [Tooltip("the number of attack cards to put in the deck")]
    public int attackCards = 4;
    [Tooltip("the number of defense cards to put in the deck")]
    public int defenseCards = 3;
    [Tooltip("the number of spell cards to put into the deck")]
    public int spellCards = 2;
    
    private CardList _cardList;
    private List<CardManager> generatedCards;

    private void Awake()
    {
        // add the deck generation event as a listener for when the card data is generated
        GameEvents.cardDataLoaded.AddListener(GenerateDeck);
        
        // tell ui positions where the deck is at
        UIPositions.deckPosition = transform.position;
        
        // and create the list to store the generated deck
        generatedCards = new List<CardManager>();
    }

    private void GenerateDeck(CardList cardList)
    {
        // if the option to randomize the deck is checked, do so
        if (randomizeDeck)
        {
            GenerateRandomDeck(cardList);
            return;
        }
        // else use the constraints given
        GenerateConstrainedDeck(cardList);
    }

    private void GenerateConstrainedDeck(CardList cardList)
    {
        _cardList = cardList;
        // add the attack cards to the deck
        for(int i = 0; i < attackCards; i++)
        {
            CreateCard(_cardList.cards[(int)CardIdNames.Sword]);
        }

        // add the defense cards to the deck
        for (int i = 0; i < defenseCards; i++)
        {
            CreateCard((_cardList.cards[(int)CardIdNames.Shield]));
        }
        
        // add the spell cards to the deck
        for (int i = 0; i < spellCards; i++)
        {
            CreateCard((_cardList.cards[(int)CardIdNames.Fortitude]));
        }

        // send the completed deck to the game
        GameEvents.sendDeckToDeckStorage.Invoke(generatedCards);
    }

    // generates a random deck of cards using the deck size settings
    private void GenerateRandomDeck(CardList cardDataList)
    {
        _cardList = cardDataList;
        for (int i = 0; i < randomDeckSize; i ++)
        {
            // randomly pick the type for the card
            int cardId = (int)PickRandomCard();
            CreateCard(_cardList.cards[cardId]);
        }
        
        //let the game know the deck was created
        GameEvents.sendDeckToDeckStorage.Invoke(generatedCards);
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

    // spawns and sets the data for a card
    private void CreateCard(CardInfo cardData)
    {
        // spawn the cards hidden under the deck
        CardManager curCard = Instantiate(Card, transform.position, Quaternion.identity);
            
        //give the card it's data and tell it to display it
        curCard.SetCardData(cardData);
            
        // tell the card to move itself to the deck
        curCard.PutCardBehindDeck();

        // add a card to the generated deck
        generatedCards.Add(curCard);
    }

    //used for an easier way to refer to each card
    private enum CardIdNames
    {
        Sword, Shield, Fortitude
    }
}
