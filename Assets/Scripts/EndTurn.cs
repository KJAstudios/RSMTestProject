using UnityEngine;

public class EndTurn : MonoBehaviour
{
    public void PressEndTurn()
    {
        if (GameManager.CanTurnEnd())
        {
            GameEvents.endTurn.Invoke();
        }
    }
}
