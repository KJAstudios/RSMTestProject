using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // this class is used to create all the needed events at runtime
    void Awake()
    {
        // create instances of all the different events
        GameEvents.cardDataLoaded = new CardDataLoadedEvent();
        GameEvents.cardAddedToDeck = new UnityEvent();
        GameEvents.energyUsed = new EnergyUsedEvent();
        GameEvents.energyReset = new UnityEvent();
    }
}
