using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Item
{
    public int code;
    public string name;
    public bool have; //보유&미적용
    public bool apply; //보유&미적용

    public Item() { }

    public Item(int _code)
    {
        code = _code;
        name = Database.instance.items[code].name;
        have = false;
        apply = false;
    }
}

