using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupManager : MonoBehaviour
{

    public TextPopUpManager TextPopUp;
    public OrderPapersManager orderPapersManager;
    public CupMixColor mixColor;
    public SpriteRenderer cupFrame;
    public SpriteRenderer cupCap;
    public SpriteMask liquidMask;
    public SpriteRenderer cupBack;
    public RectTransform cupSelWindow;
    public BoxCollider2D boxCollider;
    public Animator animator;
    public Vector3 floorPos;
    public Transform Liquid;
    private float popupPosY = -700f;

    public int cupHeight; //0 중간컵 1 낮은컵(티) 2 높은컵(파르페)
    public float liquidDepth; //"Liquid"가 컵 아래로 내려가는 깊이 0: -4.8

    public int cupNum;
    public bool moving = false; // 컵 움직이는 중인지
    public bool clickLock = false; // 컵 움직이는 중인지
    public bool trashing = false; // 쓰레기통 롱클릭 중인지
    public bool windowMoving = false;
    public bool complate = false;


    // Use this for initialization
    void Start()
    {
        cupSelWindow = GameObject.Find("Canvas").transform.Find("CupSel_window").transform.Find("Panel").GetComponent<RectTransform>();
        floorPos = transform.parent.Find("Table").transform.Find("Floor").gameObject.transform.position;
        
        ChangeCup(0);
        mixColor.ResetDrink();
    }
    public void CupClick()
    {
        if (mixColor.dropNum < 1)
        {
            WindowOpen();
            //ChangeCup(5);
        }
    }


    //완성버튼클릭
    public void ClickMakeBtn()
    {
        if (mixColor.dropNum < 1) { TextPopUp.OpenPopUp("컵이 비어서 완성할 수 없어요.", popupPosY); return; }
        if (!moving)
        {
            moving = true;
            boxCollider.enabled = false;
            complate = true;
            animator.SetTrigger("CapClose");
            orderPapersManager.WaitSelectOrderPapers();
            AudioManager.instance.PlaySFX("Click_DrinkMake");
        }
    }


    //주문서 누르면 음료 내보내기
    public Drink CupExport()
    {
        if (complate)
        {
            animator.SetTrigger("Export");
            return mixColor.MakeThumDrink(cupNum);
        }
        return null;
    }
    //CupExport애니메이션 끝날 때 실행됨
    public void CupExportEnd()
    {
        //음료리셋
        mixColor.ResetDrink();
    }

    //컵 누르면 가져오는 애니메이션 실행(컵 바꿀 때 씀)
    public void CupImport()
    {
        if (!moving)
        {
            moving = true;
            boxCollider.enabled = false;

            animator.SetTrigger("Import");
        }
    }

    //CupImport애니메이션 끝날 때 실행됨
    public void CupImportEnd()
    {
        //제자리로 오면 콜라이더 활성화 및 터치잠금 해제
        boxCollider.enabled = true;
        moving = false;
        complate = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //컵에 재료 닿으면 기울어짐
        if (col.gameObject.layer == 9) col.gameObject.GetComponent<ClickChange>().Change2Img();
        if (!moving && col.gameObject.tag != "Color" && !clickLock)
        {
            mixColor.DroppingEtc(col);
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        clickLock = false;
        //컵에 재료 벗어나면 원상복귀
        if (col.gameObject.layer == 9) col.gameObject.GetComponent<ClickChange>().GetbackImg();
        //AudioManager.instance.StopSFX("Cup_Pour");

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!moving && col.gameObject.tag == "Color")
        { 
            if(mixColor.DroppingColor(col)) moving = true; 
        }
    }

    public void ChangeCup(int _cupNum)
    {
        cupNum = _cupNum;

        //액체 스프라이트 넣고 액체 높이 할당
        if (cupNum < 10) cupHeight = 0;
        else if (cupNum < 20) cupHeight = 1;
        else if (cupNum < 30) cupHeight = 2;

        
        liquidMask.sprite = Resources.Load<Sprite>("Cup/Liquid-" + cupNum);
        cupBack.sprite = Resources.Load<Sprite>("Cup/Liquid-" + cupNum);
        cupFrame.sprite = Resources.Load<Sprite>("Cup/Frame-" + cupNum);

        //뚜껑 있는 컵 체크하고 뚜껑 이미지 할당
        if (Resources.Load<Sprite>("Cup/Cap-" + cupNum) != null)
        {
            cupCap.sprite = Resources.Load<Sprite>("Cup/Cap-" + cupNum);
        }
        else cupCap.sprite = null;


        Vector3 temp = Liquid.localPosition;
        temp.y = liquidDepth;
        Liquid.localPosition = temp;
        
        mixColor.ResetDrink();
        //WindowClose();
        CupImport();
    }

    //쓰레기통에서 롱클릭 시 1회 실행
    public void TrashLongClick()
    {
        if (trashing)
        {
            trashing = false;
            animator.SetTrigger("ThrowLongEnd");
            mixColor.SizeDownLiquidAll();
        }
    }

    //쓰레기통에서 클릭 끝났을 때 실행됨
    public void TrashClickEnd()
    {
        if (trashing)
        {
            trashing = false;
            animator.SetTrigger("ThrowEnd");
        }
    }

    //음료 한번 버리기
    public void ThrowAwayDrink()
    {
        if (moving || trashing) return;

        animator.SetTrigger("Throw");
        trashing = true;
        moving = true;
        //trashing = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        mixColor.SizeDownLiquid();

        //StartCoroutine("ThrowAwayCo");
    }

    //ThrowEnd, ThrowLondEnd 애니메이션 끝날 때 실행됨
    public void ThrowAwayEnd()
    {
        moving = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
    /*
    //음료 버리기
    IEnumerator ThrowAwayCo()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        //컵 기울어짐
        Vector3 toPos;
        toPos = new Vector3(transform.position.x + 80, transform.position.y - 50f, transform.position.z);
        //transform.position = toPos;

        Quaternion toLot;
        toLot = Quaternion.Euler(0, 0, 80f);
        while (true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, toLot, 0.4f);
            transform.Translate(toPos * 0.2f * Time.deltaTime);
            if (transform.rotation == toLot) break;
            yield return null;
        }
        //컵 세워짐
        toLot = Quaternion.Euler(0, 0, 0);
        while (true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, toLot, 0.4f);
            transform.Translate(floorPos * 0.2f * Time.deltaTime);

            if (transform.rotation == toLot)
            {
                //음료리셋 및 쓰레기통 클릭 가능
                break;
            }
            yield return null;
        }
        transform.position = floorPos;
        moving = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }*/
    public void WindowOpen()
    {
        GameObject.Find("Canvas").gameObject.GetComponent<TouchLock>().SetOn();
        StartCoroutine("WindowOpenCo");
    }
    public void WindowClose()
    {
        GameObject.Find("Canvas").gameObject.GetComponent<TouchLock>().SetOff();
        StartCoroutine("WindowCloseCo");
    }

    IEnumerator WindowOpenCo()
    {
        if (!windowMoving)
        {
            windowMoving = true;
            cupSelWindow.transform.parent.gameObject.SetActive(true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            Vector2 toPos;
            toPos = new Vector2(0, -100f);
            while (true)
            {
                cupSelWindow.anchoredPosition = Vector2.Lerp(cupSelWindow.anchoredPosition, toPos, 0.3f);
                if (cupSelWindow.anchoredPosition.y >= toPos.y - 0.01) { windowMoving = false; break; }
                yield return null;
            }
        }
    }
    IEnumerator WindowCloseCo()
    {
        if (!windowMoving)
        {
            windowMoving = true;
            Vector2 toPos;
            toPos = new Vector2(0, -600);

            transform.position = new Vector3(transform.position.x, transform.position.y + 20f, transform.position.z);

            while (windowMoving)
            {
                cupSelWindow.anchoredPosition = Vector2.MoveTowards(cupSelWindow.anchoredPosition, toPos, 5000f * Time.deltaTime);
                if (cupSelWindow.anchoredPosition.y <= toPos.y + 0.01) { windowMoving = false; break; }
                yield return null;
            }
            cupSelWindow.transform.parent.gameObject.SetActive(false);

            //컵 import 되는 중에 콜라이더 켜지지 않게 시간차 두기
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            
        }
    }
    /*
    IEnumerator CupMoveDown()
    {
        if (!moving)
        {
            moving = true;
            boxCollider.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);

            while (true)
            {
                transform.position = Vector3.Lerp(transform.position, floorPos, 0.2f);
                if (transform.position == floorPos) break;
                yield return null;
            }
            boxCollider.enabled = true;
            moving = false;
        }
    }

    IEnumerator CupMoveRight()
    {
        if (!moving)
        {
            moving = true;

            boxCollider.enabled = false;
            //원래자리에서 왼쪽으로 컵 이동
            Vector3 toPos;
            toPos = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z);
            while (true)
            {
                //Debug.Log("왼");
                transform.position = Vector3.MoveTowards(transform.position, toPos, 0.8f);
                if (transform.position == toPos) break;
                yield return null;
            }
            
            //컵썸넬 만들기
            mixColor.MakeThumDrink(cupNum);
            
            //음료리셋
            mixColor.ResetDrink();
            
            //오른쪽에서 원래자리로 컵 이동
            transform.position = new Vector3(floorPos.x - 10, floorPos.y, floorPos.z);
            toPos = new Vector3(floorPos.x + 0.5f, floorPos.y, floorPos.z);

            while (true)
            {
                transform.position = Vector3.Lerp(transform.position, toPos, 0.3f);
                if (transform.position.x >= floorPos.x) break;
                yield return null;
            }

            yield return new WaitForSeconds(0.05f);

            //제자리로 오면 콜라이더 활성화 및 터치잠금 해제
            boxCollider.enabled = true;
            moving = false;
        }
    }*/

}
