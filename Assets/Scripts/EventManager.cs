using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // this class is used to create all the needed events at runtime
    void Awake()
    {
        // create instances of all the different events
        GameEvents.startTurn = new UnityEvent();
        
        GameEvents.deckGenerated = new UnityEvent();
        GameEvents.dealCards = new DealCardsEvent();
        
        GameEvents.cardDataLoaded = new CardDataLoadedEvent();
        GameEvents.cardAddedToDeck = new CardAddedToDeckEvent();
        GameEvents.cardDealt = new CardDealtEvent();
        GameEvents.cardPlayed = new CardPlayedEvent();
        GameEvents.cardDiscarded = new CardDiscardedEvent();
        GameEvents.cardCannotPlay = new CardCannotPlayEvent();
        
        GameEvents.energyUsed = new EnergyUsedEvent();
        GameEvents.energyReset = new UnityEvent();
        GameEvents.energyRemaining = new EnergyRemainingEvent();
    }
}
