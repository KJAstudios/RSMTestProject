using UnityEngine.Events;
using CardData;

/* this class is used for holding the instances and classes of the needed events
    for easy reference */
    public static class GameEvents
    {
        public static CardDataLoadedEvent cardDataLoaded;
        public static UnityEvent cardAddedToDeck;
        public static EnergyUsedEvent energyUsed;
        public static UnityEvent energyReset;
    }

    public class CardDataLoadedEvent : UnityEvent<CardList>{}
    public class EnergyUsedEvent : UnityEvent<int>{}
