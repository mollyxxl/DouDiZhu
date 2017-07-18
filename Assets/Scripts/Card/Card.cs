using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card 
{
    private readonly string cardName;
    private readonly Weight weight;
    private readonly Suits color;
    private CharacterType belongTo;
    private bool makedSprite;
    public Card(string name, Weight weight, Suits color, CharacterType belongTo)
    {
        makedSprite = false;
        cardName = name;
        this.weight = weight;
        this.color = color;
        this.belongTo = belongTo;
    }
    public string GetCardName()
    {
        return cardName;
    }
    public Weight GetCardWeight()
    {
        return weight;
    }
    public Suits GetCardSuit()
    {
        return color;
    }
    /// <summary>
    /// 是否精灵化
    /// </summary>
    public bool isSprite
    {
        set { makedSprite = value; }
        get { return makedSprite; }
    }
    /// <summary>
    /// 牌的归属
    /// </summary>
    public CharacterType Attribution
    {
        set { belongTo = value; }
        get { return belongTo; }
    }

}
