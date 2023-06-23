using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ItemUnit : MonoBehaviour
{
    public int code;
    public ItemData itemData;

    public Text itemNameText;
    public Text itemPriceText;
    public Image spriteRenderer;
    public PopUpManager popUp;
    public GameObject LinkObject;
    public GameObject BuyBtn;
    public GameObject HaveBtn;
    public GameObject ApplyBtn;

    //아이템 셋팅
    public void ItemSet(ItemData _itemData)
    {
        itemData = _itemData;

        code = itemData.code;
        itemNameText.text = itemData.name;
        itemPriceText.text = string.Format("{0:#,0}", itemData.price);
        spriteRenderer.sprite = itemData.thumNail;
        ItemBtnSet();
        popUp = GameObject.Find("Canvas").transform.Find("PopUp").gameObject.GetComponent<PopUpManager>();
    }

    //버튼 해당되는 거 켬
    public void ItemBtnSet()
    {
        Item item = State.instance.GetMyItem(itemData.code);
        
        if (item != null)          //보유 중 아이템 = 적용하기 버튼 활성화
        { 
            if (item.apply)            //적용 중 아이템 = 적용 중 버튼 활성화
            {
                BuyBtn.SetActive(false);
                HaveBtn.SetActive(true);
                ApplyBtn.SetActive(false);

            }
            else
            {
                BuyBtn.SetActive(false);
                HaveBtn.SetActive(false);
                ApplyBtn.SetActive(true);
            }
        }
        else                         //구매하기 버튼 활성화
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
        if(State.instance.AddInteriorItem(itemData)) //아이템 추가하고 성공true반환하면 적용실행
        ApplyItem();
    }

    public void ClickApplyBtn()
    {
        popUp.OpenSelectPopUp("해당 아이템을 적용할까요?", ApplyItem); //확인 누르면 BuyItem()실행
    }

    public void ApplyItem()
    {
        State.instance.ApplyInteriorItem(itemData);

        //모든 아이템 정보 업데이트하고 창 끄기
        GameObject ScrollView = transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.gameObject;
        ScrollView.transform.parent.transform.parent.gameObject.SetActive(false);
        GameObject.Find("Canvas").GetComponent<TouchLock>().SetOff();
    }
}
