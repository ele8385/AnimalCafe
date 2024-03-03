using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public bool moving;    //움직이는 중인가
    public bool inCounter; //카운터 안에 있는가
    public Vector3 originPos; //카메라 위치 초기값
    public Vector3 toScene; //카메라 움직이는 위치
    public CameraScroll cameraScroll;
    public InKitchenSetting inKitchenSetting;
    public OrderPapersManager orderPapersManager;
    public TextPopUpManager TextPopUp;
    public Transform CounterPos;
    public Transform KitchenPos;
    public Image BtnImage;
    public Sprite CounterBtnSprite;
    public Sprite KitchenBtnSprite;

    private float distY;
    private float distZ;

    public void Start()
    {
        inCounter = true;
        originPos = transform.position;
        cameraScroll = GetComponent<CameraScroll>();
        distY = transform.position.y;  // Distance camera is above map
        distZ = transform.position.z;  // Distance camera is above map

        
    }
    
    public void CameraMoving()
    {
        if (orderPapersManager.waitSelecting == true) //주문서 선택 대기 중이면 카메라 전환 불가
        {
            TextPopUp.OpenPopUp("완성한 주문서를 선택해주세요."); return;
        }
        if (orderPapersManager.endOrderPaperEffect == true) //주문서 심사 효과 중이면 카메라 전환 불가
        {
            return;
        }
        //cameraScroll.enabled = inCounter ?  true : false;
        inCounter = !inCounter;

        Transform toObject;
        if (! inCounter) {
            cameraScroll.enabled = false;
            transform.position = originPos;
            inKitchenSetting.InKitchen();
            toObject = KitchenPos;
            BtnImage.sprite = CounterBtnSprite;
        }
        else {
            inKitchenSetting.InCounter();
            toObject = CounterPos;
            BtnImage.sprite = KitchenBtnSprite;
        }

        moving = true;
        Vector3 vector = new Vector3(0, 0, distZ);
        vector.x = toObject.position.x;
        vector.y = toObject.position.y;
        toScene = vector;
        StartCoroutine(MoveToScene());
        AudioManager.instance.PlaySFX("Click_Move");

        //카운터 안이면 스크롤 활성화, 밖이면 스크롤 비활성화
    }
    IEnumerator MoveToScene()
    {
        while (transform.position != toScene)
        {
            transform.position = Vector3.Lerp(transform.position, toScene, 0.5f);
            yield return null;
        }

        moving = false;
        if (inCounter)
        {
            cameraScroll.enabled = true;
        }
        else
        {
        }
    }

}
