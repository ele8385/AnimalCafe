using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Item
{
    public string name;
    public string type;
    public string info;
    public int price;
    public bool having; //보유&미적용
    public bool applying; //보유&적용

    public int moodClassic = 0; //클래식수치
    public int moodRomantic = 0;  //로맨틱수치
    public int moodTrendy = 0;  //트렌디수치

    public Item() { }

    public Item(string _name, string _type, string _info, int _price, bool _having, bool _applying, int _moodClassic, int _moodRomantic, int _moodTrendy)
    {
        name = _name;
        type = _type;
        info = _info;
        price = _price;
        having = _having;
        applying = _applying;
        moodClassic = _moodClassic;
        moodRomantic = _moodRomantic;
        moodTrendy = _moodTrendy;
    }
}
