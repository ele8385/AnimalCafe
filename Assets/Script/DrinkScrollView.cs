using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject prefab;
    public Transform content;
    public bool setting = false;

    public void Open()
    {
        gameObject.transform.parent.transform.parent.gameObject.SetActive(true);

        DeleteAll();
        UnitSet();
        ViewScaleSet();
       // UnitCheck();
        setting = true;
    }
    public void UnitSet()
    {
        List<Recipe> tempRecipes = Database.instance.recipes;
        
        // 이름순으로 오름차순 정렬
        tempRecipes.Sort((recipe1, recipe2) => recipe1.name.CompareTo(recipe2.name));
      
        foreach (Recipe recipe in tempRecipes)
        {
            //레시피 유닛 생성 및 레시피 할당
            GameObject Unit = Instantiate<GameObject>(prefab, content.transform);
            RecipeManager recipeManager = Unit.gameObject.transform.Find("Recipe").gameObject.GetComponent<RecipeManager>();
            recipeManager.MakeRecipe(recipe);

            //미획득 레시피
            Recipe temp = State.instance.GetMyRecipe(recipe.code);
            if (temp == null)
            {
                Unit.GetComponent<RecipeUnit>().HideRecipe();
            }
            else
            {
                Unit.GetComponent<RecipeUnit>().ShowRecipe();
            }
        }
    }

    //콘텐츠 뷰포트 높이 조정
    public void ViewScaleSet()
    {
        RectTransform rectTransform = content.GetComponent<RectTransform>();
        float scrollW = rectTransform.rect.width;         
        float scrollH = (Mathf.CeilToInt(Database.instance.recipes.Count / 3) + 1) * 408f + 28;   //스크롤뷰 높이 계산 (content.childCount로 계산하면 자꾸 2배 높이가 됨 딜리트올 유닛셋 순서로 해도 동일
        rectTransform.sizeDelta = new Vector2(scrollW, scrollH);
    }

    //재설정 위해 삭제
    public void DeleteAll()
    {
        for(int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
}
