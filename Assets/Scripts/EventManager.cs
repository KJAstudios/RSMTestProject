using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // this class is used to create all the needed events at runtime
    private void Awake()
    {
        // create instances of all the different events
        GameEvents.startTurn = new UnityEvent();
        GameEvents.endTurn = new UnityEvent();
        GameEvents.goToNextTurn = new UnityEvent();
        
        GameEvents.deckGenerated = new UnityEvent();
        GameEvents.dealCards = new DealCardsEvent();
        GameEvents.cardsAreDoneBeingDealt = new UnityEvent();
        GameEvents.deckOutOfCards = new UnityEvent();
        GameEvents.sendDiscardToDeck = new SendDiscardToDeckEvent();
        GameEvents.cardMovedToDeck = new UnityEvent();
        
        GameEvents.cardDataLoaded = new CardDataLoadedEvent();
        GameEvents.sendDeckToDeckStorage = new SendDeckToGameEvent();
        GameEvents.cardDealt = new CardDealtEvent();
        GameEvents.cardPlayed = new CardPlayedEvent();
        GameEvents.cardDiscarded = new CardDiscardedEvent();
        GameEvents.cardCannotPlay = new CardCannotPlayEvent();
        GameEvents.cardBeingPlayed = new UnityEvent();
        GameEvents.cardDoneBeingPlayed = new UnityEvent();
        
        GameEvents.energyUsed = new EnergyUsedEvent();
        GameEvents.energyReset = new UnityEvent();
        GameEvents.energyRemaining = new EnergyRemainingEvent();
    }
}
