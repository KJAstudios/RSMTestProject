using System.Collections;
using System.Collections.Generic;
using CardData;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    /* this is a wrapper to handle any time another script need to do something with
        a card. We don't want to have to pass around the specific script responsible 
        for something, so we use this one instead*/
    [SerializeField]
    private CardInfoUpdater cardInfo;
    [SerializeField]
    private CardMovement cardMovement;
    [SerializeField]
    private CardRendering cardRendering;

    // set the data for this card, and load the correct image
    public void SetCardData(CardInfo cardData)
    {
        cardInfo.SetCardData(cardData);
        LoadCardImage(cardData.image_id);
    }

    // put the card behind the current deck
    public void PutCardBeindDeck()
    {
        cardMovement.PutCardInStartingPosition();
    }

    // move the card to the spot in the hand signified by the offset
    public void MoveToHand(Vector3 offset)
    {
        cardMovement.MoveToHand(offset);
    }

    // move the card to a different spot in the hand
    public void AdjustPositionInHandTo(Vector3 offset)
    {
        cardMovement.AdjustInHand(offset);
    }

    // set the sort order for the card in the sorting layers
    public void SetSortOrder(int layerOrder)
    {
        cardRendering.setSortOrder(layerOrder);
    }

    // load the image for the card
    private void LoadCardImage(int image_id)
    {
        cardRendering.LoadCardImage(image_id);
    }
}
