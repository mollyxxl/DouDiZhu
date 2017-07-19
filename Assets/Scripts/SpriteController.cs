using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController
{
    private Dictionary<string, Sprite> sprites;
    private static SpriteController instance;
    public static SpriteController Instance {
        get
        {
            if (instance == null)
                instance = new SpriteController();
            return instance;
        }
    }
    public Dictionary<string, Sprite> Sprites 
    {
        get 
        {
            return sprites;  
        }
    }
    private SpriteController()
    {
        if (sprites == null)
        {
            var temp = Resources.LoadAll<Sprite>("PokerTex");
            sprites = new Dictionary<string, Sprite>();
            foreach (var te in temp)
            {
                sprites.Add(te.name, te);
            }
        }
    }
}
