using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public Text text;
    public GameObject PopUp_OK;
    public GameObject PopUp_Select;
    public GameObject PopUp_Item;
    public delegate void Func();
    public PopUpManager_Item popUpManager_Item;
    public Func func;

    //확인 팝업(확인 텍스트 바꿈)
    public void OpenOkPopUp(string messege, string buttonMessege)
    {
        PopUp_OK.transform.Find("Window").transform.Find("Button_OK").transform.Find("Text").gameObject.GetComponent<Text>().text = buttonMessege;
        OpenOkPopUp(messege);
    }

    //확인 팝업
    public void OpenOkPopUp(string messege)
    {
        PopUp_OK.SetActive(true);
        PopUp_OK.transform.Find("Window").transform.Find("Messege").gameObject.GetComponent<Text>().text = messege;
    }

    //아이템 정보 팝업
    public void OpenItemPopUp(ItemData itemData, Func _func)
    {

        PopUp_Item.SetActive(true);
        popUpManager_Item.OpenItemPopUp(itemData);
        func = _func;
    }

    public void ClickItemBtn()
    {
        func();
        PopUp_Item.SetActive(false);
    }

    //선택지 2개 팝업(예/아니오 텍스트 바꿈)
    public void OpenSelectPopUp(string messege, Func _func, string left, string right)
    {
        transform.Find("Button_Left").gameObject.GetComponent<Text>().text = left;
        transform.Find("Button_Right").gameObject.GetComponent<Text>().text = right;
        OpenSelectPopUp(messege, _func);
    }

    //선택지 2개 팝업
    public void OpenSelectPopUp(string messege, Func _func )
    {
        PopUp_Select.SetActive(true);
        transform.Find("Messege").gameObject.GetComponent<Text>().text = messege;
        func = _func;
    }


    //예 누르면 함수 실행, 아니요 누르면 취소
    public void ReturnSelect(bool _select)
    {
        gameObject.SetActive(false);
        if (_select) func(); 
    }
}
