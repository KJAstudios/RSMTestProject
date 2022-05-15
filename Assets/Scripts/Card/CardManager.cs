using System;
using System.Collections;
using System.Collections.Generic;
using CardData;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    /* this is a wrapper to handle any time another script need to do something with
        a card. We don't want to have to pass around the specific script responsible 
        for something, so we use this one instead.
        This also handles any tasks that need to interact with multiple of the other
        scripts, like playing a card or loading the card data*/
    [SerializeField]
    private CardInfoUpdater cardInfo;
    [SerializeField]
    private CardMovement cardMovement;
    [SerializeField]
    private CardRendering cardRendering;

    private void Awake()
    {
        // listen for if the card failed to be played, so it can return to it's original position
        GameEvents.cardCannotPlay.AddListener(ReturnCardToHand);
        // listen for updates to the amount of energy, so the card can do it's effects
        GameEvents.energyRemaining.AddListener(UpdateCardEffects);
    }

    // set the data for this card, and load the correct image
    public void SetCardData(CardInfo cardData)
    {
        cardInfo.SetCardData(cardData);
        LoadCardImage(cardData.image_id);
    }

    // put the card behind the current deck
    public void PutCardBehindDeck()
    {
        cardMovement.PutCardInStartingPosition();
    }

    // move the card to the spot in the hand signified by the offset
    public void MoveToHand(Vector3 offset)
    {
        cardMovement.MoveToHand(offset);
    }
    
    // move the card to the discard pile
    public void MoveToDiscard()
    {
        cardMovement.MoveToDiscard();
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
    
    // get the energy cost of the card
    public int GetEnergyCost()
    {
        return cardInfo.cost;
    }

    public void SetAsRightMostCard(bool isRightCard)
    {
        cardMovement.isRightMostCard = isRightCard;
    }

    // load the image for the card
    private void LoadCardImage(int image_id)
    {
        cardRendering.LoadCardImage(image_id);
    }
    
    // return the card to the hand if it didn't get played
    private void ReturnCardToHand(CardManager card)
    {
        if (card == this)
        {
            cardMovement.ReturnToHand();
        }
    }

    // update the effects of the card depending on if it can be played or not
    private void UpdateCardEffects(int remainingEnergy)
    {
        cardRendering.UpdateCardEffects(remainingEnergy, GetEnergyCost());
    }
}
