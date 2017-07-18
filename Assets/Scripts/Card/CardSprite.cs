using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSprite : MonoBehaviour {

    public Card card;
    private Sprite sprite;
    private bool isSelected;
    private Dictionary<string,Sprite> sprites;

	void Start () {
        var temp = Resources.LoadAll<Sprite>("Poker");
        sprites = new Dictionary<string, Sprite>();
        foreach (var te in temp)
        {
            sprites.Add(te.name, te);
        }
        sprite = gameObject.GetComponent<Image>().sprite;
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
            sprite = sprites[card.GetCardName()];
        }
        else
        {
            sprite = sprites["SmallCardBack1"];
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
        
    }
    public void PokerClick()
    {
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
