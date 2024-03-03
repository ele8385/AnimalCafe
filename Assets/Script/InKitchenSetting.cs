using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InKitchenSetting : MonoBehaviour
{
    //public GameObject Counter;
    public GameObject InKitchenUI;

    public OrderPapersManager orderPapersManager;
    public Transform orderPapersPos;
    public AnimalManager animalManager;

    // Start is called before the first frame update
    void Start()
    {
        OrderPaperPosSet();
    }
    public void InCounter()
    {
        animalManager.OpenMiniBalloon(true);
        InKitchenUI.SetActive(false);
        orderPapersManager.ViewOrderPapers();
    }

    public void InKitchen()
    {
        InKitchenUI.SetActive(true);
        animalManager.OpenMiniBalloon(false);
        orderPapersManager.ViewOrderPapers();
    }
    
    //게임시작할 때 게임씬에 있는 주문서 위치를 캔버스 위치로 전환해서 셋팅
    public void OrderPaperPosSet()
    {

        Vector2 vector = Camera.main.WorldToScreenPoint(orderPapersPos.position);
        RectTransform orderPaperPos = orderPapersManager.gameObject.GetComponent<RectTransform>();

        //RectTransform 좌푯값을 전달받을 변수
        Vector2 localPos = Vector2.zero;
        //스크린 좌표를 RectTransform 기준의 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(orderPaperPos
                                                                , vector
                                                                , Camera.main
                                                                , out localPos);
        // Debug.Log(vector.x + ", " + vector.y);
        orderPaperPos.localPosition = localPos;
    }
}
