using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CupMixColor : MonoBehaviour
{
    public CupManager cupManager;
    public GameObject Liquid;
    public Transform decoration;
    public SpriteRenderer frame;
    public SpriteRenderer topping;
    public Animator whipping;
    public SpriteRenderer layer;
    public Sprite liquid_gradient;
    public Sprite liquid_block;
    public Animator effect;
    public Vector3 floorPos;
    private float popupPosY = -700f;

    public List<GameObject> LiquidDiv;
    public List<Vector3> liquidPos;

    public int dropNum = 0;
    public int dropMaxNum = 6;
    public int cupHeight;
    public List<int> dropDivNum; //층 나눈 넘버 { 0, 3, 5 } -> [3][5]에서 층 나눔. MakeDrink에서 마지막 dopNum 추가
    public Color toColor;
    public List<Color> colors;

    public float liquidBottomPos = 0.3f; // floorPos와 액체 바닥 간의 로컬거리
    public float height = 3.53f; //액체 높이 보정

    public bool noMix = false;
    public bool trashing = false;
    public List<int> gradient;
    
    public string syrupName = "none";
    public string toppingName = "none";
    public string whippingName = "none";

    public GameObject Pickup;

    // Start is called before the first frame update
    void Start()
    {
        floorPos = decoration.position;

        //ResetDrink(); 컵 높이부터 정해야 해서 CupManager의 Start()에서 실행
    }

    //색상 떨어뜨림(누르고 있으면 일정 간격으로 호출)
    public bool DroppingColor(Collider2D col)
    {
        if (dropNum >= dropMaxNum) { cupManager.TextPopUp.OpenPopUp("컵이 꽉 찼어요", popupPosY); return false; } // 이미 넣음

        if (noMix) DoNotMix(); //층 나눔

        dropNum++;
        MixDrink(col.gameObject.name);

        //가루 효과 잠금 해제
        noMix = false;

        //액체 조각마다 색 넣기 ex) { 0, 3 }이고 5칸 넣었을 때 Div[5 - 1]부터 Div[3]까지 색상 바꾸기
        for (int i = dropMaxNum - 1; i >= 0; i--)
        {
            if (dropDivNum[dropDivNum.Count -1] - 1 < i && i < dropNum)
                StartCoroutine(ChangeColor(
                    LiquidDiv[i].GetComponent<SpriteRenderer>(), toColor));
        }

        StartCoroutine("SizeUpLiquid");
        AudioManager.instance.PlaySFX("Cup_Pour");

        return true; 
    }
    //기타 떨어뜨림(누르면 한 번만 호출)
    public void DroppingEtc(Collider2D col)
    {
        if (col.gameObject.tag == "Topping")
        {
            if (!Resources.Load<Sprite>("Kitchen/Topping_" + col.gameObject.name)) { Debug.Log("Kitchen/Topping_" + col.gameObject.name + "이미지 없음"); return; }

            toppingName = col.gameObject.name;
            topping.sprite = Resources.Load<Sprite>("Kitchen/Topping_" + toppingName);

            StartCoroutine(CoFadeIn(topping));
        }

        else if (col.gameObject.name == "Layer")
        {
            if (dropNum < 1) { cupManager.TextPopUp.OpenPopUp("빈 컵엔 뿌릴 수 없어요", popupPosY); return; } // 빈컵
            if (noMix) { cupManager.TextPopUp.OpenPopUp("이미 넣었어요", popupPosY); return; } // 이미 넣음
            //그라데이션 넣고 이거 바로 넣어도 상관 없을 듯
            effect.Play("Shiny", -1);
            noMix = true;
            AudioManager.instance.PlaySFX("Cup_Layer");
        }

        else if (col.gameObject.name == "Gradient")
        {
            if (dropNum < 1) { cupManager.TextPopUp.OpenPopUp("빈 컵은 섞을 수 없어요", popupPosY); return; } // 빈컵
            if (dropNum == gradient.Last()) { cupManager.TextPopUp.OpenPopUp("이미 섞었어요", popupPosY); return; } // 이미 넣음

            //마지막그라디언트 이후로 2개 이상의 색이 안 쌓인 경우                                                                                                         
            List<int> temp = dropDivNum; if(dropDivNum.Last() != dropNum) dropDivNum.Add(dropNum); //현재 dropNum을 추가한 dropDivNum리스트 임시 생성
            int a = temp.IndexOf(gradient.Last()); //마지막 그라디언트한 Div의 인덱스
            if (a >= temp.Count - 2) { cupManager.TextPopUp.OpenPopUp("두 가지 이상의 색이 쌓여야\n섞을 수 있어요", popupPosY); return; }

            Gradient();
            noMix = true;
            AudioManager.instance.PlaySFX("Cup_Mix");

        }

        else if (col.gameObject.name == "Whippings")
        {
            if (dropNum < dropMaxNum - 1) { cupManager.TextPopUp.OpenPopUp("컵을 꽉 채워야 휘핑을 올릴 수 있어요", popupPosY); return; }

            whipping.ResetTrigger("reset");
            //휘핑 넘버 설정
            whipping.SetInteger("num", 0);
            AudioManager.instance.PlaySFX("Cup_Whipping");

        }
    }

    //색섞기c
    public void MixDrink(string color)
    {
        float count = dropNum - dropDivNum.Last();
        float originRatio = (count - 1) / count; //originRatio(기존 음료 비율):nextRatio(새로 붓는 비윳)
        float nextRatio = 1 / count; //새로 붓는 음료의 비율
        Color nextColor;

        /*
        if (color == "Cyan")            nextColor = SetColor(0, 196, 255); 
        else if (color == "Magenta")    nextColor = SetColor(255, 60, 171); 
        else if (color == "Yellow")     nextColor = SetColor(255, 222, 0); 
        */
        if (color == "Cyan")            nextColor = SetColor(0, 196, 255); 
        else if (color == "Magenta")    nextColor = SetColor(255, 60, 171);
        else if (color == "Yellow")     nextColor = SetColor(255, 222, 0);
        else if (color == "Red")        nextColor = SetColor(240, 65, 65);  
        else if (color == "Green")      nextColor = SetColor(60, 180, 0); 
        else if (color == "Blue")       nextColor = SetColor(0, 70, 210);  
        else if (color == "Black")      nextColor = SetColor("black");     
        else if (color == "White")      nextColor = SetColor("white"); 
        else { nextColor = SetColor(""); }
        
        float r = toColor.r * 255 * originRatio + nextColor.r * 255 * nextRatio;
        float g = toColor.g * 255 * originRatio + nextColor.g * 255 * nextRatio;
        float b = toColor.b * 255 * originRatio + nextColor.b * 255 * nextRatio;
        //범위 넘어가지 않게
        r = Mathf.Clamp(r, 0, 255f);
        b = Mathf.Clamp(b, 0, 255f);
        g = Mathf.Clamp(g, 0, 255f);

        toColor = SetColor(r, g, b);
        ColorsExport();
        //recipe = string.Concat(recipeTemp, Mathf.Round(r), ",", Mathf.Round(g), ",", Mathf.Round(b), "-"); //반올림해서 레시피에 넣기
    }
    public void ResetDrink()
    {
        
        dropNum = 0;
        toColor = new Color(1,1,1,0);
        topping.sprite = null;

        //액체 높이에 따른 조정
        if (cupHeight == 0) { height = 3.53f; }  //중간컵
        else if(cupHeight ==1) { height = 3.53f; }  //낮은컵
        else if(cupHeight == 2) { height = 3.53f; }  //높은컵(와인잔)
        else { Debug.Log("액체 높이 연결 오류"); }

        liquidPos = new List<Vector3>();
        decoration.localPosition = new Vector3(0,0, decoration.localPosition.z);
        for (int i = 0; i < dropMaxNum; i++)
        {
            LiquidDiv[i].GetComponent<SpriteRenderer>().color = toColor;
            LiquidDiv[i].GetComponent<SpriteRenderer>().sprite = liquid_block;
            LiquidDiv[i].transform.localPosition = new Vector3(0, 0, LiquidDiv[i].transform.localPosition.z);
            liquidPos.Add(new Vector3(0, height * ((i + 1) / 6f), LiquidDiv[i].transform.localPosition.z)); //컵높이에 따라 액체 올라갈 높이 설정
        }
        colors = new List<Color>();
        dropDivNum = new List<int>();
        gradient = new List<int>();
        dropDivNum.Add(0);
        gradient.Add(0);
        noMix = false;
        topping.sprite = null;
        syrupName = "none";
        toppingName = "none";

        whipping.SetInteger("num", -1);
        whipping.SetTrigger("reset");


        //toppingMask.enabled = false;
    }

    //카운터로 보낼 음료 만들기
    public Drink MakeThumDrink(int cupNum)
    {
        ColorsExport();
        dropDivNum.Add(dropNum);
        return new Drink(cupNum, colors, gradient, whippingName, syrupName, toppingName);
    }

    public void Gradient()
    {
        for (int i = gradient.Last(); i < dropNum - 1; i++)
        {
            //액체 그라디언트 이미지로 바꾸고 맨 윗 액체 빼고 액체 한 칸의 0.7배만큼 포지션Y 위로 올림
            LiquidDiv[i].GetComponent<SpriteRenderer>().sprite = liquid_gradient;
            LiquidDiv[i].transform.localPosition = new Vector3(liquidPos[i].x, liquidPos[i].y + (height / 6) * 0.7f, liquidPos[i].z); ;
        }
        gradient.Add(dropNum);
        effect.Play("Shiny", -1);
    }

    public void DoNotMix()
    {
        if (dropNum >= dropMaxNum) return;
        dropDivNum.Add(dropNum);
        toColor = new Color(1, 1, 1, 0);
    }


    //현재 칸별 색상 리스트 만들기
    public void ColorsExport()
    {
        colors = new List<Color>();

        for (int i = 0; i < dropMaxNum; i++)
        {
            Color thisColor = LiquidDiv[i].gameObject.GetComponent<SpriteRenderer>().color;
            float rr = (float)System.Math.Round(thisColor.r, 3);
            float gg = (float)System.Math.Round(thisColor.g, 3);
            float bb = (float)System.Math.Round(thisColor.b, 3);
            float aa = (float)System.Math.Round(thisColor.a, 3);
            
            //colors.Add(new Color( rr, gg, bb));
            colors.Add(new Color(thisColor.r, thisColor.g, thisColor.b, thisColor.a));
        }
    }
    
    //액체 한번 버리기
    public void SizeDownLiquid()
    {
        if (dropNum <= 0) return; // 빈 컵이면 반환

        dropNum--;
        noMix = false;

        int lenth = dropDivNum.Count - 1;
        if (dropNum > 0)
        {
            LiquidDiv[dropNum - 1].GetComponent<SpriteRenderer>().sprite = liquid_block;
            LiquidDiv[dropNum - 1].transform.localPosition = liquidPos[dropNum - 1];
            if (dropNum == dropDivNum.Last())  //칸 나눈 색 모두 버렸을 때
            {
                toColor = LiquidDiv[dropNum - 1].gameObject.GetComponent<SpriteRenderer>().color;
                dropDivNum.RemoveAt(lenth);
            }
        }
        StartCoroutine("SizeDownLiquidCo");
        AudioManager.instance.PlaySFX("Click_Trash");
    }


    //액체 전부 버리기
    public void SizeDownLiquidAll()
    {
        noMix = false;
    }

    public Color SetColor(float r, float g, float b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }
    public Color SetColor(string color)
    {
        if (color == "black") return new Color(60 / 255f, 50 / 255f, 50 / 255f);
        else if (color == "white") return new Color(255 / 255f, 255 / 255f, 255 / 255f);
        else return new Color(255 / 255f, 255 / 255f, 255 / 255f);
    }


    //액체 한번 버리기 코루틴
    IEnumerator SizeDownLiquidCo()
    {
        float count = 90f;
        GameObject ThisLiquid = LiquidDiv[dropNum].gameObject;

        //액체와 토핑 높이 보정
        Vector3 toPos = new Vector3(ThisLiquid.transform.localPosition.x, 0, ThisLiquid.transform.localPosition.z);
        Vector3 DecoToPos = new Vector3(decoration.localPosition.x, height * (dropNum / 6f), decoration.localPosition.z);

        while (ThisLiquid.transform.localPosition.y > toPos.y + 0.05f)
        {
            ThisLiquid.transform.localPosition = Vector3.Lerp(ThisLiquid.transform.localPosition, toPos, 1 - count * 0.01f);
            decoration.localPosition = Vector3.Lerp(decoration.localPosition, DecoToPos, 1 - count * 0.01f);

            count--;
            yield return new WaitForSeconds(0.01f);
        }
        ThisLiquid.transform.localPosition = toPos;
        decoration.localPosition = DecoToPos;

        yield return null;

        if(dropNum == 0) ResetDrink();
        cupManager.moving = false;
        GameObject.Find("키친").transform.Find("Table").transform.Find("Trash").gameObject.GetComponent<BoxCollider2D>().enabled = true;

    }

    IEnumerator SizeUpLiquid()
    {
        float count = 90f;
        GameObject ThisLiquid = LiquidDiv[dropNum - 1].gameObject;

        //액체와 토핑 높이 보정
        Vector3 toPos = liquidPos[dropNum - 1];
        Vector3 DecoToPos = new Vector3(toPos.x, toPos.y, decoration.localPosition.z);

        while (ThisLiquid.transform.localPosition.y < toPos.y - 0.05f) 
        {
            ThisLiquid.transform.localPosition = Vector3.Lerp(ThisLiquid.transform.localPosition, toPos, 1 - count * 0.01f);
            decoration.localPosition = Vector3.Lerp(decoration.localPosition, DecoToPos, 1 - count * 0.01f);

            count--;
            yield return new WaitForSeconds(0.01f);
        }
        ThisLiquid.transform.localPosition = toPos;
        decoration.localPosition = DecoToPos;
        
        yield return new WaitForSeconds(0.01f);
        cupManager.moving = false;
    }
    //위 코루틴을 프레임 보정
    /*
    IEnumerator SizeUpLiquid()
    {
        GameObject ThisLiquid = LiquidDiv[dropNum - 1].gameObject;

        //액체와 토핑 높이 보정
        Vector3 toPos = liquidPos[dropNum - 1];
        Vector3 DecoToPos = new Vector3(toPos.x, toPos.y, decoration.localPosition.z);

        Vector3 startLiquidPos = ThisLiquid.transform.localPosition;
        Vector3 startDecoPos = decoration.localPosition;
        float elapsedTime = 0f;
        float duration = 0.3f; // 전체 애니메이션 지속 시간

        while (elapsedTime < duration)
        {
            // 경과 시간 비율 계산
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Ease Out 효과를 위한 비율 조절 (예: t * t * (3f - 2f * t))
            t = t * t * (3f - 2f * t);

            // 보간하여 위치 업데이트
            ThisLiquid.transform.localPosition = Vector3.Lerp(startLiquidPos, toPos, t);
            decoration.localPosition = Vector3.Lerp(startDecoPos, DecoToPos, t);

            yield return null;
        }

        // 최종 위치 확정
        ThisLiquid.transform.localPosition = toPos;
        decoration.localPosition = DecoToPos;
        cupManager.moving = false;
    }*/

    IEnumerator ChangeColor(SpriteRenderer spriteRenderer, Color _toColor)
    {
        float count = 50f;
        while (spriteRenderer.color != _toColor)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, _toColor, 1 - count * 0.01f);
            count--;
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    IEnumerator CoFadeIn(SpriteRenderer spriteRenderer, System.Action nextEvent = null)
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        Color tempColor = spriteRenderer.color;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / 0.3f;
            spriteRenderer.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return new WaitForSeconds(0.01f);
        }
        cupManager.moving = false;

        spriteRenderer.color = tempColor;
        if (nextEvent != null) nextEvent();
    }

    IEnumerator CoFadeInOut(SpriteRenderer spriteRenderer, System.Action nextEvent = null)
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        Color tempColor = spriteRenderer.color;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / 0.3f;
            spriteRenderer.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return new WaitForSeconds(0.01f);
        }
        spriteRenderer.color = new Color(1, 1, 1, 0.7f);
        tempColor = spriteRenderer.color;
        while (tempColor.a > 0)
        {
            tempColor.a -= Time.deltaTime / 0.3f;
            spriteRenderer.color = tempColor;

            if (tempColor.a < 0) tempColor.a = 0f;

            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("Exir");
        cupManager.moving = false;

        spriteRenderer.color = tempColor;
        if (nextEvent != null) nextEvent();
    }
}
