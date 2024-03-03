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
        

        // SetActive 바꾸면 애니메이션 오류 심해서 Image 알파값으로 껐다 켜기 구현
        Image[] images = GetComponentsInChildren<Image>(true); // etComponentsInChildren을 사용해 모든 Image 컴포넌트를 찾고 색상을 변경
        foreach (Image img in images)
        {
            Color color = img.color;
            if (open && !OrderPapersManager.cameraMovement.inCounter)
            {
                color.a = 1f;
                orderName.enabled = true;
            }
            else
            {
                color.a = 0f;
                orderName.enabled = false;
            }
            img.color = color;
        }

        if (OrderPapersManager.waitSelecting == true)
            animator.SetBool("WaitSelecting", true);
        if (open)
            animator.Play("OpenStart");
    }

    //주문서 선택 대기 중 바들바들 떠는 모션
    public void WaitSelect()
    {
        if (gameObject.activeSelf == true)
            animator.Play("WaitStart");
        //animator.SetTrigger("Wait_Select");
    }

    //주문서 선택 완료 후 모든 주문서에게 실행시킴
    public void WaitEnd()
    {
        if (OrderPapersManager.waitSelecting == false) //완성 후 대기상태가 아니라면 
            animator.SetBool("WaitSelecting", false);
        if (gameObject.activeSelf == true) //주문서 열려있다면 선택대기 끝
            animator.SetTrigger("Wait_End");
    }


    //주문서 심사 효과 끝날 때 카메라 전환 잠금 해제 / 애니메이션에서 이벤트로 실행
    public void EndOrderPaperEffect()
    {
        OrderPapersManager.EndOrderPaperEffect();
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
        if(open && !OrderPapersManager.cameraMovement.inCounter)
        recipeInfo.OpenRecipeInfo(orderRecipe);
    }

    public void GiveDrink()
    {
        if (orderAnimal == null) return; 

        //주문 판별
        int result = orderAnimal.GiveDrink(cupManager.CupExport()); //음료를 동물에게 내보낸 후의 결과값

        if(result < 1) animator.SetInteger("result", 0); //0 이하는 모두 miss(0) 처리
        else animator.SetInteger("result", result);

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
