using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviour
{
    public SpriteRenderer Wallpaper, WallBand, WallBand2, Floor;
    public SpriteRenderer Table0, Table1, Table2, Table3, Table4, Table5; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void ItemSet(Item _wallpaper, Item _wallband)
    {/*
        Wallpaper.sprite = Resources.Load<Sprite>(_wallpaper.name);
        if (Wallpaper.sprite.name == "Counter_Wall") Wallpaper.color = _wallpaper.color;
        WallBand.sprite = Resources.Load<Sprite>(_wallband.name);*/
    }
}
