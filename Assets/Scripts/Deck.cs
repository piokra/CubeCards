using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<string> CardsInDeck = new List<string>();
    private List<string> _cardsInStack = new List<string>();
    private List<string> _returningCards = new List<string>();
    private List<GameObject> _cardsInTray = new List<GameObject>();


    // Use this for initialization
    void Start()
    {
        _cardsInStack = CardsInDeck.ToList();
        StartCoroutine(DealCards());
    }

    IEnumerator DealCards()
    {
        while (PlayerLogic.Instance == null)
            yield return new WaitForFixedUpdate();

        while (true)
        {
            if (_cardsInStack.Count == 0)
            {
                _cardsInStack = _returningCards.ToList();
                _returningCards.Clear();
                yield return new WaitForSeconds(DealDelay());
            }
            else
            {
                var front = _cardsInStack[0];
                _cardsInStack.RemoveAt(0);
                var card = CardCreator.Instance.CreateCard(front).GetComponent<CardBaseLogic>();
                card.Deck = this;
                _cardsInTray.Add(card.gameObject);
                BalanceTray();
                yield return new WaitForSeconds(DealDelay());
            }
        }
    }

    private void BalanceTray()
    {
        var dx = 5f / _cardsInTray.Count;

        for (int i = 0; i < _cardsInTray.Count; i++)
        {
            _cardsInTray[i].transform.localPosition = new Vector3(-2.5f+dx*i, -1.5f, 3);
        }
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void PlayCard(GameObject card)
    {
        _cardsInTray.Remove(card);
        BalanceTray();
        
    }
    
    public void ReturnCard(string card)
    {
        _returningCards.Add(card);
        
    }

    public float DealDelay()
    {
        var intel = PlayerLogic.Instance.GetStat(CubeObject.Stat.Intelligence);
        return 5f / (1 + intel);
    }
}