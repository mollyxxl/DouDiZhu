using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController 
{
    private CharacterType biggest;
    private CharacterType currentAuthority;
    private static OrderController instance;
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
        }
        else 
        { 
          //电脑
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
            
        }
        else if (currentAuthority == CharacterType.Player)
        { 
          
        }
    }
}
