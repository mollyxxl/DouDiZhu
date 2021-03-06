﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSprite : MonoBehaviour {

    public Card card;
    private bool isSelected;
   
	void Start () {
        var btn = gameObject.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(PokerClick);
        }
    }

    public Card Poker
    {
        set {
            card = value;
            card.isSprite = true;
            SetSprite();
        }
        get { return card; }
    }
    /// <summary>
    /// 是否被点击中
    /// </summary>
    public bool Select
    {
        set { isSelected = value; }
        get { return isSelected; }
    }
    void SetSprite()
    {
        if (card.Attribution == CharacterType.Player || card.Attribution == CharacterType.Desk)
        {
            try
            {
                gameObject.GetComponent<Image>().sprite = SpriteController.Instance.Sprites[card.GetCardName()];
            }
            catch (System.Exception e)
            { 
             
            }
           
        }
        else
        {
            try
            {
                gameObject.GetComponent<Image>().sprite = SpriteController.Instance.Sprites["SmallCardBack1"];
            }
            catch (System.Exception e)
            {

            }
            
        }
    }
    /// <summary>
    /// 销毁精灵
    /// </summary>
    public void Destroy()
    {
        //精灵化false
        card.isSprite = false;
        //销毁对象
        Destroy(this.gameObject);
    }
    /// <summary>
    /// 调整位置
    /// </summary>
    public void GoToPosition(GameObject parent, int index)
    {
        gameObject.transform.SetSiblingIndex(index);
        if (card.Attribution == CharacterType.Player)
        {
            transform.localPosition =  Vector3.right * 25 * index;
            if (isSelected)
            {
                transform.localPosition += Vector3.up * 10;
            }
          //  Debug.Log("localPosition" + transform.localPosition);
        }
        else if(card.Attribution== CharacterType.ComputerOne||
            card.Attribution== CharacterType.ComputerTwo)
        {
            transform.localPosition =  Vector3.up * -25 * index;
        }
        else if (card.Attribution == CharacterType.Desk)
        {
            transform.localPosition =  Vector3.right * 25 * index;
        }
    }
    /// <summary>
    /// 卡牌点击效果
    /// </summary>
    public void PokerClick()
    {
        Debug.Log("点击卡牌" + card.GetCardName());
        if (card.Attribution == CharacterType.Player)
        {
            if (isSelected)
            {
                transform.localPosition -= Vector3.up * 10;
                isSelected = false;
            }
            else
            {
                transform.localPosition += Vector3.up * 10;
                isSelected = true;
            }
        }
    }
}
