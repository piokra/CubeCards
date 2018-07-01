using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardData
{
    public string CardTitle;
    public string CardText;
    public Sprite CardArt;
    public string CardLogic;
}

public class CardLibrary : MonoBehaviour
{
    public CardData[] CardDatas;


    // Use this for initialization
    void Start()
    {
        foreach (var cardData in CardDatas)
        {
            StaticCardLibrary.CardDatas[cardData.CardTitle] = cardData;
        }
    }

}

public static class StaticCardLibrary
{
    public static Dictionary<string, CardData> CardDatas = new Dictionary<string, CardData>();
    public static Dictionary<string, Type> LogicTypes = new Dictionary<string, Type>()
    {
        {"Base", typeof(SpellBase)},
        {"Fireball", typeof(Fireball)},
        {"Thunderball", typeof(Thunderball)},
        {"Heal", typeof(Heal)},
        {"Blink", typeof(Blink)}
    };
}

