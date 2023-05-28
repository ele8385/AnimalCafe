using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public Text text;
    public GameObject Btn_OK;
    public GameObject Btn_Sel;
    public delegate void Func();
    public Func func;

    public void OpenOkPopUp(string messege)
    {
        gameObject.SetActive(true);
        text.text = messege;
        Btn_Sel.SetActive(false);
        Btn_OK.SetActive(true);
    }

    public void OpenSelectPopUp(string messege, Func _func )
    {
        gameObject.SetActive(true);
        text.text = messege;
        func = _func;
        Btn_Sel.SetActive(true);
        Btn_OK.SetActive(false);
    }

    //예 누르면 함수 실행, 아니요 누르면 취소
    public void ReturnSelect(bool _select)
    {
        gameObject.SetActive(false);
        if (_select) func(); 
    }
}
