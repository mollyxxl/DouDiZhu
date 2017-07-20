using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public int basePointPerMatch; //底分
    public int multiples;//全场倍数
    public int MatchType = 0; //0 :Easy  1:Normal

    public Button btn_FaPai;
    public Button btn_bujiao;
    public Button btn_jiao;
    public Button btn_Play;
    public Button btn_DisPlay;
    
	// Use this for initialization
	void Start () {
        multiples = 1;
        basePointPerMatch = 100;
        //根据类型加载


        OrderController.Instance.activeButton += Btn_activeButton;
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
 

    public static void AdjustCardSpritsPosition(CharacterType type)
    {
        Debug.Log("调整位置");

        if (type == CharacterType.Desk)
        {
            GameObject parent = GameObject.Find("Canvas/" + type.ToString() + "/StartPoint");
            CardSprite[] cs = parent.GetComponentsInChildren<CardSprite>();
            for (int i = 0; i < cs.Length; i++)
            {
                for (int j = 0; j < cs.Length; j++)
                {
                    if (cs[j].Poker == DeskCardsCache.Instance[i])
                    {
                        cs[j].GoToPosition(parent, i);
                    }
                }
            }
        }
        else
        {
            HandCards hc = GameObject.Find(type.ToString()).GetComponent<HandCards>();
            var parent = GameObject.Find("Canvas/" + type.ToString() + "/StartPoint");
            CardSprite[] cs = parent.GetComponentsInChildren<CardSprite>();
            for (int i = 0; i < hc.CardsCount; i++)
            {
                for (int j = 0; j < cs.Length; j++)
                {
                    if (cs[j].Poker == hc[i])
                    {
                        cs[j].GoToPosition(parent, i);
                    }
                }
            }

        }

    }

    public static  void UpdateLeftCardsCount(CharacterType characterType, int p)
    {
        Debug.Log("显示剩余");
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
            var poker = Resources.Load("Poker") as GameObject;
            var obj = GameObject.Instantiate(poker);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.SetParent(GameObject.Find(type.ToString()).transform.Find("StartPoint").transform, false);  //需要设置相对坐标系，否则这句放到前面写去
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
    /// <summary>
    /// 发牌按钮点击事件
    /// </summary>
    public void Btn_FaPai_Click()
    {
        DealCards();
        btn_FaPai.gameObject.SetActive(false);
        btn_bujiao.gameObject.SetActive(true);
        btn_jiao.gameObject.SetActive(true);
    }
    /// <summary>
    /// 抢
    /// </summary>
    public void Btn_Jiaodizhu_Click()
    {
        CardsOnTable(CharacterType.Player);
        OrderController.Instance.Init(CharacterType.Player);
        btn_jiao.gameObject.SetActive(false);
        btn_bujiao.gameObject.SetActive(false);
    }
    /// <summary>
    /// 不抢
    /// </summary>
    public void Btn_Bujiao_Click()
    {
        int index = Random.Range(2, 4);
        CardsOnTable((CharacterType)index);
        OrderController.Instance.Init((CharacterType)index);
        btn_jiao.gameObject.SetActive(false);
        btn_bujiao.gameObject.SetActive(false);
    }
    void Btn_activeButton(bool arg)
    {
        btn_Play.gameObject.SetActive(true);
        btn_DisPlay.gameObject.SetActive(true);
        btn_DisPlay.enabled = arg;
    }
    public void Btn_Play_Click()
    {
        PlayCard playCard = GameObject.Find("Player").GetComponent<PlayCard>();
        if (playCard.CheckSelectCards())
        {
            btn_Play.gameObject.SetActive(false);
            btn_DisPlay.gameObject.SetActive(false);
        }
    }
    public void Btn_DisPlay_Click()
    {
        OrderController.Instance.Turn();
        btn_Play.gameObject.SetActive(false);
        btn_DisPlay.gameObject.SetActive(false);
    }
}
