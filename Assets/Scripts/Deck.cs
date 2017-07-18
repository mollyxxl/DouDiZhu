using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck 
{
    private static Deck instance;
    private List<Card> library;
    private CharacterType ctype;
    public static Deck Instance {
        get {
            if (instance == null)
                instance = new Deck();
            return instance;
        }
    }
    public int CardsCount {
        get { return library.Count; }
    }
    public Card this[int index]
    {
        get {
            return library[index];
        }
    }
    private Deck()
    {
        library = new List<Card>();
        ctype = CharacterType.Library;
        CreateDeck();
    }
    void CreateDeck()
    {
        for (int color = 0; color < 4; color++)
        {
            for (int value = 0; value < 13; value++)
            {
                Weight w = (Weight)value;
                Suits s = (Suits)color;
                string name = s.ToString() + w.ToString();
                Card card = new Card(name, w, s, ctype);
                library.Add(card);
            }
        }
        //创建大小joker
        Card smallJoker = new Card("SJoker", Weight.SJoker, Suits.None, ctype);
        Card largeJoker = new Card("LJoker", Weight.LJoker, Suits.None, ctype);
        library.Add(smallJoker);
        library.Add(largeJoker);
    }
    /// <summary>
    /// 洗牌
    /// </summary>
    public void Shuffle()
    {
        if (CardsCount == 54)
        {
            System.Random random = new System.Random();
            List<Card> newList = new List<Card>();
            foreach (Card item in library)
            {
                newList.Insert(random.Next(newList.Count + 1), item); //相同的索引时，列表后移
            }
            library.Clear();
            foreach (Card item in newList)
            {
                library.Add(item);
            }
            newList.Clear();
         }
    }
    /// <summary>
    /// 发牌
    /// </summary>
    /// <returns></returns>
    public Card Deal()
    {
        Card ret = library[library.Count - 1];
        library.Remove(ret);
        return ret;
    }
    /// <summary>
    /// 像牌库中添加牌
    /// </summary>
    /// <param name="card"></param>
    public void AddCard(Card card)
    {
        card.Attribution = ctype;
        library.Add(card);
    }

}
