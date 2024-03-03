using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class DialogueManager : MonoBehaviour
{
    public Text animalName;
    public Text context;
    public Image animalImg;
    public GameObject btns;
    public AnimalMovement ThisAnimal;
    public Button btn1, btn2, btn3, btn4;
    public AnimalData AnimalData;

    public int lineNum = 0;

    public Script script;
    public string printLine;
    public bool ordering; //주문 창 오픈 중
    public bool typing;
    public bool endWaitng; //텍스트 모두 출력 후 클릭 가능할 때까지 잠깐의 기다림
    public bool selecting; //선택지 선택 완료
    public List<string> lines = new List<string>();

    IEnumerator enumerator;

    private void Start()
    {

    }

    //주문하는 대화
    public void OpenDialogue(AnimalMovement animalMovement, bool _ordering)
    {
        ordering = _ordering;
        OpenDialogue(animalMovement);
    }

    //대화창 열기
    public void OpenDialogue(AnimalMovement animalMovement)
    {
        GameObject.Find("Canvas").gameObject.GetComponent<TouchLock>().SetOn(); //스크롤터치잠금
        gameObject.SetActive(true);

        AnimalData = Database.instance.GetAnimalData(animalMovement.AnimalData.code);

        //처음 만난 동물이면 myAnimals에 추가
        Animal animal = State.instance.GetMyAnimal(AnimalData.code);
        if (animal == null) {
            animal = State.instance.AddMyAnimal(AnimalData.code);
        } 

        ThisAnimal = animalMovement;
        animalName.text = AnimalData.name;
        animalImg.sprite = animalMovement.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite;

        //주문 대사
        if (ordering) PrintOrder();
        //이벤트 대사
        else
        {
            try
            {
                //line(대사)에 나눠쓰기 기호($)있으면 문자열 분할하여 리스트로 저장
                string filename = System.Text.RegularExpressions.Regex.Replace(AnimalData.name, @"\d", "");
                script = Database.instance.GetDial(filename).scripts[animal.dialNum];
                lines = script.line?.Split('$').ToList();
            }
            catch (DivideByZeroException err)
            {
                Debug.Log("OpenDialogue 오류: " + err);
            }
            PrintDialogue();
        }
    }

    public void PrintOrder()
    {
        ordering = true;
        string orderName = ThisAnimal.recipe.name;
        context.text = orderName + "줄래";
    }

    //대화창 클릭 시
    public void ClickDialog()
    {
        if (ordering) CloseDialogue(); //주문 중이면 닫기
        else PrintDialogue(); //대화 중이면 대사 출력
    }

    //대사 출력
    public void PrintDialogue()
    {
        if (endWaitng) return;

        try
        {
            if (typing) //타이핑 중에 클릭하면 타이핑효과 끝내고 텍스트 모두 출력
            {
                StartCoroutine("StopText");
            }
            else //처음이나 타이핑 끝나고 클릭하면 다음 텍스트 출력
            {
                //대사 하나씩 출력
                if (lineNum < lines.Count)
                {
                    printLine = lines[lineNum];
                    enumerator = PrintText();
                    StartCoroutine(enumerator);
                }
                //대사 끝나고 난 후
                else
                {
                    if (selecting) { CloseDialogue(); return; } //대화 끝까지 했으면 종료
                    OpenSelectBtn();
                }
            }
        }
        catch(DivideByZeroException err)
        {
            Debug.Log("PrintDialogue() 오류" + err);
        }

    }

    //대답버튼 클릭. 버튼 오브젝트에 이벤트함수 할당
    public void ClickSelectBtn(int selectNum)
    {
        //버튼 접기
        for (int i = 0; i < btns.transform.childCount; i++)
        {
            btns.transform.GetChild(i).gameObject.SetActive(false);
        }
        selecting = true;

        State.instance.AddHeart(AnimalData.code, script.answers[selectNum].heart);
        State.instance.AddDialNum(AnimalData.code, script.answers[selectNum].nextLine);

        printLine = script.answers[selectNum].text;
        enumerator = PrintText();
        StartCoroutine(enumerator);
    }

    //대답버튼 열기
    public void OpenSelectBtn()
    {
        for (int i = 0; i < script.answers.Count; i++)
        {
            //버튼 할당 (선택지버튼는 4개까지만 가능)
            GameObject btn = btns.transform.Find("Button" + i).gameObject;
            btn.SetActive(true);
            //버튼 텍스트 할당
            btn.transform.Find("Text").GetComponent<Text>().text = script.answers[i].select; 
        }
    }

    public void CloseDialogue()
    {
        GameObject.Find("Canvas").gameObject.GetComponent<TouchLock>().SetOff(); //스크롤터치잠금

        lineNum = 0;
        selecting = false;
        ThisAnimal.StartOrder();
        gameObject.SetActive(false);
        ordering = false;
    }

    IEnumerator StopText()  
    {
        endWaitng = true;
        StopCoroutine(enumerator);
        context.text = printLine;
        yield return new WaitForSeconds(0.4f);
        endWaitng = false;
        typing = false;
        lineNum++;
        AudioManager.instance.StopSFX("Typing");

    }

    IEnumerator PrintText()
    {
        typing = true;
        AudioManager.instance.PlaySFX("Typing");

        //글자 하나씩
        for (int j = 0; j < printLine.Length; j++)
        {
            context.text = printLine.Substring(0, j + 1);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.4f);
        typing = false;
        lineNum++;
        AudioManager.instance.StopSFX("Typing");
    }
    /*
  

    IEnumerator ShowText(string[] _fullText)
    {
        //모든텍스트 종료
        if (cnt >= dialog_cnt)
        {
            text_exit = true;
            StopCoroutine("showText");
        }
        else
        {
            //기존문구clear
            currentText = "";
            //타이핑 시작
            for (int i = 0; i < _fullText[cnt].Length; i++)
            {
                //타이핑중도탈출
                if (text_cut == true)
                {
                    break;
                }
                //단어하나씩출력
                currentText = _fullText[cnt].Substring(0, i + 1);
                this.GetComponent<Text>().text = currentText;
                yield return new WaitForSeconds(delay);
            }
            //탈출시 모든 문자출력
            Debug.Log("Typing 종료");
            this.GetComponent<Text>().text = _fullText[cnt];
            yield return new WaitForSeconds(Skip_delay);

            //스킵_지연후 종료
            Debug.Log("Enter 대기");
            text_full = true;
        }
    }*/
    
}
