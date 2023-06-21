using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class AnimalData
{
    public int code;       //코드
    public string name;        //이름
    public AnimalType type;    //종족
    public string info;       //설명    
    public string hiddenInfo;  //히든설명
    public string hello;  //인삿말
    public string script_good;  //맛있을 때 대사
    public string script_bad;  //맛없을 때 대사
    public Recipe recipe;       //음료
    public int moneyCondition; //등장조건_돈
    public MoodType moodType;  //등장조건_분위기
    public int moodCondition;  //등장조건_분위기수치
    public int temperatureCondition;

    //Animal클래스스크립트를 Database에서 AnimalsJson데이터로 불러오기
    
    public AnimalData(int _code, string _name, AnimalType _type, string _info, string _hiddenInfo, string _hello, string _script_good, string _script_bad, Recipe _recipe, int _moneyCondition, MoodType _moodType, 
                int _moodCondition, int _temperatureCondition)
    {
        code = _code;
        name = _name;
        type = _type;
        info = _info;
        hiddenInfo = _hiddenInfo;
        hello = _hello;
        script_good = _script_good;
        script_bad = _script_bad;
        recipe = _recipe;
        moneyCondition = _moneyCondition;
        moodType = _moodType;
        moodCondition = _moodCondition;
        temperatureCondition = _temperatureCondition;
    }

    public AnimalData()
    { //AnimalData 초기화 시 사용
    }
}
public enum AnimalType
{  //Hair=0; Top=1; Bottom=2;로 자동 할당
    Rabbit,
    Dog,
    Cat
}
public enum MoodType
{  //Hair=0; Top=1; Bottom=2;로 자동 할당
    Classic,
    Romantic,
    Trendy,
    Business
}