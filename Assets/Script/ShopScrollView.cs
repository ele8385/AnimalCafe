using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform content;
    public GameObject ItemUnit;
    public string category;
    public int row;
    public int column;//한 줄당 유닛 개수
    public List<ItemData> itemDatas;

    public void OpenScrollView()
    {
        gameObject.SetActive(true);
        SetItemUnit();
    }

    public void CloseScrollView()
    {
        gameObject.SetActive(false);
    }

    public void ClickCategory(string _category)
    {
        category = _category;
        SetItemUnit();
    }

    public void DestroyItemUnit()
    {
        foreach (Transform i in content.transform)
        {
            Destroy(i.gameObject);
        }
    }

    public void SetItemUnit()
    {
        DestroyItemUnit();

        itemDatas = new List<ItemData>();
        
        foreach (ItemData i in Database.instance.items)
        {
            if (i.category == category)
                itemDatas.Add(i);
        }
        
        row = (int)Mathf.Ceil(itemDatas.Count / (float)column);

        int r = 0;
        GameObject Horizontal = null;
        foreach (ItemData i in itemDatas)
        {

            if (r % 3 == 0)
            {
                Horizontal = new GameObject("Horizontal");
                Horizontal.transform.SetParent(content.transform);

                HorizontalLayoutGroup horizontalLayoutGroup = Horizontal.AddComponent<HorizontalLayoutGroup>();
                horizontalLayoutGroup.childControlHeight = false;
                horizontalLayoutGroup.childControlWidth = false;
                horizontalLayoutGroup.childForceExpandHeight = true;
                horizontalLayoutGroup.childForceExpandWidth = true;
                horizontalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
                Horizontal.GetComponent<RectTransform>().localScale = Vector3.one;
            }
            
            GameObject itemPrefab = Instantiate(ItemUnit, Horizontal.transform);
            itemPrefab.gameObject.GetComponent<ItemUnit>().ItemSet(i);

            r++;
        }
    }

    public void AddHorizontal()
    {

    }
    /*
    public void ItemsSet()
    {
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
        row = content.childCount;

        for (int j = 0; j < row; j++)
        {
            for (int i = 0; i < column; i++)
            {
                int index = (j * column) + i - 1;
                ItemUnit itemScript = content.GetChild(j).transform.GetChild(i).gameObject.GetComponent<ItemUnit>();
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
    }*/
}
