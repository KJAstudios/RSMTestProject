using UnityEngine.Events;
using CardData;

/* this class is used for holding the instances and classes of the needed events
    for easy reference */
public static class GameEvents
{
    public static UnityEvent startTurn;
    
    public static UnityEvent deckGenerated;
    public static DealCardsEvent dealCards;
    
    
    public static CardDataLoadedEvent cardDataLoaded;
    public static CardAddedToDeckEvent cardAddedToDeck;
    public static CardDealtEvent cardDealt;

    #region energyEvents

    public static EnergyUsedEvent energyUsed;
    public static UnityEvent energyReset;

    #endregion
}

public class DealCardsEvent : UnityEvent<int>{}

public class CardDataLoadedEvent : UnityEvent<CardList>
{
}

public class CardDealtEvent : UnityEvent<CardManager>
{
}

public class CardAddedToDeckEvent : UnityEvent<CardManager>
{
}

public class EnergyUsedEvent : UnityEvent<int>
{
}