using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskCardsCache 
{
    private static DeskCardsCache instance;
    private List<Card> library;
    private CharacterType ctype;
    private CardsType rule;

    private DeskCardsCache()
    {
        library = new List<Card>();
        ctype = CharacterType.Desk;
        rule = CardsType.None;
    }
    public void Init()
    { 
    
    }
    public static DeskCardsCache Instance
    {
        get {
            if (instance == null)
                 instance=new DeskCardsCache();
            return instance;
        }
    }
    public CardsType Rule
    {
        set { rule = value; }
        get { return rule; }
    }
    public Card this[int index]
    {
        get {
            return library[index];
        }
    }
    public int CardsCount {
        get { return library.Count; }
    }
    public int MinWeight {
        get { return (int)library[0].GetCardWeight(); }
    }

    public int TotalWeight {
        get {
            return GameController.GetWeight(library.ToArray(), rule);
        }
    }
    public Card Deal()
    {
        Card ret = library[library.Count - 1];
        library.Remove(ret);
        return ret;
    }
    public void AddCard(Card card)
    {
        card.Attribution = ctype;
        library.Add(card);
    }
    public void Clear()
    {
        if (library.Count != 0)
        {
            CardSprite[] cardSprites = GameObject.Find("Desk").GetComponentsInChildren<CardSprite>();
            for (int i = 0; i < cardSprites.Length; i++)
            {
                cardSprites[i].transform.parent = null;
                cardSprites[i].Destroy();
            }

            while (library.Count != 0)
            {
                Card card = library[library.Count - 1];
                library.Remove(card);
                Deck.Instance.AddCard(card);
            }

            rule = CardsType.None;
        }
    }
    public void Sort()
    {
        CardRules.SortCards(library, true);
    }
}
