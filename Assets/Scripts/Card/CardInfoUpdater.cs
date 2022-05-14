using CardData;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoUpdater : MonoBehaviour
{
    // the information for the card, is public for debugging purposes
    public int cost;
    private string cardName;
    private string type;
    private string description;
    private int renderOrder;

    // the different text fields, added here to avoid GetChild() and GetComponent() calls
    [Tooltip("place the cost text for the card here")]
    public Text costText;
    [Tooltip("place the name text for the card here")]
    public Text nameText;
    [Tooltip("place the type text for the card here")]
    public Text typeText;
    [Tooltip("place the description text for the card here")]
    public Text descriptionText;
    
    // function to set the data for this card
    public void SetCardData(CardInfo cardInfo)
    {
        saveCardInformation(cardInfo);
        UpdateCardText();
    }

    // save the card information into it's relevant field
    private void saveCardInformation(CardInfo cardInfo)
    {
        this.cost = cardInfo.cost;
        cardName = cardInfo.name;
        type = cardInfo.type;
        description = GenerateDescriptionText(cardInfo.effects);
    }
    
    // used to update the text on the card
    private void UpdateCardText()
    {
        costText.text = cost.ToString();
        nameText.text = cardName;
        typeText.text = type;
        descriptionText.text = description;
    }

    // generates the description text for the card
    private string GenerateDescriptionText(CardEffects[] cardEffectData)
    {
        string tempDescription = "";
        // add a line to the description for each effect the card has
        foreach(CardEffects cardEffect in cardEffectData)
        {
            tempDescription += DetermineEffectType(cardEffect);
        }

        return tempDescription;
    }

    // used to get the right type of description for each type of effect
    private string DetermineEffectType(CardEffects cardEffect)
    {
        switch (cardEffect.type)
        {
            case "damage":
            {
                return CreateDamageInfoText(cardEffect);
            }
            case "shield":
            {
               return CreateShieldInfoText(cardEffect);
            }
            case "strength":
            {
                return CreateStrengthInfoText(cardEffect);
            }
        }

        return "";
    }

    private string CreateDamageInfoText(CardEffects cardEffect)
    {
        return "Deal " + cardEffect.value + " damage to " + cardEffect.target + "\n";
    }

    private string CreateShieldInfoText(CardEffects cardEffect)
    {
        return "Shield " + cardEffect.target + " for " + cardEffect.value + "\n";
    }
    
    private string CreateStrengthInfoText(CardEffects cardEffect)
    {
        return "Strengthen " + cardEffect.target + " by " + cardEffect.value + "\n";
    }

    
}
