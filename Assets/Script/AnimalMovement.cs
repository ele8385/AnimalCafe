using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class AnimalMovement : MonoBehaviour {

    // Use this for initialization
    public bool enter; //매장 내에 입장했는지
    public Recipe recipe; //주문할 음료
    public int TableCount;
    public string syrup; //컨디션시럽 "피로회복" 등
    public int waitNum;
    public int indexChair;
    public int counterLine;
    public int exitRoute; //0:문으로 1:테이블로 2:구경 (애니메이터 프리메터)
    public float TableWaitTime = 7f;
    public float moveSpeed;
    public bool isBalloon;
    public GameObject Talk;
    public GameObject Money;
    public GameObject Heart;
    public GameObject CountPos;
    public GameObject AnimalManagement;
    public delegate void Func();
    public delegate void StringFunc(string a);

    public OrderPapersManager orderPapers;
    public BalloonManager balloonManager;
    public HeartManager heartManager;
    public CoinManager coinManager;  //머리 위에 금액 올라감 표시
    public SeatManager seat;  //테이블 좌석
    public AnimalData AnimalData;
    public Animator animator;
    public Animator moveAnimator;
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D circleCollider;

    Coroutine runningCoroutine = null;

    public string filename; //임시네임

    public Vector3 moveToPos;
    
    public Vector3 tempCounterPos;
    public Vector3 counterPos;
    public Vector3 tablePos;

    public Color heartColor = new Color(255 / 255f, 117 / 255f, 110 / 255f);
    public Color defaltColor = new Color(123 / 255f, 88 / 255f, 69 / 255f);
    

	void Start () {
        moveAnimator     = GetComponent<Animator>();
        circleCollider   = GetComponent<CircleCollider2D>();
        
        CountPos         = GameObject.Find("Counter").transform.Find("CountPos").gameObject;
        orderPapers      = GameObject.Find("Canvas").transform.Find("InKitchenUI").transform.Find("OrderPapers").gameObject.GetComponent<OrderPapersManager>();
        AnimalManagement = GameObject.Find("AnimalManagement");
        
        tempCounterPos   = GameObject.Find("TempCounterPos").transform.position;
        counterLine      = 3; //카운터 앞에 서는 동물 수 
        TableCount = GameObject.Find("카운터1").transform.Find("Table").transform.childCount; //테이블 개수
        moveSpeed = 3f;

        ResetAnimal();
    }

    //카운터 앞까지 와서 어디 줄에 설지 결정하고 이동
    public void GoToCounter()  //애니메이터 이벤트로 실행
    {
        //카운터 자리 있음
        if (CounterPosSet())
        {
            moveAnimator.enabled = false; //애니메이터 멈추고 카운터자리로 코루틴이동
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }
            runningCoroutine = StartCoroutine(MoveTo("카운터대기")); //코루틴을 시작하며, 동시에 저장한다.         
        }
        //카운터 자리 업슴
        else
        {
            moveAnimator.SetBool("counterFull", true);
        }
    }

    //카운터 줄 빈 자리 있으면 true, 꽉 찼으면 false
    public bool CounterPosSet()
    {
        for (int i = 0; i < CountPos.transform.childCount; i++)
        {
            //카운터 줄 빈 자리 체크
            SeatManager _seatManager = CountPos.transform.GetChild(i).gameObject.GetComponent<SeatManager>();
            if (! _seatManager.full)
            {
                _seatManager.full = true;
                waitNum = i;
                int line = i / counterLine;
                counterPos = new Vector3(_seatManager.gameObject.transform.position.x,
                                         _seatManager.gameObject.transform.position.y,
                                         _seatManager.gameObject.transform.position.z + (i * 0.1f));
                moveToPos = counterPos;
                return true;
            }
        }
        return false;
    }

    public void ExitTable()
    {
        DrinkManager tableDrink = seat.gameObject.transform.Find("TableDrink").gameObject.GetComponent<DrinkManager>();
        tableDrink.gameObject.SetActive(false);
        TableMoney tableManager = seat.gameObject.transform.Find("TableMoney").gameObject.GetComponent<TableMoney>();
        tableManager.MakeTableMoney("coin", 200); //팁 생성

        Walk();
    }

    //<AnimalManager>에서 호출. 동물 캐스팅 후 등장시킴
    public void Come(AnimalData castAnimal)
    {
        AnimalData = castAnimal;
        enter = true;
        SetAnimal();
        //등장 시작
        moveAnimator.enabled = true;
        moveAnimator.SetTrigger("Restart");
        moveAnimator.Play("GoToCounter");

    }

    //동물리셋
    public void ResetAnimal()
    {
        transform.parent.gameObject.GetComponent<AnimalManager>().RemoveAnimal(AnimalData.code); //현재 매장 내 동물 리스트에서 삭제
        Money.SetActive(false);
        Heart.SetActive(false);
        Talk.SetActive(false);
        balloonManager.ResetBalloon();
        Walk(); //서있기
        moveAnimator.SetBool("counterFull", false);
        moveAnimator.enabled = false;
        moveAnimator.Rebind();
        waitNum = -1;//카운터줄초기화
        enter = false;
        syrup = "";
    }

    public void Walk()
    {
        animator.SetBool("isSitting", false);
        animator.SetBool("isWalking", true);
    }
    //정지상태
    public void Stand()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isSitting", false);
    }

    public void Sit()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isSitting", true);
        
        DrinkManager tableDrink = seat.gameObject.transform.GetChild(0).gameObject.GetComponent<DrinkManager>();
        tableDrink.gameObject.SetActive(true);
        tableDrink.MakeDrink(recipe.drink);
        
        StartCoroutine(WaitCo(SitTalking, 2f));
        //spriteRenderer.sprite = Resources.Load<Sprite>("Character/" + AnimalData.name);
    }

    //캐스팅된 동물 셋팅
    private void SetAnimal()
    {
        filename = System.Text.RegularExpressions.Regex.Replace(AnimalData.name, @"\d", "");
        //스프라이트와 애니메이터 할당
        spriteRenderer.sprite = Resources.Load<Sprite>("Character/" + filename);
        animator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Animator/Animal_" + filename, typeof(RuntimeAnimatorController))); ;

        //랜덤 음료배정
        if (AnimalData.recipe == null) { recipe = State.instance.GetRandomRecipe(); }
        else { recipe = AnimalData.recipe; }
        /*
        int a = Random.Range(0, 4); 
        if (a > 1) recipe = AnimalData.recipe; //75%의 확률로 최애음료 배정
        else//25%의 확률로 랜덤음료 배정
        {
            // 해당 음료 오픈되었는지 여부
            recipe = State.instance.GetRandomRecipe();
        }*/
        balloonManager.OrderDrink.MakeDrink(recipe.drink);
    }

    //이동
    public void Move(Vector3 _moveToPos)
    {
        moveToPos = _moveToPos;
        transform.position = Vector3.MoveTowards(transform.position, moveToPos, moveSpeed * Time.deltaTime);

        Walk();

        if (transform.position == moveToPos)
        {
            //SetMoveStep();
        }
        
        //circleCollider.enabled = false;
    }


    //대기상태(풍선 켜짐)
    public void StandCounter()
    {
        Stand();
        if (waitNum < counterLine)
        {

            //대화 이벤트 시작
            if (State.instance.GetBoolEvent(AnimalData.code))
            {
                balloonManager.OpenSpeakBalloon(AnimalData.hello, true);
            }
            //주문 시작
            else
            {
                StartOrder();
            }
        }
    }

    public void StartOrder()
    {
        State.instance.AddOrderCount(AnimalData.code); //주문 수 증가

        if (Camera.main.GetComponent<CameraMovement>().inCounter) //카운터 화면이면 주문풍선 켬
            balloonManager.OpenOrderBalloon(true);

        circleCollider.enabled = true;
        orderPapers.MakeOrderPaper(recipe, waitNum % counterLine, this); //주문서 열림
        CountPos.transform.GetChild(waitNum).gameObject.GetComponent<SeatManager>().animal = this; //카운터 자리에 동물정보 넣기
    }

    //255배 해서 비교하는 이유: <Color>값 그대로 넣으면 소숫점 차이로 비교가 어긋나서 255를 곱한 뒤에 비교함.
    public List<Color> ConvertColors(List<Color> _colors)
    {
        List<Color> convertColors = new List<Color>();
        for (int j = 0; j < _colors.Count; j++)
        {
            float rr = Mathf.Round(_colors[j].r * 255);
            float gg = Mathf.Round(_colors[j].g * 255);
            float bb = Mathf.Round(_colors[j].b * 255);
            convertColors.Add(new Color(rr, gg, bb));
        }
        return convertColors;
    }

    //3: 전달 실패
    //2: 레시피 perfect
    //1: 레시피 ok
    //0: 색상 miss
    //-1: 높이 miss
    //-2: 토핑 miss
    //-3: 휘핑 miss
    //-4: 컵 miss
    //-5: 그라디언트 miss

    public int GiveDrink(Drink _takeDrink) //result를 반환
    {
        //맨 앞줄이 아닌가
        if (transform.position != counterPos || waitNum > counterLine) return 3;
        //대화 이벤트 진행 중인가
        if (balloonManager.SpeakBalloon.activeSelf == true) return 3;

        circleCollider.enabled = true;

        int result = CompareDrink(_takeDrink); // 0: miss 1:ok 2:perfect
        
        if (result  == -2) balloonManager.OpenSpeakBalloon("뭔가 잘못 들어갔어.");
        else if (result  == -3) balloonManager.OpenSpeakBalloon("휘핑이 이게 아니야.");
        else if (result == 0)//miss
        {
            balloonManager.OpenSpeakBalloon(AnimalData.script_bad); 
        }
        else
        {
            State.instance.AddBuyCount(AnimalData.code);//판매 수 증가
            //OK
            if (result == 1)  {
                balloonManager.OpenSpeakBalloon(AnimalData.script_good); // 추후 퍼펙트 대사 수정
                coinManager.AddMoney("coin", recipe.price / 2); //수익 증가

                State.instance.AddHeart(AnimalData.code, 1);
            }
            //perfect
            else if (result == 2)
            {
                balloonManager.OpenSpeakBalloon(AnimalData.script_good); // 추후 퍼펙트 대사 수정
                coinManager.AddMoney("coin", recipe.price); //수익 증가
                State.instance.AddHeart(AnimalData.code, 2);

            }
            else
            {
            }
            //레시피 등록 성공
            if (State.instance.AddRecipe(recipe))
            {
                //성공 이펙트
            }
        }

        StartCoroutine(WaitCo(balloonManager.ResetBalloon, 2f)); //2초 뒤에 말풍선OFF
        StartCoroutine(WaitCo(TakeOut, 3f)); //3초 뒤에 이동 시작

        return result;
    }

    //컬러 두 가지를 비교(range만큼의 허용치)하여 일치하면 true
    public bool CompareColor(float range, Color order, Color take)
    {
        if (order.r + range < take.r || order.r - range > take.r ||
            order.g + range < take.g || order.g - range > take.g ||
            order.b + range < take.b || order.b - range > take.b)
        {
            return false;
        }
        return true;
    }

    //컬러리스트 두 가지를 비교
    public int CompareDrink(Drink _takeDrink)
    {
        //주문한 음료의 색상리스트 변환
        List<Color> order = ConvertColors(recipe.drink.colors);

        //받은 음료의 색상리스트 변환
        List<Color> take = ConvertColors(_takeDrink.colors);

        bool custom = false; //임시변수. recipe로 넣기

        //커스텀음료가 아닐 경우
        if (!custom)
        {
            if (_takeDrink.topping != recipe.drink.topping) return -2;
            if (_takeDrink.whipping != recipe.drink.whipping) return -3;
            if (_takeDrink.cupNum!= recipe.drink.cupNum) return -4;
            if (! _takeDrink.gradient.SequenceEqual(recipe.drink.gradient)) return -5;
            for (int i = 0; i < take.Count; i++)
            {
                if (!CompareColor(50f, order[i], take[i])) return 0;
            }
            if (take.SequenceEqual(order)) return 2;
            return 1;
        }
        else
        {
            if (recipe.name == "desert")
            {
                CompareColor(70f, order[0], take[0]); //"desert"라는 커스텀음료의 경우 음료 색상의 첫 번째만 맞혀도 성공
                return 2;
            }
            else if (recipe.name == "carrot")
            {
                CompareColor(70f, order[0], take[0]);
                return 2;
            }
        }
        return 0;
    }

    public void TakeOut() //음료 받고 코멘트 날린 뒤 이동할 때 호출
    {
        //balloonManager.ResetBalloon(); //말풍선 끄기

        exitRoute = Random.Range(-1, 3);
        //-1: 문으로, 0~n: n번 테이블로, -2: 구경

        if (exitRoute == -1)
        {
            //테이크아웃이면 문으로
        }
        else if (exitRoute >= 0)
        {
            indexChair = Random.Range(0, 2);
            seat = GameObject.Find("카운터1").transform.Find("Table").GetChild(exitRoute).
                transform.GetChild(indexChair).GetComponent<SeatManager>();

            //테이블 만석이면 퇴장
            if (seat.full) { exitRoute = -1; }
            //테이블 공석 맞으면 테이블로
            else seat.full = true; //tablePos = new Vector3(seat.transform.position.x, seat.transform.position.y + 1, seat.transform.position.z - 0.1f
        }


        //뒷자리 동물있으면 앞줄로 이동
        UpdateWaitNum();

        moveToPos = tempCounterPos;
        StartCoroutine(MoveTo("경로설정"));
    }

    //애니메이션에서 실행
    public void Exit()
    {
        ResetAnimal();
    }

    //음료 받고 어디로 갈지 설정
    public void GoToRoute()
    {
        moveAnimator.enabled = true;
        moveAnimator.SetTrigger("SetRoute");
        moveAnimator.SetInteger("route", exitRoute);
        moveAnimator.Play("GoToDoor");

        if (indexChair == 0) moveAnimator.SetBool("Right", false);
        else moveAnimator.SetBool("Right", true);
        Walk();
    }

    //뒷자리 동물 줄땡김
    public void UpdateWaitNum()
    {
        //카운터 서 있는 자리 비움
        CountPos.transform.GetChild(waitNum).gameObject.GetComponent<SeatManager>().full = false;
        CountPos.transform.GetChild(waitNum).gameObject.GetComponent<SeatManager>().animal = null;


        //뒷자리 동물 컴포넌트 할당
        for (int i = 0; i < AnimalManagement.transform.childCount / counterLine; i++)
        {
            int backWaitNum = counterLine * (i + 1) + waitNum;
            
            for (int j = 0; j < AnimalManagement.transform.childCount; j++)
            {
                AnimalMovement backAnimal = AnimalManagement.transform.GetChild(j).gameObject.GetComponent<AnimalMovement>();

                if (backAnimal.waitNum == backWaitNum)
                {
                    CountPos.transform.GetChild(backWaitNum).gameObject.GetComponent<SeatManager>().full = false;
                    backAnimal.GoToCounter();
                }
            }
        }
    }
    public void SitTalking()
    {
        Talk.SetActive(true);
        TextEdit(Talk, "맛있군");
        StartCoroutine(WaitCo(SitTalkingStop, 2f));
    }
    public void SitTalkingStop()
    {
        Talk.SetActive(false);
        TextEdit(Talk, "");
    }
    
    //해당 색상으로 해당 글자를 오브젝트의 텍스트메쉬에 집어넣음
    public void TextEdit(GameObject Object, string val)
    {
        GameObject TextObject = Object.transform.Find("Text").gameObject;
        
        for (int i = 0; i < TextObject.transform.childCount; i++)
        {
            TextObject.transform.GetChild(i).GetComponent<TextMesh>().text = val;
        }

    }

    //sec초 뒤에 func함수 실행
    IEnumerator WaitCo(Func func, float sec)
    {
        yield return new WaitForSeconds(sec);
        func();
    }
    IEnumerator WaitStrCo(StringFunc func, float sec, string str)
    {
        yield return new WaitForSeconds(sec);
        func(str);
    }
    /*
    //문에서등장
    IEnumerator ApearCo()
    {
        yield return new WaitForSeconds(apearTime * (animalNum + 1));
        moveAnimator.enabled = true;
        moveAnimator.SetTrigger("Restart");
    }*/

    IEnumerator MoveTo(string goal)
    {
        Walk();
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToPos, moveSpeed * Time.deltaTime);

            if (transform.position == moveToPos)
            {
                animator.SetBool("isWalking", false);
                spriteRenderer.sprite = Resources.Load<Sprite>("Character/" + filename);
                
                break;
            }
            yield return null;
        }

        //카운터 도착했으면 0.5초 후에 주문 시작
        if(goal == "카운터대기")
        {
            yield return new WaitForSeconds(0.5f);
            StandCounter();
        }
        else if(goal == "경로설정")
        {
            GoToRoute();
        }
    }
    /*
     * 
    public void FadeIn(float fadeOutTime, System.Action nextEvent = null)
    {
        StartCoroutine(CoFadeIn(fadeOutTime, nextEvent));
    }
    public void FadeOut(GameObject _Object, float fadeOutTime, System.Action nextEvent = null)
    {
        StartCoroutine(CoFadeOut(_Object, fadeOutTime, nextEvent));
    }
    IEnumerator CoFadeIn(float fadeOutTime, System.Action nextEvent = null)
    {
        Color tempColor = spriteRenderer.color;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeOutTime;
            spriteRenderer.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }

        spriteRenderer.color = tempColor;
        if (nextEvent != null) nextEvent();
    }
    IEnumerator CoFadeOut(GameObject target, float fadeOutTime, System.Action nextEvent = null)
    {
        SpriteRenderer sr = target.gameObject.GetComponent<SpriteRenderer>();
        Color tempColor = sr.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeOutTime;
            sr.color = tempColor;

            if (tempColor.a <= 0f) tempColor.a = 0f;

            yield return null;
        }
        sr.color = tempColor;
        if (nextEvent != null) nextEvent();
    }

    
    IEnumerator ApearCo()
    {
        yield return new WaitForSeconds(apearTime * (animalNum + 1));

        //FadeIn(1f);                                       //문에서등장
        moveAnimator.enabled = true;
        moveAnimator.SetTrigger("Restart");
        transform.position = doorPos;
        yield return new WaitForSeconds(3f);
        animator.SetBool("isWalking", true);
        //SetMoveStep("문옆이동");
    }
    IEnumerator DisApearCo()
    {
        FadeOut(gameObject, 1f);                                       //문에서퇴장/ 애니메이션으로 바꿈
        yield return new WaitForSeconds(1f);
        transform.position = outPos;
        SetMoveStep("리셋");
    }*/
}
