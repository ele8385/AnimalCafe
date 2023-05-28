using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject prefab;
    public Transform content;
    public int row;
    public int column;
    public bool setting = false;
    string scrollViewName;

    public void Open()
    {
        gameObject.transform.parent.transform.parent.gameObject.SetActive(true);
        scrollRect = GetComponent<ScrollRect>();
        scrollViewName = scrollRect.gameObject.name;
        content = transform.Find("Viewport").transform.Find("Content").transform.Find("Vertical").gameObject.transform;
        prefab = Resources.Load("Prefabs/" + scrollViewName + "Unit") as GameObject;
        column = (int)Mathf.Ceil(Database.instance.animals.Count / 3f); //한 줄당 유닛 개수
        row = 3;

        ViewScaleSet();

        if (!setting)
        {
            for (int j = 0; j < row; j++)
            {
                for (int i = 0; i < column; i++)
                {
                    int index = (j * column) + i;

                    GameObject Unit = Instantiate<GameObject>(prefab, content.GetChild(j));
                    Unit.transform.Find("Name").GetComponent<Text>().text = "???";
                    try
                    {
                        Sprite[] sprites = Resources.LoadAll<Sprite>("Character/" + Database.instance.animals[index].name);
                        Unit.transform.Find("Thumnail").transform.Find("Img").GetComponent<Image>().sprite = sprites[0];
                        Unit.transform.Find("Thumnail").transform.Find("Shadow").GetComponent<Image>().sprite = sprites[0];
                    }
                    catch{ Debug.Log("동물 이미지 연결 오류"); }
                }
            }
        }
        UnitCheck();
        setting = true;
    }
    public void ViewScaleSet()
    {
        RectTransform rectTransform = content.transform.parent.gameObject.GetComponent<RectTransform>();
        float scrollW = column * 300f;                            //스크롤뷰 사이즈 조정
        float scrollH = rectTransform.rect.height;//스크롤뷰 사이즈 조정
        rectTransform.sizeDelta = new Vector2(scrollW, scrollH);
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0);
        rectTransform.anchoredPosition = new Vector2(0, 0);

        for (int j = 0; j < row; j++)
        {
            scrollH = content.GetChild(j).GetComponent<RectTransform>().rect.height;
            content.GetChild(j).GetComponent<RectTransform>().sizeDelta = new Vector2(scrollW, scrollH);
        }

    }
    //있없체크
    public void UnitCheck()
    {
        for (int j = 0; j < row; j++)
        {
            for (int i = 0; i < column; i++)
            {
                int index = (j * column) + i;
                GameObject Unit = content.GetChild(j).transform.GetChild(i).gameObject;
                    
                //오픈 됨
                if (State.instance.myState.myAnimals[index].heart > 0)
                {
                    Unit.transform.Find("Name").GetComponent<Text>().text = Database.instance.animals[index].name + index.ToString();

                    Color color = new Color(1, 1, 1, 0);
                    Unit.transform.Find("Thumnail").transform.Find("Shadow").transform.Find("Color").GetComponent<Image>().color = color;
                }

            }
        }
    }
}
