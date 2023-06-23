using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSetting : MonoBehaviour
{
    //비활성화 오브젝트의 초기화스크립트 실행 용도

    public RectTransform orderPaperPos;
    public Camera camera;
    // Start is called before the first frame updates
    void Awake()
    {

    }

    private void Start()
    {
        Vector2 vector = camera.WorldToScreenPoint(GameObject.Find("OrderPapersPos").gameObject.transform.position);

        //RectTransform 좌푯값을 전달받을 변수
        Vector2 localPos = Vector2.zero;
        //스크린 좌표를 RectTransform 기준의 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(orderPaperPos
                                                                , vector
                                                                , camera
                                                                , out localPos);
       // Debug.Log(vector.x + ", " + vector.y);
        orderPaperPos.localPosition = localPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
