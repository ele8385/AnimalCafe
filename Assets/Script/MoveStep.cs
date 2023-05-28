using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStep : MonoBehaviour {
    public static MoveStep instance;
    public string[] stepName;
    public List<string> Step;
    private void Awake()
    {
        instance = this;
        
        Step = new List<string>();
        stepName = new string[] {   "리셋",        //0
                                    "입장시작",    //1
                                    "입장중",      //2
                                    "문옆이동",    //3
                                    "이미지조절",  //4
                                    "카운터옆이동",//5
                                    "카운터설정",  //6
                                    "카운터이동",  //7
                                    "카운터멈춤",  //8
                                    "카운터대기",  //9
                                    "음료평가",    //10
                                    "카운터종료",  //11
                                    "카운터탈출",  //12
                                    "중앙으로",    //13
                                    "경로설정",    //14
                                    "테이블이동",  //15
                                    "테이블멈춤",  //16
                                    "테이블앉음",  //17
                                    "테이블비움",  //18
                                    "문이동",      //19
                                    "퇴장시작",    //20
                                    "퇴장중"       //21
        };
        foreach (string arrItem in stepName)
        {
            Step.Add(arrItem);
        }


    }
    public int Set(string stepName)
    {
        return Step.IndexOf(stepName);
    }
}
