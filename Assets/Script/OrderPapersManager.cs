using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPapersManager : MonoBehaviour
{
    public List<OrderPaper> orderPaper;

    public void MakeOrderPaper(Recipe orderRecipe, int index, AnimalMovement _animal)
    {
        orderPaper[index].MakeOrder(orderRecipe, _animal);
    }
    //주문서 보이기
    public void ViewOrderPapers()
    {
        foreach (OrderPaper order in orderPaper)
        {
            order.ViewOrder();
        }
    }
    //음료 완성 후 내보낼 주문서 선택 대기 모션
    public void WaitSelectOrderPapers()
    {
        foreach (OrderPaper order in orderPaper)
        {
            order.WaitSelect();
        }
    }
    //선택 못받은 주문서 모션 끄기
    public void WaitEndOrderPapers(OrderPaper seletOrder)
    {
        foreach (OrderPaper order in orderPaper)
        {
            if (seletOrder == order) continue;
            order.animator.SetBool("Selected", false);
            order.WaitEnd();
        }
    }
}
