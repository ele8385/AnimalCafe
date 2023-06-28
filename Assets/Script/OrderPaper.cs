using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderPaper : MonoBehaviour
{
    public RecipeManager recipeManager;
    public Recipe orderRecipe;
    public Text orderName;
    public AnimalMovement orderAnimal;
    public Animator animator;
    public CupManager cupManager;
    public RecipeInfo recipeInfo;
    public bool open;

    public void MakeOrder(Recipe _orderRecipe, AnimalMovement _animal)
    {
        orderRecipe = _orderRecipe;
        recipeManager.MakeRecipe(orderRecipe);
        orderName.text = orderRecipe.name;
        orderAnimal = _animal;
        open = true;
        ViewOrder();
    }

    public void ViewOrder()
    {
        /*
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(open);
        }*/
        if (open)
        {
            gameObject.SetActive(true);
            animator.SetBool("Open", true);
        }
        else
        {

        }
        gameObject.SetActive(open);
        animator.SetBool("Open", open);
    }

    //주문서 선택 대기 중 바들바들 떠는 모션
    public void WaitSelect()
    {
        animator.SetTrigger("Wait_Select");
    }

    //주문서 선택 완료
    public void WaitEnd()
    {
        animator.SetTrigger("Wait_End");
    }

    //주문서 클릭
    public void ClickOrderPaper()
    {
        if (cupManager.complate) //음료 완성 버튼 누른 상태에서 누르면 음료 전달
        {
            GiveDrink();
        }
        else //아닐 경우 레시피 창 오픈
        {
            OpenRecipeInfo();
        }
    }

    //주문서 정보창 오픈
    public void OpenRecipeInfo()
    {
        recipeInfo.OpenRecipeInfo(orderRecipe);
    }

    public void GiveDrink()
    {
        if (orderAnimal == null) return; 
        orderAnimal.GiveDrink(cupManager.CupExport());
        animator.SetBool("Selected", true);
        WaitEnd();
        transform.parent.GetComponent<OrderPapersManager>().WaitEndOrderPapers(this);
        ResetOrderPaper();
        ViewOrder();
    }

    public void ResetOrderPaper()
    {
        open = false;
        //animator.SetBool("Selected", false);
    }
}
