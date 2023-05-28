using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupManager2 : MonoBehaviour
{
    /*
    public TextPopUpManager TextPopUp;
    public GameObject Pickup;
    public CupMixColor mixColor;
    public SpriteRenderer cupFrame;
    public SpriteMask liquidMask;
    public SpriteRenderer cupBack;
    public SpriteRenderer cupBack2;
    public RectTransform cupSelWindow;
    public BoxCollider2D boxCollider;
    public Animator animator;
    public Vector3 floorPos;
    public string cupHeight; //s 낮은컵 m 중간컵 l 높은컵

    public int cupNum;
    public bool moving = false; // 컵 움직이는 중인지
    public bool trashing = false; // 쓰레기통 롱클릭 중인지
    public bool windowMoving = false;


    // Use this for initialization
    void Start()
    {
        mixColor = GetComponent<CupMixColor>();
        animator = GetComponent<Animator>();
        cupFrame = transform.Find("Frame").GetComponent<SpriteRenderer>();
        liquidMask = transform.Find("LiquidMask").GetComponent<SpriteMask>();
        cupBack = transform.Find("Back").GetComponent<SpriteRenderer>();
        cupBack2 = transform.Find("Back (1)").GetComponent<SpriteRenderer>();

        cupSelWindow = GameObject.Find("Canvas").transform.Find("CupSel_window").transform.Find("Panel").GetComponent<RectTransform>();
        boxCollider = GetComponent<BoxCollider2D>();
        floorPos = transform.parent.Find("Table").transform.Find("Floor").gameObject.transform.position;

        ChangeCup(0);
        mixColor.ResetDrink();
    }
    public void CupClick()
    {
        if (mixColor.dropNum < 1)
        {
            StartCoroutine("WindowOpen");
            //ChangeCup(5);
        }
        else CupExport();
    }


    //컵 누르면 내보내는 애니메이션 실행
    public void CupExport()
    {
        if (!moving)
        {
            //컵 자리 꽉 찼으면 팝업
            int count = 0;
            for (int i = 0; i < Pickup.transform.childCount; i++)
            {
                GameObject ThumDrink = Pickup.transform.GetChild(i).gameObject;
                if (ThumDrink.activeSelf) count++;
            }
            if (count > 2) { TextPopUp.OpenPopUp("픽업대가 모두 찼어요!", -760f); return; }


            moving = true;
            boxCollider.enabled = false;
            animator.SetTrigger("Export");
        }
    }
    //CupExport애니메이션 끝날 때 실행됨
    public void CupExportEnd()
    {
        //컵썸넬 만들기
        mixColor.MakeThumDrink(cupNum);
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
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //컵에 재료 닿으면 기울어짐
        if (col.gameObject.layer == 9) col.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 30));

    }
    private void OnTriggerExit2D(Collider2D col)
    {
        //StartCoroutine("DropDelay");
        //컵에 재료 벗어나면 원상복귀
        if (col.gameObject.layer == 9) col.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!moving)
        {
            moving = true;
            //컬러재료 드래그
            mixColor.Dropping(col);
        }
    }

    public void ChangeCup(int _cupNum)
    {
        cupNum = _cupNum;
        cupBack.sprite = Resources.Load<Sprite>("Cup/Cup-" + cupNum);
        cupBack2.sprite = Resources.Load<Sprite>("Cup/Cup-" + cupNum);
        cupFrame.sprite = Resources.Load<Sprite>("Cup/Frame-" + cupNum);

        //액체 스프라이트 넣고 액체 높이 할당 0:낮음 1:중간 2:높음
        if (Resources.Load<Sprite>("Cup/Liquid-" + cupNum + "_m") != null) { liquidMask.sprite = Resources.Load<Sprite>("Cup/Liquid-" + cupNum + "_m"); cupHeight = "m"; }
        else if (Resources.Load<Sprite>("Cup/Liquid-" + cupNum + "_s") != null) { liquidMask.sprite = Resources.Load<Sprite>("Cup/Liquid-" + cupNum + "_s"); cupHeight = "s"; }
        else if (Resources.Load<Sprite>("Cup/Liquid-" + cupNum + "_l") != null) { liquidMask.sprite = Resources.Load<Sprite>("Cup/Liquid-" + cupNum + "_l"); cupHeight = "l"; }

        mixColor.ResetDrink();
        //WindowClose();
        CupImport();
    }

    //쓰레기통에서 롱클릭 시 1회 실행
    public void TrashLongClick()
    {
        if (trashing)
        {
            Debug.Log("long");

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
    /*
    public void WindowClose()
    {
        StartCoroutine("WindowCloseCo");
    }

    IEnumerator WindowOpen()
    {
        if (!windowMoving)
        {
            windowMoving = true;
            cupSelWindow.transform.parent.gameObject.SetActive(true);
            Vector2 toPos;
            toPos = new Vector2(0, 20);
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
            toPos = new Vector2(0, -1000);

            transform.position = new Vector3(transform.position.x, transform.position.y + 20f, transform.position.z);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            while (windowMoving)
            {
                cupSelWindow.anchoredPosition = Vector2.MoveTowards(cupSelWindow.anchoredPosition, toPos, 5000f * Time.deltaTime);
                if (cupSelWindow.anchoredPosition.y <= toPos.y + 0.01) { windowMoving = false; break; }
                yield return null;
            }
            cupSelWindow.transform.parent.gameObject.SetActive(false);

            StartCoroutine("CupMoveDown");
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
