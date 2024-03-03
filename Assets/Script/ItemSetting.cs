using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviour
{
    public SpriteRenderer Wallpaper, WallBand, WallBand2, Floor;
    public SpriteRenderer Table0, Table1, Table2, Table3, Table4, Table5;

    public List<string> mood;
    
    // Start is called before the first frame update
    void Start()
    {
        ItemObjectSet();

    }
    public void ItemObjectSet()
    {
        mood = new List<string>();
        ItemObjectSet("Wallpaper");
        ItemObjectSet("Floor");

    }

    public void ItemObjectSet(string type)
    {
        if (! State.instance.myState.applyItem.ContainsKey(type))
        {
            Debug.Log(1); return;
        }

        ItemData itemData = State.instance.myState.applyItem[type]; //해당 타입의 적용아이템 딕셔너리 값인 아이템데이터를 가져옴
        if (itemData == null) { Debug.Log(0); return; }
        mood.AddRange(itemData.mood);

        //스프라이트 교체
        string path = itemData.category + "/" + itemData.type + "/" + itemData.category + "_" + itemData.type + "_" + itemData.name;
        if (itemData.type == "Wallpaper") Wallpaper.sprite = Resources.Load<Sprite>(path);
        else if (itemData.type == "Floor") Floor.sprite = Resources.Load<Sprite>(path);
    }
}
