using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Answer
{
    [Tooltip("선택지")]
    public string select;

    [Tooltip("대답 내용")]
    public string text;

    [Tooltip("다음 대사 번호")]
    public int nextLine;

    [Tooltip("호감도 증가치")]
    public int heart;

    public Answer(string _select, string _text, int _nextLine, int _heart)
    {
        select = _select;
        text = _text;
        nextLine = _nextLine;
        heart = _heart;
    }

    public Answer() { }
}
