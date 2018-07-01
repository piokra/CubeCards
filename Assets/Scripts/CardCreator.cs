using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCreator : MonoBehaviour
{
    private static CardCreator _instance;

    public GameObject CardBase;

    private void Start()
    {
        _instance = this;
    }

    public static CardCreator Instance
    {
        get { return _instance; }
    }

    public GameObject CreateCard(Sprite cardArt, string cardTitle, string cardText, Type spell)
    {
        var card = GameObject.Instantiate(CardBase, Camera.main.transform);

        card.name = cardTitle;

        var cardArtObject = card.transform.Find("Card Art");
        var canvasObject = card.transform.Find("Canvas");

        var cardTitleObject = canvasObject.transform.Find("Card Title");
        var cardTextObject = canvasObject.transform.Find("Card Text");

        var cardArtRenderer = cardArtObject.GetComponent<SpriteRenderer>();
        cardArtRenderer.sprite = cardArt;

        var cardTitleText = cardTitleObject.GetComponent<Text>();
        var cardTextText = cardTextObject.GetComponent<Text>();

        cardTitleText.text = cardTitle;
        cardTextText.text = cardText;

        var cardLogic = card.AddComponent<CardBaseLogic>();
        cardLogic.Spell = spell;
        cardLogic.Name = cardTitle;

        return card;
    }

    public GameObject CreateCard(string name)
    {
        CardData cardData;
        if (StaticCardLibrary.CardDatas.TryGetValue(name, out cardData))
        {
            return CreateCard(cardData.CardArt, cardData.CardTitle, cardData.CardText,
                StaticCardLibrary.LogicTypes[cardData.CardLogic]);
        }
        
        return null;
    }
}