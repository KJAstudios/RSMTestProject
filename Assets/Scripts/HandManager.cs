using System;
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
        GameEvents.startTurn.AddListener(dealCardsToHand);
        GameEvents.cardDealt.AddListener(addCardToHand);

        // tell UI positions where the hand is
        UIpositions.handPosition = transform.position;
        UIpositions.handLookAtPointPosition = handLookAtPoint.transform.position;
    }

    // when a turn starts, deal cards to the hand
    private void dealCardsToHand()
    {
        int cardsToDeal = maxHandSize - cardsInHand;
        GameEvents.dealCards.Invoke(cardsToDeal);
    }

    // when a card is dealt, this will properly move and add it to the hand
    private void addCardToHand(CardManager card)
    {
        cardList.Insert(0, card);
        cardsInHand++;
        List<Vector3> cardOffsets = GetCardOffsets();
        // move the incoming card to the hand
        card.MoveToHand(cardOffsets[0]);
        // we also need to set the sort order so that the cards don't merge together
        int sortOrder = 0;
        // then adjust the angle of the cards, and their sort order
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].AdjustPositionInHandTo(cardOffsets[i]);
            cardList[i].SetSortOrder(sortOrder);
            sortOrder += 2;
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

        float currentCardPosition = UIpositions.handPosition.x - ((xOffset * cardsInHand) / 2);

        float angle = 90f;
        float radius = 0.1f;
        float increment = 180f / cardsInHand;

        List<Vector3> returnList = new List<Vector3>();
        for (int i = 0; i < cardsInHand; i++)
        {
            float verticalPosition = (float)Math.Cos(angle) * radius;
            Vector3 curOffset = new Vector3(currentCardPosition, verticalPosition, 0);
            returnList.Add(curOffset);
            currentCardPosition += xOffset;
            angle -= increment;
        }

        return returnList;
    }
}