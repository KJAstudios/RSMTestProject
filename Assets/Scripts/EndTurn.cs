using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    public void PressEndTurn()
    {
        GameEvents.endTurn.Invoke();
    }
}
