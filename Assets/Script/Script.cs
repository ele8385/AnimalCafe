using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//인스펙터창에서 수정가능
[System.Serializable]
public class Script
{
    [Tooltip("대사 번호")]
    public int num;

    [Tooltip("대사 내용")]
    public string line;

    [Tooltip("대답 내용")]
    public List<Answer> answers;


    public Script(int _num, string _line, List<Answer> _answers)
    {
        num = _num;
        line = _line;
        answers = _answers;
    }

    public Script() { }
    
}
