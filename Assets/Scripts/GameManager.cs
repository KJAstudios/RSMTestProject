using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // start listening for important game events
    private void Awake()
    {
        GameEvents.deckGenerated.AddListener(StartTurn);
    }

    // function used to start a turn
    private void StartTurn()
    {
        //start the turn and reset the energy
        GameEvents.startTurn.Invoke();
        GameEvents.energyReset.Invoke();
    }
}
