using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public int basePointPerMatch; //底分
    public int multiples;//全场倍数
    public int MatchType = 0; //0 :Easy  1:Normal
    
	// Use this for initialization
	void Start () {
        multiples = 1;
        basePointPerMatch = 100;
        //根据类型加载

	}
    void InitMenu()
    {
        if (MatchType == 0) {
            multiples=1;
        }
        else if (MatchType == 1)
        {
            multiples = 2;
        }
       
    }
    /// <summary>
    /// 获取指定数组的权值
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="rule"></param>
    /// <returns></returns>
    public static int GetWeight(Card[] cards, CardsType rule)
    {
        int totalWeight = 0;
        if (rule == CardsType.ThreeAndOne || rule == CardsType.ThreeAndTwo)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                if (i < cards.Length - 2)
                {
                    if (cards[i].GetCardWeight() == cards[i + 1].GetCardWeight() &&
                        cards[i].GetCardWeight() == cards[i + 2].GetCardWeight())
                    {
                        totalWeight += (int)cards[i].GetCardWeight();
                        totalWeight *= 3;
                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < cards.Length; i++)
            {
                totalWeight += (int)cards[i].GetCardWeight();
            }
        }

        return totalWeight;
    }
 

    public static void AdjustCardSpritsPosition(CharacterType characterType)
    {
        throw new System.NotImplementedException();
    }

    public static  void UpdateLeftCardsCount(CharacterType characterType, int p)
    {
        throw new System.NotImplementedException();
    }
    public void DealCards()
    {
        Deck.Instance.Shuffle();

        CharacterType currentCharacter = CharacterType.Player;
        for (int i = 0; i < 51; i++)
        { 
           if(currentCharacter== CharacterType.Desk)
           {
               currentCharacter = CharacterType.Player;
           }
           DealTo(currentCharacter);
           currentCharacter++;
        }
        for (int i = 0; i < 3; i++)
        {
            DealTo(CharacterType.Desk);
        }

        for (int i = 1; i < 5; i++)
        {
            MakeHandCardsSprite((CharacterType)i, false);
        }
    }
    /// <summary>
    /// 精灵化角色手牌
    /// </summary>
    /// <param name="type"></param>
    /// <param name="isSelected"></param>
    void MakeHandCardsSprite(CharacterType type, bool isSelected)
    {
        if (type == CharacterType.Desk)
        {
            DeskCardsCache instance = DeskCardsCache.Instance;
            for (int i = 0; i < instance.CardsCount; i++)
            {
                MakeSprite(type, instance[i], isSelected);
            }
        }
        else
        {
            GameObject parentObj = GameObject.Find(type.ToString());
            HandCards cards = parentObj.GetComponent<HandCards>();
            //排序
            cards.Sort();
            //精灵化
            for (int i = 0; i < cards.CardsCount; i++)
            {
                if (!cards[i].isSprite)
                {
                    MakeSprite(type, cards[i], isSelected);
                }
            }

            //显示剩余扑克
            UpdateLeftCardsCount(cards.cType, cards.CardsCount);
        }
        //调整精灵位置
        AdjustCardSpritsPosition(type);
    }
    /// <summary>
    /// 使卡牌精灵化
    /// </summary>
    /// <param name="type"></param>
    /// <param name="card"></param>
    /// <param name="selected"></param>
    void MakeSprite(CharacterType type, Card card, bool selected)
    {
        if (!card.isSprite)
        {
            GameObject obj = Resources.Load("poker") as GameObject;
           // GameObject poker = NGUITools.AddChild(GameObject.Find(type.ToString()), obj);
            obj.transform.SetParent(GameObject.Find(type.ToString()).transform);
            CardSprite sprite = obj.gameObject.GetComponent<CardSprite>();
            sprite.Poker = card;
            sprite.Select = selected;
        }
    }
    void DealTo(CharacterType person)
    {
        if (person == CharacterType.Desk)
        {
            Card movedCard = Deck.Instance.Deal();
            DeskCardsCache.Instance.AddCard(movedCard);
        }
        else {

            GameObject playerObj = GameObject.Find(person.ToString());
            HandCards cards = playerObj.GetComponent<HandCards>();

            Card movedCard = Deck.Instance.Deal();
            cards.AddCard(movedCard);
        }
    }
    /// <summary>
    /// 发底牌
    /// </summary>
    /// <param name="type"></param>
    public void CardsOnTable(CharacterType type)
    {
        GameObject parentObj = GameObject.Find(type.ToString());
        HandCards cards = parentObj.GetComponent<HandCards>();
        cards.Multiples = 2;


        CardSprite[] sprites = GameObject.Find("Desk").GetComponentsInChildren<CardSprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].Destroy();
        }

        while (DeskCardsCache.Instance.CardsCount != 0)
        {
            Card card = DeskCardsCache.Instance.Deal();
            cards.AddCard(card);
        }
        MakeHandCardsSprite(type, true);
        //更新身份
        UpdateIndentity(type, Identity.Landlord);
    }
    /// <summary>
    /// 更新身份
    /// </summary>
    /// <param name="type"></param>
    /// <param name="identity"></param>
    public  void UpdateIndentity(CharacterType type, Identity identity)
    {
        GameObject obj = GameObject.Find(type.ToString()).transform.Find("Identity").gameObject;
        //改变属性
        GameObject.Find(type.ToString()).GetComponent<HandCards>().AccessIdentity = identity;
        //更改显示
        //obj.GetComponent<UISprite>().spriteName = "Identity_" + identity.ToString();
    }
    public void PlayCallBack()
    {
        PlayCard playCard = GameObject.Find("Player").GetComponent<PlayCard>();
        if (playCard.CheckSelectCards())
        { 
          
        }
    }
}
