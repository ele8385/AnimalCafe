using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ItemUnit : MonoBehaviour
{
    public string type = "";
    public string info = "";
    public bool having; //보유&미적용
    public bool applying; //보유&적용

    public int moodClassic = 0; //클래식수치
    public int moodRomantic = 0;  //로맨틱수치
    public int moodTrendy = 0;  //트렌디수치
    
    public Item item = new Item();

    public PopUpManager popUp;
    public GameObject LinkObject;
    private GameObject BuyBtn;
    private GameObject HaveBtn;
    private GameObject ApplyBtn;

    //초기 아이템버튼 셋팅
    public void ItemSet()
    {
        BuyBtn = transform.Find("Button_구입하기").gameObject;
        HaveBtn = transform.Find("Button_적용중").gameObject;
        ApplyBtn = transform.Find("Button_적용하기").gameObject;
        popUp = GameObject.Find("Canvas").transform.Find("PopUp").gameObject.GetComponent<PopUpManager>();

        //인스펙터창에서 입력한 아이템정보 집어넣음
        string itemName = transform.Find("ItemName").gameObject.GetComponent<Text>().text;
        string priceText = BuyBtn.transform.Find("Price").gameObject.GetComponent<Text>().text;
        string tempPrice = Regex.Replace(priceText, @"[^0-9]", ""); //가격에서 콤마 뺀 문자열
        int price = int.Parse(tempPrice); // 가격문자열 정수로 변환

        item = new Item(itemName, type, info, price, having, applying, moodClassic, moodRomantic, moodTrendy);
    }

    //버튼 해당되는 거 켬
    public void ItemBtnSet()
    {
        //아이템 보유 및 적용 유무확인
        if (State.instance.CheckHaveItem(item.name)) item.having = true; else item.having = false;
        if (State.instance.CheckApplyItem(item.name)) item.applying = true; else item.applying = false;

        if (item.applying) //적용중버튼
        {
            BuyBtn.SetActive(false);
            HaveBtn.SetActive(true);
            ApplyBtn.SetActive(false);

            LinkObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Item/" + item.name);
        }
        else if (item.having) //적용하기버튼
        {
            BuyBtn.SetActive(false);
            HaveBtn.SetActive(false);
            ApplyBtn.SetActive(true);
        }
        else
        {
            BuyBtn.SetActive(true);
            HaveBtn.SetActive(false);
            ApplyBtn.SetActive(false);
        }
    }

    public void ClickBuyBtn()
    {
        popUp.OpenSelectPopUp("해당 아이템을 구입할까요?", BuyItem); //확인 누르면 BuyItem()실행
    }

    public void BuyItem()
    {
        if(State.instance.AddInteriorItem(item)) //아이템 추가하고 성공true반환하면 적용실행
        ApplyItem();
    }

    public void ClickApplyBtn()
    {
        popUp.OpenSelectPopUp("해당 아이템을 적용할까요?", ApplyItem); //확인 누르면 BuyItem()실행
    }

    public void ApplyItem()
    {
        State.instance.ApplyInteriorItem(item);

        //모든 아이템 정보 업데이트하고 창 끄기
        GameObject ScrollView = transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.gameObject;
        ScrollView.GetComponent<ShopScrollView>().ItemsBtnSet();
        ScrollView.transform.parent.transform.parent.gameObject.SetActive(false);
        GameObject.Find("Canvas").GetComponent<TouchLock>().SetOff();
    }
}
