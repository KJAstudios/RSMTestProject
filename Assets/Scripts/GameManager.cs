using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isEndOfTurn;
    // start listening for important game events
    private void Awake()
    {
        // this call is only for when the game starts, to know that the deck has been created
        GameEvents.deckGenerated.AddListener(StartTurn);
        // this is called at all other times, when the turn starts
        GameEvents.GoToNextTurn.AddListener(StartTurn);
        GameEvents.endTurn.AddListener(EndTurn);
    }

    // function used to start a turn
    private void StartTurn()
    {
        //start the turn and reset the energy
        isEndOfTurn = false;
        GameEvents.startTurn.Invoke();
        GameEvents.energyReset.Invoke();
    }

    private void EndTurn()
    {
        isEndOfTurn = false;
    }
}
