using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement : MonoBehaviour
{
    // store the tranform of the parent card so that it can be moved
    [SerializeField] private Transform cardTransform;
    public float TimeForMove = 0.5f;

    [Tooltip("the amount of time the card is displayed to the player before being played")]
    public float TimeForDisplayAfterBeingPlayed = 2;

    private float moveSpeed = 20f;

    // these check if the card is moving or if its position needs to be adjusted 
    private bool _cardNeedsToAdjustInHand;
    private bool _isMoving;

    // check if the card has been dragged and needs to go back to the hand
    private bool _returnToHand;

    // also check if it's been discarded
    private bool _isDiscarded;

    // the position the card should be at once it's done moving
    private Vector3 _positionToMoveTo;

    // the offset the card is supposed to be at in the hand
    private Vector3 _handOffset;

    // if it's the right most card in the hand, we handle what the card does on mouse up differently
    public bool isRightMostCard;

    private void Awake()
    {
        // listen for the discard pile to be returned to the deck, so the card will go to the deck
        GameEvents.sendDiscardToDeck.AddListener(returnCardToDeck);
    }

    private void Update()
    {
        // if the card needs to adjust its position, do so after it's completely in the hand
        if (_cardNeedsToAdjustInHand && !_isMoving)
        {
            AdjustCardInHand();
        }

        // if the card was dragged, move it back to the hand
        if (_returnToHand) ReturnCardToHand();

        // rotate the card to point at the handLookAtPosition, but only if it's not discarded
        if (!_isDiscarded)
        {
            cardTransform.up = -(UIPositions.handLookAtPointPosition - cardTransform.position);
        }
    }


    #region UnityMouseEvents

    // OnMouseEnter and OnMouseExit handle what happens when the player mouses over a card, in this case move it up
    private void OnMouseEnter()
    {
        // generate the offset for when the card is moused over
        Vector3 mouseOverPosition = cardTransform.position;
        mouseOverPosition.y += 2;

        // if the card is the right most in the hand, we have to subtly move it right instead of left
        // to avoid the card flashing like mad
        if (isRightMostCard)
        {
            mouseOverPosition.x += 0.3f;
        }
        else if (!isRightMostCard)
        {
            mouseOverPosition.x -= 1;
        }

        // check that the mouse isn't draggging another card, and that the card is actually in the hand
        if (!Input.GetMouseButton(0) &&
            (cardTransform.position == UIPositions.handPosition + _handOffset ||
             cardTransform.position == mouseOverPosition))
        {
            cardTransform.position = mouseOverPosition;
        }
    }

    private void OnMouseExit()
    {
        cardTransform.position = _positionToMoveTo;
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
        // if the card in the upper middle of the screen, a card is not being played, and there's no cards being dealt, play it
        if (isMouseInUpperHalfOfScreen() && isMouseInMiddleOfScreen() && !GameManager.cardIsBeingPlayed &&
            !GameManager.areCardsBeingDealt)
        {
            // play the card
            CardManager card = GetComponent<CardManager>();
            GameEvents.cardPlayed.Invoke(card);
        }

        // if not return it to the hand
        ReturnToHand();
    }

    // check if the mouse is in the upper half of the screen
    private bool isMouseInUpperHalfOfScreen()
    {
        return Input.mousePosition.y > Screen.height / 2;
    }

    // check if the mouse is in the middle of the screen horizontally
    private bool isMouseInMiddleOfScreen()
    {
        return Input.mousePosition.x > (0 + Screen.width / 4) &&
               Input.mousePosition.x < (Screen.width - (Screen.width / 4));
    }

    #endregion

    public void PutCardInStartingPosition()
    {
        // move the card to behind the deck image and scale it to 0 to hide it
        cardTransform.localScale = Vector3.zero;
        cardTransform.position = UIPositions.deckPosition;
    }

    #region HandMovementFunctions

    // adjust the position of the card in the hand to fan them out
    private void AdjustCardInHand()
    {
        cardTransform.position =
            Vector3.MoveTowards(cardTransform.position, _positionToMoveTo, moveSpeed * Time.deltaTime);
        if (cardTransform.position == _positionToMoveTo)
        {
            _cardNeedsToAdjustInHand = false;
        }
    }

    // return the card back to the hand if it's been moved
    private void ReturnCardToHand()
    {
        cardTransform.position = Vector3.MoveTowards(cardTransform.position, _positionToMoveTo, 0.25f);
        if (cardTransform.position == _positionToMoveTo)
        {
            _returnToHand = false;
        }
    }

    // move the card into the hand
    public void MoveToHand(Vector3 offset)
    {
        _isMoving = true;
        _isDiscarded = false;
        _handOffset = offset;
        StartCoroutine(MoveCardToFromHand(UIPositions.handPosition + offset, Vector3.one, TimeForMove));
    }

    // lets the card know it needs to move to a different position in the hand
    public void AdjustInHand(Vector3 offset)
    {
        _handOffset = offset;
        _positionToMoveTo = UIPositions.handPosition + offset;
        _cardNeedsToAdjustInHand = true;
    }

    // used for when the card is released after being dragged around by the player, if it needs to return to the hand
    public void ReturnToHand()
    {
        _returnToHand = true;
    }

    #endregion

    #region DiscardMovementFunctions

    // used for moving the card to the discard pile
    public void MoveToDiscard()
    {
        // make sure the card doesn't go anywhere else
        _isMoving = true;
        // and that it's been marked as discarded
        _isDiscarded = true;
        // if it's the end of the turn, we just immediately move it to the discard pile
        if (GameManager.isEndOfTurn)
        {
            StartCoroutine(MoveCardToFromHand(UIPositions.discardPosition, Vector3.zero, 0.5f));
            return;
        }

        GameEvents.cardBeingPlayed.Invoke();
        // have it move to the center of the screen instead of back to the hand, and have it not be rotated
        Vector3 centerScreen = new Vector3(0, 2, 0);
        _positionToMoveTo = centerScreen;
        cardTransform.eulerAngles = Vector3.zero;
        // and move it to the discard
        StartCoroutine(MoveCardToDiscard());
    }

    // event function for sending the card back to the deck from the discard
    private void returnCardToDeck(List<CardManager> cardList)
    {
        if (cardList.Contains(GetComponent<CardManager>()))
        {
            _isMoving = true;
            StartCoroutine(MoveCardToDeck());
        }
    }

    #endregion

    #region Coroutines

    // the coroutine that handles moving and scaling the card into the hand
    IEnumerator MoveCardToFromHand(Vector3 pointToMoveTo, Vector3 scaleToScaleTo, float moveTime)
    {
        float elapsedTime = 0;
        while (elapsedTime < moveTime)
        {
            Vector3 newPosition = Vector3.Lerp(cardTransform.position, pointToMoveTo, (elapsedTime / moveTime));
            cardTransform.position = newPosition;
            Vector3 newScale = Vector3.Lerp(cardTransform.localScale, scaleToScaleTo, (elapsedTime / moveTime));
            cardTransform.localScale = newScale;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        cardTransform.position = pointToMoveTo;
        cardTransform.localScale = scaleToScaleTo;
        _isMoving = false;
        GameEvents.cardDoneBeingPlayed.Invoke();
    }

    // waits for two seconds before moving the card
    IEnumerator MoveCardToDiscard()
    {
        // making our own timer so we can end it early if the player ends the turn before the display time is up
        float timer = 0;
        while (timer < TimeForDisplayAfterBeingPlayed)
        {
            if (GameManager.isEndOfTurn)
            {
                timer = TimeForDisplayAfterBeingPlayed;
            }

            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        StartCoroutine(MoveCardToFromHand(UIPositions.discardPosition, Vector3.zero, TimeForMove));
    }

    // used to move the card back to the deck from the discard pile. it doesn't need to change scale so we need a different function
    IEnumerator MoveCardToDeck()
    {
        _positionToMoveTo = UIPositions.deckPosition;
        cardTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        float elapsedTime = 0;
        while (elapsedTime < 0.5f)
        {
            Vector3 newPosition = Vector3.Lerp(cardTransform.position, UIPositions.deckPosition, (elapsedTime / 0.5f));
            cardTransform.position = newPosition;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        cardTransform.position = UIPositions.deckPosition;
        cardTransform.localScale = Vector3.zero;
        _isMoving = false;
        GameEvents.cardMovedToDeck.Invoke();
    }

    #endregion
}