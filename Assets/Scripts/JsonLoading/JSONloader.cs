using System.IO;
using System.Net;
using CardData;
using UnityEngine;

public class JSONloader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Send a request to get the JSON info
        WebRequest cardRequest =
            WebRequest.Create(
                "https://client.game.kote.robotseamonster.com/TEST_HARNESS/json_files/cards.json");
        HttpWebResponse cardData = (HttpWebResponse)cardRequest.GetResponse();

        // get the acutal JSON data from the request
        StreamReader dataReader = new StreamReader(cardData.GetResponseStream());
        string cardDataString = dataReader.ReadToEnd();
        
        // turn the JSON info into usable data
        CardList cardList = JsonUtility.FromJson<CardList>(cardDataString);
        
        //notify the game that the card info has been loaded
        GameEvents.cardDataLoaded.Invoke(cardList);
    }
}