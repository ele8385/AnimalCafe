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
    public Func func;

    //확인 팝업(확인 텍스트 바꿈)
    public void OpenOkPopUp(string messege, string buttonMessege)
    {
        transform.Find("Button_OK").gameObject.GetComponent<Text>().text = buttonMessege;
        OpenOkPopUp(messege);
    }

    //확인 팝업
    public void OpenOkPopUp(string messege)
    {
        PopUp_OK.SetActive(true);
        transform.Find("Messege").gameObject.GetComponent<Text>().text = messege;
    }

    //아이템 정보 팝업
    public void OpenItemPopUp(ItemData itemData, Func _func)
    {

        PopUp_Item.SetActive(true);

        Item item = State.instance.GetMyItem(itemData.code);
        Debug.Log(0);

        Debug.Log(PopUp_Item.transform.Find("ItemImage").gameObject.name);

        PopUp_Item.transform.Find("ItemImage").GetComponent<Image>().sprite = itemData.thumNail;
        PopUp_Item.transform.Find("ItemName").GetComponent<Text>().text = itemData.name;
        PopUp_Item.transform.Find("ItemInfo").GetComponent<Text>().text = itemData.info;
        PopUp_Item.transform.Find("ButtonText").GetComponent<Text>().text = string.Format("{0:#,0}", itemData.price);

        GameObject BtnPrice = PopUp_Item.transform.Find("ButtonBuy").gameObject;
        GameObject BtnApply = PopUp_Item.transform.Find("ButtonApply").gameObject;
        GameObject BtnApplying = PopUp_Item.transform.Find("Button_Applying").gameObject;

        BtnPrice.SetActive(false);
        BtnApply.SetActive(false);
        BtnApplying.SetActive(false);
       
        if      (item == null)          BtnPrice.SetActive(true);       //미보유 아이템
        else if (item.apply == true)    BtnApply.SetActive(true);       //적용 중인 보유 아이템
        else                            BtnApplying.SetActive(true);    //보유 아이템

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
