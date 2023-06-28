using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager_Item : MonoBehaviour
{
    public Image ItemImage;
    public Text ItemName;
    public Text ItemInfo;
    public Text PriceText;
    public GameObject ButtonBuy;
    public GameObject ButtonApply;
    public GameObject ButtonApplying;

    //아이템 정보 팝업
    public void OpenItemPopUp(ItemData itemData)
    {
        Item item = State.instance.GetMyItem(itemData.code);

        ItemImage.sprite = itemData.thumNail;
        ItemName.text = itemData.name;
        ItemInfo.text = itemData.info;
        PriceText.text = string.Format("{0:#,0}", itemData.price);
        
        ButtonBuy.SetActive(false);
        ButtonApply.SetActive(false);
        ButtonApplying.SetActive(false);

        if (item == null) ButtonBuy.SetActive(true);       //미보유 아이템
        else if (item.apply == true) ButtonApply.SetActive(true);       //적용 중인 보유 아이템
        else ButtonApplying.SetActive(true);    //보유 아이템
        
    }
}
