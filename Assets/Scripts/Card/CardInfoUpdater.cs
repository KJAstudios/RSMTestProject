using System;
using CardData;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoUpdater : MonoBehaviour
{
    // the information for the card, is public for debugging purposes
    public int cost;
    private string _cardName;
    private string _type;
    private string _description;

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
        SaveCardInformation(cardInfo);
        UpdateCardText();
    }

    // save the card information into it's relevant field
    private void SaveCardInformation(CardInfo cardInfo)
    {
        this.cost = cardInfo.cost;
        _cardName = cardInfo.name;
        _type = cardInfo.type;
        _description = GenerateDescriptionText(cardInfo.effects);
    }
    
    // used to update the text on the card
    private void UpdateCardText()
    {
        costText.text = cost.ToString();
        nameText.text = CapitalizeName(_cardName);
        typeText.text = _type;
        descriptionText.text = _description;
    }

    // capitalizes the first letter of the name of the card
    private string CapitalizeName(string nameString)
    {
        string nameStr = nameString;
        nameStr = Char.ToUpper(nameStr[0]) + nameStr.Substring(1);
        return nameStr;
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
