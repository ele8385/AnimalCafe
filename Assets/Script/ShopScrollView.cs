using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform content;
    public int row;
    public int column;
    
    public void ItemsSet()
    {
        column = 3; //한 줄당 유닛 개수
        row = content.childCount;
        ViewScaleSet();
        for (int j = 0; j < row; j++)
        {
            for (int i = 0; i < column; i++)
            {
                int index = (j * column) + i - 1;
                ItemUnit itemScript = content.GetChild(j).transform.GetChild(i).gameObject.GetComponent<ItemUnit>();
                itemScript.ItemSet();
            }
        }
    }
    public void ItemsBtnSet()
    {
        column = 3; //한 줄당 유닛 개수
        row = content.childCount;

        for (int j = 0; j < row; j++)
        {
            for (int i = 0; i < column; i++)
            {
                int index = (j * column) + i - 1;
                ItemUnit itemScript = content.GetChild(j).transform.GetChild(i).gameObject.GetComponent<ItemUnit>();
                itemScript.applying = false;
                itemScript.ItemBtnSet();
            }
        }
    }

    //콘텐츠 뷰포트 사이즈 조정
    public void ViewScaleSet()
    {
        RectTransform rectTransform = content.transform.parent.gameObject.GetComponent<RectTransform>();
        float scrollW = rectTransform.rect.width;           //스크롤뷰 사이즈 조정
        float scrollH = row * 440f;   //스크롤뷰 사이즈 조정
        rectTransform.sizeDelta = new Vector2(scrollW, scrollH);

        rectTransform.offsetMin = new Vector2(0, rectTransform.offsetMin.y);
        rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y);
        rectTransform.anchoredPosition = new Vector2(0, 0);
    }
}
