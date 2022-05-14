using System.Collections;
using UnityEngine;

public class CardMovement : MonoBehaviour
{
    // store the tranform of the parent card so that it can be moved
    [SerializeField] private Transform cardTransform;
    public float TimeForMove = 0.5f;
    private float moveSpeed = 10f;
    
    // these check if the card is moving or if its position needs to be adjusted 
    private bool cardNeedsToAdjustInHand;
    private bool isMoving;
    
    // the position the card should be at once it's done moving
    private Vector3 positionToMoveTo;

    private void Update()
    {
        // if the card needs to adjust its position, do so after it's completely in the hand
        if (cardNeedsToAdjustInHand && !isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionToMoveTo, moveSpeed * Time.deltaTime);
            if (transform.position == positionToMoveTo)
            {
                cardNeedsToAdjustInHand = false;
            }
        }
        // rotate the card to point at the handLookAtPosition
        transform.up = -(UIpositions.handLookAtPointPosition - cardTransform.position);
        }

    public void PutCardInStartingPosition()
    {
        // move the card to behind the deck image and scale it to 0 to hide it
        cardTransform.localScale = Vector3.zero;
        cardTransform.position = UIpositions.deckPosition;
    }

    // move the card into the hand
    public void MoveToHand(Vector3 offset)
    {
        isMoving = true;
        StartCoroutine(MoveCardIntoHand(UIpositions.handPosition + offset, Vector3.one));
    }
    
    // lets the card know it needs to move to a different position in the hand
    public void AdjustInHand(Vector3 offset)
    {
        positionToMoveTo = UIpositions.handPosition + offset;
        cardNeedsToAdjustInHand = true;
        
    }

    // the coroutine that handles moving and scaling the card into the hand
    IEnumerator MoveCardIntoHand(Vector3 pointToMoveTo, Vector3 scaleToScaleTo)
    {
        float elapsedTime = 0;
        while (elapsedTime < TimeForMove)
        {
            Vector3 newPosition = Vector3.Lerp(cardTransform.position, pointToMoveTo, (elapsedTime / TimeForMove));
            cardTransform.position = newPosition;
            Vector3 newScale = Vector3.Lerp(cardTransform.localScale, scaleToScaleTo, (elapsedTime / TimeForMove));
            cardTransform.localScale = newScale;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        cardTransform.position = pointToMoveTo;
        cardTransform.localScale = scaleToScaleTo;
        isMoving = false;
    }
}