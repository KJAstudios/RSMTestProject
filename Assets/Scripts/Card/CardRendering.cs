using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class CardRendering : MonoBehaviour
{
    public SortingGroup cardGroup;
    [Tooltip("place the card sprite renderer here")]
    public SpriteRenderer cardRenderer;
    [Tooltip("Place the sprite renderer for the card image here")]
    public SpriteRenderer cardImage;
    [Tooltip("Place the canvas for the card image here")]
    public Canvas cardCanvas;
    

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
                cardImage.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Images/AttackIcon.png", typeof(Sprite));
                break;
            }
            case 77:
            {
                cardImage.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Images/DefenceIcon.png", typeof(Sprite));
                break;
            }
            case 18:
            {
                cardImage.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Images/SpellIcon.png", typeof(Sprite));
                break;
            }
        }
    }
}
