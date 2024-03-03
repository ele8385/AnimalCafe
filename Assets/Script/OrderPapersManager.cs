using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPapersManager : MonoBehaviour
{
    public List<OrderPaper> orderPaper;
    public CameraMovement cameraMovement;
    public bool waitSelecting; //컵 완성 누른 뒤 대기 상태
    public bool endOrderPaperEffect; //주문서 심사 효과 진행 상태
    

    public void MakeOrderPaper(Recipe orderRecipe, int index, AnimalMovement _animal)
    {
        orderPaper[index].MakeOrder(orderRecipe, _animal);
        AudioManager.instance.PlaySFX("Order_Open");
    }
    //주문서 보이기
    public void ViewOrderPapers()
    {
            StartCoroutine("ViewOrderPapersCo"); //0.03초 간격으로 오픈
    }
    //음료 완성 후 내보낼 주문서 선택 대기 모션
    public void WaitSelectOrderPapers()
    {
        waitSelecting = true;
        endOrderPaperEffect = true;
        foreach (OrderPaper order in orderPaper)
        {
            order.WaitSelect();
        }
    }
    //선택 못받은 주문서 모션 끄기
    public void WaitEndOrderPapers(OrderPaper seletOrder)
    {
        waitSelecting = false;

        foreach (OrderPaper order in orderPaper)
        {
            if (seletOrder == order) order.animator.SetBool("Selected", true);
            else                     order.animator.SetBool("Selected", false);
            order.WaitEnd();
        }
    }
    
    //주문서 심사 효과 끝날 때 카메라 전환 잠금 해제 / 애니메이션에서 이벤트로 실행
    public void EndOrderPaperEffect()
    {
        StartCoroutine("EndOrderPaperEffectCo");
    }

    //약간의 시간차를 두고 주문서 차례대로 펼치기
    IEnumerator ViewOrderPapersCo()
    {
        if (! cameraMovement.inCounter)
        {
            foreach (OrderPaper order in orderPaper)
            {
                order.ViewOrder();
                yield return new WaitForSeconds(0.04f);
            }
        }
        else
        {
            foreach (OrderPaper order in orderPaper)
            {
                order.ViewOrder();
            }
        }
    }

    IEnumerator EndOrderPaperEffectCo()
    {
        yield return new WaitForSeconds(0.5f);
          endOrderPaperEffect = false;
    }

}
