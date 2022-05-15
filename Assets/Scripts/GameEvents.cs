using System.Collections.Generic;
using UnityEngine.Events;
using CardData;

/* this class is used for holding the instances and classes of the needed events
    for easy reference */
public static class GameEvents
{
    public static UnityEvent startTurn;
    public static UnityEvent endTurn;
    public static UnityEvent goToNextTurn;
    
    public static UnityEvent deckGenerated;
    public static DealCardsEvent dealCards;
    public static UnityEvent cardsAreDoneBeingDealt;
    public static SendDeckToGameEvent sendDeckToDeckStorage;
    public static UnityEvent deckOutOfCards;
    public static SendDiscardToDeckEvent sendDiscardToDeck;
    public static UnityEvent cardMovedToDeck;

    public static CardDataLoadedEvent cardDataLoaded;
    public static CardDealtEvent cardDealt;
    public static CardPlayedEvent cardPlayed;
    public static CardDiscardedEvent cardDiscarded;
    public static CardCannotPlayEvent cardCannotPlay;
    public static UnityEvent cardBeingPlayed;
    public static UnityEvent cardDoneBeingPlayed;

    #region energyEvents

    // these two are used to alert the energy counter to update itself
    public static EnergyUsedEvent energyUsed;
    public static UnityEvent energyReset;

    // this is to let everything in the game know how much energy is left (that needs to know)
    public static EnergyRemainingEvent energyRemaining;

    #endregion
}

public class DealCardsEvent : UnityEvent<int>
{
}

public class CardDataLoadedEvent : UnityEvent<CardList>
{
}

public class CardDealtEvent : UnityEvent<CardManager>
{
}

public class SendDeckToGameEvent : UnityEvent<List<CardManager>>
{
}

public class SendDiscardToDeckEvent : UnityEvent<List<CardManager>>
{
}

public class CardPlayedEvent : UnityEvent<CardManager>
{
}

public class CardDiscardedEvent : UnityEvent<CardManager>
{
}

public class CardCannotPlayEvent : UnityEvent<CardManager>
{
}

public class EnergyUsedEvent : UnityEvent<int>
{
}

public class EnergyRemainingEvent : UnityEvent<int>
{
}