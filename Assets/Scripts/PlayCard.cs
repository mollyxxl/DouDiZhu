using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCard : MonoBehaviour {

    public bool CheckSelectCards()
    {
        CardSprite[] sprites = this.GetComponentsInChildren<CardSprite>();
        // 找出所有选中的
        List<Card> selectedCardsList = new List<Card>();
        List<CardSprite> selectedSpriteList = new List<CardSprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].Select)
            {
                selectedSpriteList.Add(sprites[i]);
                selectedCardsList.Add(sprites[i].Poker);
            }
        }

        //排序
        CardRules.SortCards(selectedCardsList, true);

        return CheckPlayCards(selectedCardsList, selectedSpriteList);

    }
    private bool CheckPlayCards(List<Card> selectedCardsList, List<CardSprite> selectedSpriteList)
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        Card[] selectedCardsArray = selectedCardsList.ToArray();
        CardsType type;
        if (CardRules.PopEnable(selectedCardsArray, out type))
        {
            CardsType rule = DeskCardsCache.Instance.Rule;
            if (OrderController.Instance.Biggest == OrderController.Instance.Type)
            {
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            else if (DeskCardsCache.Instance.Rule == CardsType.None)
            {
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            //炸弹
            else if (type == CardsType.Boom && rule != CardsType.Boom)
            {
                gameController.multiples = 2;
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            else if (type == CardsType.JokerBoom)
            {
                gameController.multiples = 4;
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            else if (type == CardsType.Boom && rule == CardsType.Boom &&
               GameController.GetWeight(selectedCardsArray, type) > DeskCardsCache.Instance.TotalWeight)
            {
                gameController.multiples = 2;
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            else if (GameController.GetWeight(selectedCardsArray, type) > DeskCardsCache.Instance.TotalWeight)
            {
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            return true;
        }
        return false;
    }

    private void PlayCards(List<Card> selectedCardsList, List<CardSprite> selectedSpriteList, CardsType type)
    {
        HandCards player = GameObject.Find("Player").GetComponent<HandCards>();
        DeskCardsCache.Instance.Clear();
        DeskCardsCache.Instance.Rule = type;

        for (int i = 0; i < selectedSpriteList.Count; i++)
        { 
           //先进行卡牌移动
            player.PopCard(selectedSpriteList[i].Poker);
            DeskCardsCache.Instance.AddCard(selectedSpriteList[i].Poker);
            selectedSpriteList[i].transform.parent = GameObject.Find("Desk").transform;
        }

        DeskCardsCache.Instance.Sort();
        GameController.AdjustCardSpritsPosition(CharacterType.Desk);
        GameController.AdjustCardSpritsPosition(CharacterType.Player);
        GameController.UpdateLeftCardsCount(CharacterType.Player, player.CardsCount);

        if (player.CardsCount == 0)
        {
            Debug.Log("Game OVer");
        }
        else
        {
            OrderController.Instance.Biggest = CharacterType.Player;
            OrderController.Instance.Turn();
        }
    }
}
