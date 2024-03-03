using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    public GameObject OrderBallon;
    public DrinkManager OrderDrink;
    public GameObject SpeakBalloon;
    public GameObject Talk;
    public AnimalMovement anim;

    public bool dialog = false; //대화이벤트용 풍선인지

    // Start is called before the first frame update
    void Start()
    {
    }
    public void ResetBalloon()
    {
        OrderBallon.SetActive(false);
        SpeakBalloon.SetActive(false);
        Talk.SetActive(false);
        dialog = false;
    }

    //간단한 멘트
    public void OpenTalk(string _text)
    {
        Talk.SetActive(true);
        Talk.transform.Find("Text").gameObject.GetComponent<TextMeshPro>().text = _text;
    }
    //간단한 멘트
    public void CloseTalk()
    {
        Talk.SetActive(false);
        Talk.transform.Find("Text").gameObject.GetComponent<TextMeshPro>().text = "";
    }
    //대사
    public void OpenSpeakBalloon(string _text)
    {
        OrderBallon.SetActive(false);
        SpeakBalloon.SetActive(true);
        SpeakBalloon.transform.Find("Text").gameObject.GetComponent<TextMeshPro>().text = _text;
        //StartCoroutine(WaitClosingCo()); 이거왜있음

    }
    //대화 이벤트 시작
    public void OpenSpeakBalloon(string _text, bool _dialog)
    {
        dialog = _dialog;
        OpenSpeakBalloon(_text);
    }
    //주문 말풍선
    public void OpenOrderBalloon(bool open)
    {
        OrderBallon.SetActive(open);
        OrderBallon.transform.Find("Text").gameObject.GetComponent<TextMeshPro>().text = anim.AnimalData.hello;
    }
    //대화 이벤트 풍선 클릭하면 이벤트 대화
    public void ClickSpeakBalloon()
    {
        if (!dialog) return;
        GameObject.Find("Canvas").transform.Find("Dialogue_window").GetComponent<DialogueManager>().OpenDialogue(anim);
        ResetBalloon();
    }
    ////주문 말풍선 클릭하면 주문 대화
    public void ClickOrderBalloon()
    {
        if (dialog) return;
        GameObject.Find("Canvas").transform.Find("Dialogue_window").GetComponent<DialogueManager>().OpenDialogue(anim, true); 
        ResetBalloon();
    }

    IEnumerator WaitClosingCo() {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
