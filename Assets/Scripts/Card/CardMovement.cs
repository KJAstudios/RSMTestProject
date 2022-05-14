using System;
using System.Collections;
using TMPro;
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

    // check if the card has been dragged and needs to go back to the hand
    private bool returnToHand;

    // also check if it's been discarded
    private bool isDiscarded;

    // the position the card should be at once it's done moving
    private Vector3 positionToMoveTo;

    private void Update()
    {
        // if the card needs to adjust its position, do so after it's completely in the hand
        if (cardNeedsToAdjustInHand && !isMoving)
        {
            adjustCardInHand();
        }

        // if the card was dragged, move it back to the hand
        if (returnToHand) returnCardToHand();

        // rotate the card to point at the handLookAtPosition, but only if it's not discarded
        if (!isDiscarded)
        {
            cardTransform.up = -(UIpositions.handLookAtPointPosition - cardTransform.position);
        }
    }

    // adjust the position of the card in the hand to fan them out
    private void adjustCardInHand()
    {
        cardTransform.position =
            Vector3.MoveTowards(cardTransform.position, positionToMoveTo, moveSpeed * Time.deltaTime);
        if (cardTransform.position == positionToMoveTo)
        {
            cardNeedsToAdjustInHand = false;
        }
    }

    // return the card back to the hand if it's been moved
    private void returnCardToHand()
    {
        cardTransform.position = Vector3.MoveTowards(cardTransform.position, positionToMoveTo, 0.25f);
        if (cardTransform.position == positionToMoveTo)
        {
            returnToHand = false;
        }
    }

    // OnMouseEnter and OnMouseExit handle what happens when the player mouses over a card, in this case move it up
    private void OnMouseEnter()
    {
        // generate the offset for when the card is moused over
        Vector3 mouseOverPosition = cardTransform.position;
        mouseOverPosition.y += 2;
        mouseOverPosition.x -= 0.5f;
        // check that the mouse isn't draggging another card, and that the card is actually in the hand
        if (!Input.GetMouseButton(0) &&
            (cardTransform.position == positionToMoveTo || cardTransform.position == mouseOverPosition))
        {
            cardTransform.position = mouseOverPosition;
        }
    }

    private void OnMouseExit()
    {
        cardTransform.position = positionToMoveTo;
    }


    // OnMouseDrag and OnMouseUp handle how the card moves when it's dragged
    private void OnMouseDrag()
    {
        Vector3 cardPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cardPosition.z = 0;
        cardTransform.position = cardPosition;
    }

    private void OnMouseUp()
    {
        // if the card is in the upper half of the screen and middle of the screen, try to play it
        if (Input.mousePosition.y > Screen.height / 2 &&Input.mousePosition.x > (0 + Screen.width / 4) &&
                                                          Input.mousePosition.x < (Screen.width - (Screen.width / 4)))
        {
            // play the card
            CardManager card = GetComponent<CardManager>();
            GameEvents.cardPlayed.Invoke(card);
        }

        // if not return it to the hand
        ReturnToHand();
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
        isDiscarded = false;
        StartCoroutine(MoveCardToFromHand(UIpositions.handPosition + offset, Vector3.one));
    }

    // lets the card know it needs to move to a different position in the hand
    public void AdjustInHand(Vector3 offset)
    {
        positionToMoveTo = UIpositions.handPosition + offset;
        cardNeedsToAdjustInHand = true;
    }

    // used for when the card is released after being dragged around by the player, if it needs to return to the hand
    public void ReturnToHand()
    {
        returnToHand = true;
    }

    // used for moving the card to the discard pile
    public void MoveToDiscard()
    {
        // make sure the card doesn't go anywhere else
        isMoving = true;
        // and that it's been marked as discarded
        isDiscarded = true;
        // have it move to the center of the screen instead of back to the hand, and have it not be rotated
        Vector3 centerScreen = new Vector3(0, 0, 0);
        positionToMoveTo = centerScreen;
        cardTransform.eulerAngles = Vector3.zero;
        // and move it to the discard
        StartCoroutine(MoveCardToDiscard());
    }

    // the coroutine that handles moving and scaling the card into the hand
    IEnumerator MoveCardToFromHand(Vector3 pointToMoveTo, Vector3 scaleToScaleTo)
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

    // waits for two seconds before moving the card
    IEnumerator MoveCardToDiscard()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(MoveCardToFromHand(UIpositions.discardPosition, Vector3.zero));
    }
}