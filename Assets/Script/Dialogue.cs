using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//인스펙터창에서 수정가능
[System.Serializable]
public class Dialogue
{
    [Tooltip("캐릭터 이름")]
    public string name;

    [Tooltip("대화")]
    public List<Script> scripts;


    public Dialogue(string _name, List<Script> _scripts)
    {
        name = _name;
        scripts = _scripts;
    }

    public Dialogue() { }
}
