using System;
using System.Threading;

namespace CardData
{
    [Serializable]
    public class CardList
    {
        public CardInfo[] cards;
    }

    [Serializable]
    public class CardInfo
    {
        public int id;
        public string name;
        public int cost;
        public string type;
        public int image_id;
        public CardEffects[] effects;
    }

    [Serializable]
    public class CardEffects
    {
        public string type;
        public int value;
        public string target;
    }
}