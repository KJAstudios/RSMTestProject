using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isEndOfTurn;
    public static bool cardIsBeingPlayed;
    public static bool areCardsBeingDealt;

    [Tooltip("the time between turns")]
    public float timeBetweenTurns = 0.5f;
    public static float timeBetweenTurnsRef;
    // start listening for important game events
    private void Awake()
    {
        // this call is only for when the game starts, to know that the deck has been created
        GameEvents.deckGenerated.AddListener(StartTurn);
        // this is called at all other times, when the turn starts
        GameEvents.goToNextTurn.AddListener(StartTurn);
        GameEvents.cardsAreDoneBeingDealt.AddListener(CardsDoneBeingDealt);
        GameEvents.endTurn.AddListener(EndTurn);
        
        // keeps track of if a card is being played in order to stop certain actions from being played.
        GameEvents.cardBeingPlayed.AddListener(CardBeingPlayed);
        GameEvents.cardDoneBeingPlayed.AddListener(CardDoneBeingPlayed);
        
        // turns the serialized field into a static one
        timeBetweenTurnsRef = timeBetweenTurns;
    }

    public static bool CanTurnEnd()
    {
        if (isEndOfTurn || areCardsBeingDealt)
        {
            return false;
        }

        return true;
    }

    // function used to start a turn
    private void StartTurn()
    {
        //start the turn and reset the energy
        isEndOfTurn = false;
        areCardsBeingDealt = true;
        GameEvents.startTurn.Invoke();
        GameEvents.energyReset.Invoke();
    }

    private void CardsDoneBeingDealt()
    {
        areCardsBeingDealt = false;
    }

    private void EndTurn()
    {
        isEndOfTurn = true;
    }

    private void CardBeingPlayed()
    {
        cardIsBeingPlayed = true;
    }

    private void CardDoneBeingPlayed()
    {
        cardIsBeingPlayed = false;
    }
    
    
}
