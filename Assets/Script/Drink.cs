using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Drink
{
    public int cupNum;      //컵 넘버
    public List<Color> colors; //음료 색상
    public List<int> gradient;//그라디언트 여부
    public string whipping;
    public string syrup;
    public string topping;
    
    public Drink()
    {

    }

    public Drink(int _cupNum, List<Color> _colors, List<int> _gradient, string _whipping, string _syrup, string _topping)
    {
        cupNum = _cupNum;
        colors = _colors;
        gradient = _gradient;
        whipping = _whipping;
        syrup = _syrup;
        topping = _topping;
    }
}
