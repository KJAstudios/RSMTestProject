using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CardRendering : MonoBehaviour
{
    public SortingGroup cardGroup;
    [Tooltip("place the card sprite renderer here")]
    public SpriteRenderer cardRenderer;
    [Tooltip("Place the sprite renderer for the card image here")]
    public SpriteRenderer cardImage;
    [Tooltip("Place the canvas for the card image here")]
    public Canvas cardCanvas;
    [Tooltip("Place the cost text for the card here")]
    public Text costText;
    
    [Tooltip("place the glow material here")]
    public Material glowMaterial;
    [Tooltip("place the default sprite material here")]
    public Material defaultMaterial;

    //set the sort order for the card so that it doesn't appear on cards in front of it
    public void setSortOrder(int sortOrder)
    {
        cardRenderer.sortingOrder = sortOrder;
        cardCanvas.sortingOrder = sortOrder+1;
        cardImage.sortingOrder = sortOrder;
        cardGroup.sortingOrder = sortOrder;
    }
    
    // load the correct image for the card, based on the image_id supplied in the JSON data
    public void LoadCardImage(int image_id)
    {
        switch (image_id)
        {
            case 50:
            {
                cardImage.sprite = Resources.Load<Sprite>("Images/AttackIcon");
                break;
            }
            case 77:
            {
                cardImage.sprite = Resources.Load<Sprite>("Images/DefenceIcon");
                break;
            }
            case 18:
            {
                cardImage.sprite = Resources.Load<Sprite>("Images/SpellIcon");
                break;
            }
        }
    }

    // change the card effects depending on if the card can be played or not
    public void UpdateCardEffects(int energyRemaining, int cardCost)
    {
        if (energyRemaining < cardCost)
        {
            costText.color = Color.red;
            cardRenderer.material = defaultMaterial;
        }
        else if (energyRemaining >= cardCost)
        {
            costText.color = Color.white;
            cardRenderer.material = glowMaterial;
        }
        
    }
}
