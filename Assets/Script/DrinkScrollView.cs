using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject prefab;
    public Transform content;
    public int row;
    public int column;
    public bool setting = false;

    public void Open()
    {
        gameObject.transform.parent.transform.parent.gameObject.SetActive(true);
        
        UnitSet();
        ViewScaleSet();
       // UnitCheck();
        setting = true;
    }
    public void UnitSet()
    {
        DeleteAll();

        column = 3; //한 줄당 유닛 개수
        row = (int)Mathf.Ceil((State.instance.myState.myRecipe.Count + 1) / 3f);
        float xDistance = 262f;
        float yDistance = -350f;

        for (int j = 0; j < row; j++)
        {
            for (int i = 0; i < column; i++)
            {
                int index = (j * column) + i - 1;
                if (index == -1) continue;

                if (index > State.instance.myState.myRecipe.Count - 1) return;

                GameObject Unit = Instantiate<GameObject>(prefab, content.transform);
                RectTransform rectTransform = Unit.GetComponent<RectTransform>();
                //RecipeUnit recipeUnit = Unit.GetComponent<RecipeUnit>();
                Recipe recipe = State.instance.myState.myRecipe[index];
                //recipeUnit.MakeRecipe(recipe);
                
                if      (i == 0) rectTransform.anchoredPosition = new Vector2(-xDistance, yDistance * j - 165);
                else if (i == 1) rectTransform.anchoredPosition = new Vector2(0, yDistance * j - 165);
                else if (i == 2) rectTransform.anchoredPosition = new Vector2(xDistance, yDistance * j - 165);

            }
        }
    }
    //콘텐츠 뷰포트 사이즈 조정
    public void ViewScaleSet()
    {
        RectTransform rectTransform = content.GetComponent<RectTransform>();
        float scrollW = rectTransform.rect.width;           //스크롤뷰 사이즈 조정
        float scrollH = row * 350f;   //스크롤뷰 사이즈 조정
        rectTransform.sizeDelta = new Vector2(scrollW, scrollH);

        rectTransform.offsetMin = new Vector2(0, rectTransform.offsetMin.y);
        rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y);
        rectTransform.anchoredPosition = new Vector2(0, 0);
    }
    //음료 뽑기
    public void AddDrink()
    {
        /*
        int code = Random.Range(0, Database.instance.recipes.Count);
        State.instance.AddDrink(code);*/
        UnitSet();
        ViewScaleSet();
    }
    //재설정 위해 삭제
    public void DeleteAll()
    {
        for(int i = 0; i < content.transform.childCount; i++)
        {
            if (i == 0) continue;
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
}
