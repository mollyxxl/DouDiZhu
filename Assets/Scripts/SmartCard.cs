using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmartCard:MonoBehaviour
{
    protected GameObject aiNotice;
    void Start()
    {
        aiNotice = transform.Find("Notice").gameObject;
    }
    public virtual IEnumerator DelayDiscardCard(bool isNone)
    {
        yield return new WaitForSeconds(1.0f);
        CardsType rule = isNone ? CardsType.None : DeskCardsCache.Instance.Rule;
        int deskWeight = DeskCardsCache.Instance.TotalWeight;
        switch (rule)
        {
            case CardsType.None:
                {
                    List<Card> discard = FirstCard();
                    if (discard.Count != 0)
                    {
                        RemoveCards(discard);
                        DiscardCards(discard, GetSprite(discard));
                    }
                }
                break;
            case CardsType.JokerBoom:
                OrderController.Instance.Turn();
                Debug.Log("pass");
                break;
            case CardsType.Boom:
                {
                    List<Card> discardCards_01 = FindBoom(GetAllCards(), deskWeight, false);
                    if (discardCards_01.Count != 0)
                    {
                        GameController controller = GameObject.Find("GameController").GetComponent<GameController>();
                        //积分翻倍
                        if (discardCards_01.Count == 4)
                        {
                            controller.multiples = 2;
                        }
                        else if (discardCards_01.Count == 2)
                        {
                            controller.multiples = 4;
                        }
                        RemoveCards(discardCards_01);
                        DiscardCards(discardCards_01, GetSprite(discardCards_01));
                    }
                    else
                    {
                        OrderController.Instance.Turn();
                        ShowNotice();
                    }
                }
                break;
            case CardsType.Double:
                {
                    List<Card> discardCards_02 = FindDouble(GetAllCards(), deskWeight, false);
                    if (discardCards_02.Count != 0)
                    {
                        RemoveCards(discardCards_02);
                        DiscardCards(discardCards_02, GetSprite(discardCards_02));
                    }
                    else
                    {
                        OrderController.Instance.Turn();
                        ShowNotice();
                    }
                }
                break;
            case CardsType.Single:
                {
                    List<Card> discardCards_03 = FindSingle(GetAllCards(), deskWeight, false);
                    if (discardCards_03.Count != 0)
                    {
                        RemoveCards(discardCards_03);
                        DiscardCards(discardCards_03, GetSprite(discardCards_03));
                    }
                    else
                    {
                        OrderController.Instance.Turn();
                        ShowNotice();
                    }

                }
                break;
            case CardsType.OnlyThree:
                { 
                   List<Card> discardCards_04=FindOnlyThree(GetAllCards(),deskWeight,false);
                   if (discardCards_04.Count != 0)
                   {
                       RemoveCards(discardCards_04);
                       DiscardCards(discardCards_04, GetSprite(discardCards_04));
                   }
                   else
                   {
                       OrderController.Instance.Turn();
                       ShowNotice();
                   }
                }
                break;
            case CardsType.Straight:
                {
                    List<Card> discardCards_05 = FindStraight(GetAllCards(), DeskCardsCache.Instance.MinWeight, DeskCardsCache.Instance.CardsCount, false);
                    if (discardCards_05.Count != 0)
                    {
                        RemoveCards(discardCards_05);
                        DiscardCards(discardCards_05, GetSprite(discardCards_05));
                    }
                    else
                    {
                        List<Card> boom = FindBoom(GetAllCards(), 0, true);
                        if (boom.Count != 0)
                        {
                            RemoveCards(boom);
                            DiscardCards(boom, GetSprite(boom));
                        }
                        else {
                            OrderController.Instance.Turn();
                            ShowNotice();
                        }
                    }
                }
                break;
            case CardsType.ThreeAndOne:
                {
                    List<Card> discardCards_06 = FindThreeAndOne(GetAllCards(), deskWeight, false);
                    if (discardCards_06.Count != 0)
                    {
                        RemoveCards(discardCards_06);
                        DiscardCards(discardCards_06,GetSprite(discardCards_06));
                    }
                    else
                    {
                        OrderController.Instance.Turn();
                        ShowNotice();
                    }
                }
                break;
            case CardsType.ThreeAndTwo:
                {
                    List<Card> discardCards_07 = FindThreeAndTwo(GetAllCards(), deskWeight, false);
                    if (discardCards_07.Count != 0)
                    {
                        RemoveCards(discardCards_07);
                        DiscardCards(discardCards_07, GetSprite(discardCards_07));
                    }
                    else
                    {
                        OrderController.Instance.Turn();
                        ShowNotice();
                    }
                }
                break;
            case CardsType.DoubleStraight:
                List<Card> discardCards_10 = FindStraight(GetAllCards(), DeskCardsCache.Instance.MinWeight, DeskCardsCache.Instance.CardsCount, false);
                if (discardCards_10.Count != 0)
                {
                    RemoveCards(discardCards_10);
                    DiscardCards(discardCards_10, GetSprite(discardCards_10));
                }
                else
                {
                    List<Card> boom = FindBoom(GetAllCards(), 0, true);
                    if (boom.Count != 0)
                    {
                        RemoveCards(boom);
                        DiscardCards(boom, GetSprite(boom));
                    }
                    else
                    {
                        OrderController.Instance.Turn();
                        ShowNotice();
                    }
                }
                break;
            case CardsType.TripleStraight:
                List<Card> boom_01 = FindBoom(GetAllCards(), 0, true);
                if (boom_01.Count != 0)
                {
                    RemoveCards(boom_01);
                    DiscardCards(boom_01, GetSprite(boom_01));
                }
                else
                {
                    OrderController.Instance.Turn();
                    ShowNotice();
                }
                break;
        }
    }
    public abstract List<Card> FirstCard();
    protected void RemoveCards(List<Card> cards)
    {
        HandCards allCards = gameObject.GetComponent<HandCards>();
        for (int j = 0; j < cards.Count; j++)
        {
            for (int i = 0; i < allCards.CardsCount; i++)
            {
                if (cards[j] == allCards[i])
                {
                    allCards.PopCard(cards[j]);
                    break;
                }
            }
        }
    }
    protected void DiscardCards(List<Card> selectedCardsList, List<CardSprite> selectedSpriteList)
    {
        Card[] selectedCardsArray = selectedCardsList.ToArray();
        //检测是否符合出牌规则
        CardsType type;
        if (CardRules.PopEnable(selectedCardsArray, out type))
        {

            HandCards player = gameObject.GetComponent<HandCards>();
            //如果符合将牌从手牌移到出牌缓存区
            DeskCardsCache.Instance.Clear();
            DeskCardsCache.Instance.Rule = type;

            for (int i = 0; i < selectedSpriteList.Count; i++)
            {
                DeskCardsCache.Instance.AddCard(selectedSpriteList[i].Poker);
                selectedSpriteList[i].transform.parent = GameObject.Find("Desk").transform.Find("StartPoint");
                selectedSpriteList[i].Poker = selectedSpriteList[i].Poker;
            }

            DeskCardsCache.Instance.Sort();
            GameController.AdjustCardSpritsPosition(CharacterType.Desk);
            GameController.AdjustCardSpritsPosition(player.cType);

            GameController.UpdateLeftCardsCount(player.cType, player.CardsCount);

            if (player.CardsCount == 0)
            {
                Debug.Log("Game OVer!");
                //GameObject.Find("GameController").GetComponent<GameController>().GameOver();
            }
            else
            {
                OrderController.Instance.Biggest = player.cType;
                OrderController.Instance.Turn();
            }
        }
    }
    protected List<CardSprite> GetSprite(List<Card> cards)
    {
        HandCards t = gameObject.GetComponent<HandCards>();
        CardSprite[] sprites = GameObject.Find(t.cType.ToString()).transform.Find("StartPoint").GetComponentsInChildren<CardSprite>();

        List<CardSprite> selectedSpriteList = new List<CardSprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            for (int j = 0; j < cards.Count; j++)
            {
                if (cards[j] == sprites[i].Poker)
                {
                    selectedSpriteList.Add(sprites[i]);
                    break;
                }
            }
        }

        return selectedSpriteList;
    }
    protected List<Card> FindBoom(List<Card> allCards, int weight, bool equal)
    {
        List<Card> ret = new List<Card>();
        for(int i=0;i<allCards.Count;i++)
        {
            if (i <= allCards.Count - 4)
            {
                if (allCards[i].GetCardWeight() == allCards[i + 1].GetCardWeight()&&
                    allCards[i].GetCardWeight()==allCards[i+2].GetCardWeight()&&
                    allCards[i].GetCardWeight()==allCards[i+3].GetCardWeight())
                {
                    int totalWeight = (int)allCards[i].GetCardWeight() + (int)allCards[i + 1].GetCardWeight() + (int)allCards[i + 2].GetCardWeight()
                       + (int)allCards[i + 4].GetCardWeight();
                    if (equal)
                    {
                        if (totalWeight >= weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i+1]);
                            ret.Add(allCards[i+2]);
                            ret.Add(allCards[i+3]);
                            break;
                        }
                    }
                    else
                    {
                        if (totalWeight > weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i + 1]);
                            ret.Add(allCards[i + 2]);
                            ret.Add(allCards[i + 3]);
                            break;
                        }
                    }
                }
            }
        }

        if (ret.Count == 0)
        {
            for (int j = 0; j < allCards.Count; j++)
            {
                if (j < allCards.Count - 1)
                {
                    if (allCards[j].GetCardWeight() == Weight.SJoker &&
                        allCards[j + 1].GetCardWeight() == Weight.LJoker)
                    {
                        ret.Add(allCards[j]);
                        ret.Add(allCards[j + 1]);
                    }
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// 找对子，这是强制模式哦，三个的也会拆掉的
    /// </summary>
    /// <param name="allCards"></param>
    /// <param name="weight"></param>
    /// <param name="equal"></param>
    /// <returns></returns>
    protected List<Card> FindDouble(List<Card> allCards, int weight, bool equal)
    {
        List<Card> ret = new List<Card>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (i < allCards.Count - 1)
            {
                if (allCards[i].GetCardWeight() == allCards[i + 1].GetCardWeight()) 
                {
                    int totalWeight = (int)allCards[i].GetCardWeight() + (int)allCards[i + 1].GetCardWeight();
                    if (equal)
                    {
                        if (totalWeight >= weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i + 1]);
                            break;
                        }
                    }
                    else
                    {
                        if (totalWeight > weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i + 1]);
                            break;
                        }
                    }
                }
            }
        }
        return ret;
    }

    protected List<Card> FindSingle(List<Card> allCards, int weight, bool equal)
    {
        List<Card> ret = new List<Card>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (equal)
            {
                if ((int)allCards[i].GetCardWeight() >= weight)
                {
                    ret.Add(allCards[i]);
                    break;
                }
            }
            else
            {
                if ((int)allCards[i].GetCardWeight() > weight)
                {
                    ret.Add(allCards[i]);
                    break;
                }
            }
        }
        return ret;
    }

    protected List<Card> FindOnlyThree(List<Card> allCards, int weight, bool equal)
    {
        List<Card> ret = new List<Card>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (i <= allCards.Count - 3)
            {
                if (allCards[i].GetCardWeight() == allCards[i + 1].GetCardWeight() &&
                    allCards[i].GetCardWeight() == allCards[i + 2].GetCardWeight())
                {
                    int totalWeight = (int)allCards[i].GetCardWeight() +
                          (int)allCards[i + 1].GetCardWeight() +
                          (int)allCards[i + 2].GetCardWeight();

                    if (equal)
                    {
                        if (totalWeight >= weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i + 1]);
                            ret.Add(allCards[i + 2]);
                            break;
                        }
                    }
                    else
                    {
                        if (totalWeight > weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i + 1]);
                            ret.Add(allCards[i + 2]);
                            break;
                        }
                    }
                }
            }
        }
        return ret;
    }
    protected List<Card> FindStraight(List<Card> allCards,int minWeight,int length,bool equal)
    {
        List<Card> ret = new List<Card>();
        int conter = 1;
        List<int> indeies = new List<int>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (i < allCards.Count - 4)
            {
                int weight = (int)allCards[i].GetCardWeight();
                if (equal)
                {
                    if (weight >= minWeight)
                    {
                        conter = 1;
                        indeies.Clear();
                        for (int j = i + 1; j < allCards.Count; j++)
                        {
                            if (allCards[j].GetCardWeight() > Weight.One)
                                break;

                            if ((int)allCards[j].GetCardWeight() - weight == conter)
                            {
                                conter++;
                                indeies.Add(j);
                            }

                            if (conter == length)
                                break;
                        }
                    }
                }
                else
                {
                    if (weight > minWeight)
                    {
                        conter = 1;
                        indeies.Clear();

                        for (int j = i + 1; j < allCards.Count; j++)
                        {
                            if (allCards[j].GetCardWeight() > Weight.One)
                                break;
                            if ((int)allCards[j].GetCardWeight() - weight == conter)
                            {
                                conter++;
                                indeies.Add(j);
                            }

                            if (conter == length)
                                break;
                        }
                    }
                }
            }
            if (conter == length)
            {
                indeies.Insert(0, i);
                break;
            }
        }

        if (conter == length)
        {
            for (int i = 0; i < indeies.Count; i++)
            {
                ret.Add(allCards[indeies[i]]);
            }
        }

       return ret;
    }

    protected List<Card> FindDoubleStraight(List<Card> allCards, int minWeight, int length)
    {
        List<Card> ret = new List<Card>();
        int counter = 0;
        List<int> indeies = new List<int>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (i < allCards.Count - 4)
            {
                int weight = (int)allCards[i].GetCardWeight();

                if (weight > minWeight)
                {
                    counter = 0;
                    indeies.Clear();

                    int circle = 0;
                    for(int j=i+1;j<allCards.Count;j++)
                    {
                        if (allCards[j].GetCardWeight() > Weight.One)
                            break;

                        if ((int)allCards[j].GetCardWeight() - weight == counter)
                        {
                            circle++;
                            if (circle % 2 == 1)
                            {
                                counter++;
                            }
                            indeies.Add(j);
                        }

                        if (counter == length / 2)
                            break;
                    }
                }
            }
            if (counter == length / 2)
            {
                indeies.Insert(0, i);
                break;
            }
        }

        if (counter == length / 2)
        { 
          for(int i=0;i<indeies.Count;i++)
          {
              ret.Add(allCards[indeies[i]]);
          }
        }
        return ret;

    }

    protected List<Card> FindThreeAndTwo(List<Card> allCards, int weight, bool equal)
    {
        List<Card> three = FindOnlyThree(allCards, weight, equal);
        if (three.Count != 0)
        {
            List<Card> leftCards = GetAllCards(three);
            List<Card> two = FindDouble(leftCards, (int)Weight.Three, true);
            three.AddRange(two);
        }
        else
            three.Clear();

        return three;
    }
    protected List<Card> FindThreeAndOne(List<Card> allCards, int weight, bool equal)
    {
        List<Card> three = FindOnlyThree(allCards, weight, equal);
        if (three.Count != 0)
        {
            List<Card> leftCards = GetAllCards(three);
            List<Card> one = FindSingle(leftCards, (int)Weight.Three, true);
            three.AddRange(one);
        }
        else
            three.Clear();

        return three;
    }
    /// <summary>
    /// 获取所有手牌
    /// </summary>
    /// <returns></returns>
    protected List<Card> GetAllCards(List<Card> exclude = null)
    {
        List<Card> cards = new List<Card>();
        HandCards allCards = gameObject.GetComponent<HandCards>();
        bool isContinue = false;
        for (int i = 0; i < allCards.CardsCount; i++)
        {
            isContinue = false;
            if (exclude != null)
            {
                for (int j = 0; j < exclude.Count; j++)
                {
                    if (allCards[i] == exclude[j])
                    {
                        isContinue = true;
                        break;
                    }

                }
            }

            if (!isContinue)
                cards.Add(allCards[i]);
        }
        //从小到大排序
        CardRules.SortCards(cards, true);
        return cards;
    }
    protected void ShowNotice()
    {
        Debug.Log("ShowNotice");
    }
}
