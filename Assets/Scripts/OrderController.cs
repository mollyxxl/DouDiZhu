using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CardEvent(bool arg);
public class OrderController 
{
    private CharacterType biggest; //最大出牌者
    private CharacterType currentAuthority;//当前出牌者
    private static OrderController instance;
    public event CardEvent smartCard;
    public event CardEvent activeButton;
    public static OrderController Instance
    {
        get {
            if (instance == null)
                instance = new OrderController();
            return instance;
        }
    }
    public CharacterType Type {
        get { return currentAuthority; }
    }
    public CharacterType Biggest
    {
        set { biggest = value; }
        get { return biggest; }
    }
    private OrderController()
    {
        currentAuthority = CharacterType.Desk;
    }
    public void Init(CharacterType type)
    {
        currentAuthority = type;
        Biggest = type;
        if (currentAuthority == CharacterType.Player)
        {
            //玩家
            activeButton(false);
        }
        else 
        { 
          //电脑
            smartCard(true);
        }
    }
    public void Turn()
    {
        currentAuthority += 1;
        if(currentAuthority== CharacterType.Desk)
        {
          currentAuthority= CharacterType.Player;
        }

        if (currentAuthority == CharacterType.ComputerOne ||
            currentAuthority == CharacterType.ComputerTwo)
        {
            smartCard(biggest == currentAuthority);
        }
        else if (currentAuthority == CharacterType.Player)
        {
            activeButton(biggest != currentAuthority);
        }
    }
}
