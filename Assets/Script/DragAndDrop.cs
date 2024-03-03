using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DragAndDrop : MonoBehaviour {
    public GameObject target;
    public Vector2 temp;
    public Vector3 originPos;
    public CupManager cup;
    private Vector3 screenSpace;
    private Vector3 offset;
    private Vector3 nowPos;
    public bool openUI;
        
    const float dragAccuracy = 50f;
    public float clickTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetMouseButton(0)) //터치했다면
        {
            nowPos = (Input.touchCount == 0) ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
            
            if (Input.GetMouseButtonDown(0)) //터치 시작인경우
            {
                target = CastRay();
                if (target)
                {
                    //Debug.Log(target + "클릭");

                    //드래그가능오브젝트
                    if (target.layer == 9)
                    {
                        originPos = target.transform.position;
                        screenSpace = Camera.main.WorldToScreenPoint(target.transform.position); //오브젝트 월드좌표 구하기
                        offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
                        if(target.tag == "Color")
                        {
                            AudioManager.instance.PlaySFX("Click_Color");
                        }
                    }
                    //롱클릭오브젝트 누르고있으면창켜기
                    else if(target.layer == 10)
                    {
                        if(target.GetComponent<ClickEvent>() != null) target.GetComponent<ClickEvent>().SetOn();
                    }
                    //쓰레기통 한번 클릭
                    else if (target.name == "Trash")
                    {
                        target.GetComponent<BoxCollider2D>().enabled = false;
                        cup.ThrowAwayDrink();

                    }
                    //팁 클릭
                    else if (target.tag == "Coin")
                    {
                        target.gameObject.GetComponent<TableMoney>().ClickMoney();
                    }

                    //터치 시 이미지교체 스크립트
                    if (target.GetComponent<ClickChange>() != null)
                    {
                        target.GetComponent<ClickChange>().ChangeImg();
                    }
                }
            }

            if (target)
            {
                //쓰레기통 롱클릭
                if (target.name == "Trash")
                {
                    if (clickTime <= 0.5f) clickTime += Time.deltaTime;
                    else
                    {
                        target.GetComponent<BoxCollider2D>().enabled = false;
                        cup.TrashLongClick();
                    }
                }
                //드래그오브젝트 실행
                if (target.layer == 9)
                {
                    var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
                    var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

                    target.transform.position = curPosition;
                    //Debug.Log("드래그앤 드롭 작동중");

                    //드래그 중인 오브젝트는 항상 앞에 보이게
                    if (target.GetComponent<SpriteRenderer>() != null) target.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;

                    GetComponent<CameraScroll>().windowMoving = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))  //터치 끝
        {
            clickTime = 0;

            if (target)
            {
                //터치 시 이미지교체 스크립트 종료
                if (target.GetComponent<ClickChange>() != null) target.GetComponent<ClickChange>().GetbackImg();
                //동물 대화풍선 클릭
                else if (target.name == "SpeakBalloon")
                {
                    target.transform.parent.gameObject.GetComponent<BalloonManager>().ClickSpeakBalloon();
                }
                //동물 주문풍선 클릭
                else if (target.name == "OrderBalloon")
                {
                    target.transform.parent.gameObject.GetComponent<BalloonManager>().ClickOrderBalloon();
                }
                else if (target.name == "DrinkGiveBtn")
                {
                    //target.transform.parent.gameObject.GetComponent<OrderPaper>().GiveDrink(cup.CupExport());

                }
                //드래그오브젝트 실행완료
                if (target.layer == 9)
                {
                    //타겟 제자리로
                    target.transform.position = originPos;
                    target.GetComponent<BoxCollider2D>().enabled = false;
                    if (target.GetComponent<SpriteRenderer>() != null) target.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

                    GameObject DropObject = CastRay();
                    if (DropObject) 
                    {
                        /*
                        if (target.tag == "Drink" && DropObject.tag == "AnimalData") //음료를 동물에게 줌
                        {

                            DrinkManager drink = target.GetComponent<DrinkManager>();
                            if (DropObject.GetComponent<AnimalMovement>().GiveDrink(drink)) //음료주면 true 못줄상태면false
                            {
                                target.gameObject.SetActive(false);
                            }
                        }*/
                    }
                    target.GetComponent<BoxCollider2D>().enabled = true;
                    GetComponent<CameraScroll>().windowMoving = false;

                }
                //롱클릭오브젝트
                else if (target.layer == 10)
                {
                    if (target.GetComponent<ClickEvent>() != null) target.GetComponent<ClickEvent>().SetOff();
                }
                else
                {
                    if (target.tag == "Cup")
                    {
                        target.GetComponent<CupManager>().CupClick();
                    }
                    else if (target.name == "Trash")
                    {
                        cup.TrashClickEnd();
                        target.GetComponent<BoxCollider2D>().enabled = true;

                    }

                }

            }
        }
    }

    void CastRay_() // 유닛 히트처리 부분.  레이를 쏴서 처리합니다. 
    {
        target = null;
        //GetComponent<BoxCollider2D>().enabled = false;

        Vector2 pos = Camera.main.ScreenToWorldPoint(temp);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);


        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);  //이 부분을 활성화 하면, 선택된 오브젝트의 이름이 찍혀 나옵니다. 

            target = hit.collider.gameObject;  //히트 된 게임 오브젝트를 타겟으로 지정

            /*
            if (gameObject.tag == "Drink" && target.tag == "AnimalData") //음료를 동물에게 줌
            {
                Debug.Log("동물클릭");

                DrinkManager drink = GetComponent<DrinkManager>();
                if (target.GetComponent<AnimalMovement>().GiveDrink(drink.dropDivNum, drink.recipe, drink.syrup, drink.topping)) //음료주면 true 못줄상태면false
                {
                    gameObject.SetActive(false);
                }
            }*/
        }
        //GetComponent<BoxCollider2D>().enabled = true;
    }
    public GameObject CastRay() // 유닛 히트처리 부분.  레이를 쏴서 처리합니다. 
    {
        GameObject RayTarget = null;

        Vector2 pos = Camera.main.ScreenToWorldPoint(nowPos);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
        
        if (hit.collider != null)
        {
            RayTarget = hit.collider.gameObject;  //히트 된 게임 오브젝트를 타겟으로 지정
            
        }
        return RayTarget;
    }
}
