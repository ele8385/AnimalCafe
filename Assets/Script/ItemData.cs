using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int code;
    public string name;
    public string category;
    public string type;
    public string info;
    public int price;
    public Color color;
    public List<string> mood;
    public Sprite thumNail;

    public ItemData(int _code, string _name, string _category, string _type, string _info, int _price, List<string> _mood)
    {
        code = _code;
        name = _name;
        category = _category;
        type = _type;
        info = _info;
        price = _price;
        mood = _mood;
        thumNail = Resources.Load<Sprite>("Item/Interior/" + name);
        color = Color.white;
    }
}
