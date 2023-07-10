using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderPaper : MonoBehaviour
{
    public OrderPapersManager OrderPapersManager;
    public RecipeManager recipeManager;
    public Recipe orderRecipe;
    public Text orderName;
    public AnimalMovement orderAnimal;
    public Animator animator;
    public CupManager cupManager;
    public RecipeInfo recipeInfo;
    public CameraMovement cameraMovement;
    public bool open;

    //주문서 레시피 넣어서 만들기
    public void MakeOrder(Recipe _orderRecipe, AnimalMovement _animal)
    {
        orderRecipe = _orderRecipe;
        recipeManager.MakeRecipe(orderRecipe);
        orderName.text = orderRecipe.name;
        orderAnimal = _animal;
        open = true;
        ViewOrder();
    }

    //주문서 보이기
    public void ViewOrder()
    {
        if (orderRecipe.name == "") return;
        if (!cameraMovement.inCounter) //In Kitchen
        {
            gameObject.SetActive(open);
            if (OrderPapersManager.waitSelecting == true)
                animator.SetBool("WaitSelecting", true);
            if (open)
                animator.Play("OpenStart");
        }
    }

    //주문서 선택 대기 중 바들바들 떠는 모션
    public void WaitSelect()
    {
        if (gameObject.activeSelf == true)
            animator.Play("WaitStart");
        //animator.SetTrigger("Wait_Select");
    }

    //주문서 선택 완료
    public void WaitEnd()
    {
        if (OrderPapersManager.waitSelecting == false)
            animator.SetBool("WaitSelecting", false);
        if (gameObject.activeSelf == true)
            animator.SetTrigger("Wait_End");
    }

    //주문서 클릭
    public void ClickOrderPaper()
    {
        if (cupManager.complate) //음료 완성 버튼 누른 상태에서 누르면 음료 전달
        {
            GiveDrink();
        }
        else //아닐 경우 모든 레시피 창 재오픈
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
        OrderPapersManager.WaitEndOrderPapers(this);        
    }

    //주문서 체크 효과 애니메이션에서 실행
    public void ResetOrderPaper()
    {
        open = false;
        animator.SetBool("Selected", false);
        animator.Rebind();
        ViewOrder();
    }
}
