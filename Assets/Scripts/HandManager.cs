using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [Tooltip("the maximum number of cards the player can hold")]
    public int maxHandSize = 4;

    [Tooltip("the maximum distance between cards in the hand")]
    public float maxDistanceBetweenCards = 1.5f;

    [Tooltip("the maximum width the hand can possibly be")]
    public float maxHandWidth = 170f;

    [Tooltip("the gameobject for the bottom of the cards to point to")]
    public GameObject handLookAtPoint;

    private int cardsInHand;
    private List<CardManager> cardList;

    private void Awake()
    {
        cardList = new List<CardManager>();

        // listen for important events
        GameEvents.startTurn.AddListener(DealCardsToHand);
        GameEvents.cardDealt.AddListener(AddCardToHand);
        GameEvents.cardDiscarded.AddListener(CardDiscarded);
        GameEvents.endTurn.AddListener(DiscardHand);

        // tell UI positions where the hand is
        UIpositions.handPosition = transform.position;
        UIpositions.handLookAtPointPosition = handLookAtPoint.transform.position;
    }

    // when a turn starts, deal cards to the hand
    private void DealCardsToHand()
    {
        int cardsToDeal = maxHandSize - cardsInHand;
        GameEvents.dealCards.Invoke(cardsToDeal);
    }

    // when a turn ends, empty the hand
    private void DiscardHand()
    {
        while (cardList.Count > 0)
        {
            CardManager card = cardList[0];
            GameEvents.cardDiscarded.Invoke(card);
        }

        StartCoroutine(WaitToStartNextTurn());
    }

    // when a card is discarded, remove it from the hand, and readjust the cards
    private void CardDiscarded(CardManager card)
    {
        cardList.Remove(card);
        cardsInHand--;
        AdjustCardsInHand(GetCardOffsets());
    }

    // when a card is dealt, this will properly move and add it to the hand
    private void AddCardToHand(CardManager card)
    {
        // add the card to the list and counter
        cardList.Insert(0, card);
        cardsInHand++;

        List<Vector3> cardOffsets = GetCardOffsets();
        // move the incoming card to the hand
        card.MoveToHand(cardOffsets[0]);
        AdjustCardsInHand(cardOffsets);
    }

    // this adjusts the order of the cards in the hand
    private void AdjustCardsInHand(List<Vector3> cardOffsets)
    {
        // we need to set the sort order so that the cards don't merge together
        int sortOrder = 0;
        // then adjust the angle of the cards, and their sort order
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].AdjustPositionInHandTo(cardOffsets[i]);
            cardList[i].SetSortOrder(sortOrder);
            sortOrder += 3;
        }
    }

    // this is to get the offset for each card in the hand relative to the location of the hand
    private List<Vector3> GetCardOffsets()
    {
        float xOffset = maxHandWidth / cardsInHand;
        if (xOffset > maxDistanceBetweenCards)
        {
            xOffset = maxDistanceBetweenCards;
        }

        // this calculates the x distance between the cards
        float currentCardPosition = UIpositions.handPosition.x - ((xOffset * cardsInHand) / 2);

        // these are to calculate the arc for the cards
        float angle = 90f;
        float radius = 0.1f;
        float increment = 180f / cardsInHand;

        // and this is used so that the cards have different z offsets to aid in mouse over detection
        float zOffset = 0.01f;
        float curZ = 0;

        List<Vector3> returnList = new List<Vector3>();
        for (int i = 0; i < cardsInHand; i++)
        {
            float verticalPosition = (float)Math.Cos(angle) * radius;
            Vector3 curOffset = new Vector3(currentCardPosition, verticalPosition, curZ);
            returnList.Add(curOffset);
            currentCardPosition += xOffset;
            curZ -= zOffset;
            angle -= increment;
        }

        return returnList;
    }

    IEnumerator WaitToStartNextTurn()
    {
        yield return new WaitForSeconds(2);
        GameEvents.GoToNextTurn.Invoke();
    }
}