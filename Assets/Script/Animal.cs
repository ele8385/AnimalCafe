using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Animal
{
    public int code;  //동물코드
    public string name;  //동물이름
    public int heart; //호감도
    public int orderCount; //주문수 //주문 시작하자마자 카운팅
    public int buyCount; //판매수 //판매 성공했을 때 카운팅
    public int dialNum; //대화순서

    public Animal(int _code)
    {
        code = _code;
        name = Database.instance.animals[code].name;
        heart = 0;
        orderCount = 0;
        buyCount = 0;
        dialNum = 0;
    }
    public Animal() { }
}
