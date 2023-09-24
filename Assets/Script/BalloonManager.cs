using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    public GameObject OrderBallon;
    public DrinkManager OrderDrink;
    public GameObject SpeakBalloon;

    public bool dialog = false; //대화이벤트용 풍선인지

    // Start is called before the first frame update
    void Start()
    {
    }
    public void ResetBalloon()
    {
        OrderBallon.SetActive(false);
        SpeakBalloon.SetActive(false);
        dialog = false;
    }
    //대사
    public void OpenSpeakBalloon(string _text)
    {
        OrderBallon.SetActive(false);
        SpeakBalloon.SetActive(true);
        SpeakBalloon.transform.Find("Text").gameObject.GetComponent<TextMesh>().text = _text;
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
    }
    //대화 이벤트 풍선 클릭
    public void ClickSpeakBalloon()
    {
        if (!dialog) return;
        AnimalMovement anim = GetComponent<AnimalMovement>();
        GameObject.Find("Canvas").transform.Find("Dialogue_window").GetComponent<DialogueManager>().OpenDialogue(anim.AnimalData.code, anim);
        ResetBalloon();
    }
    public void ClickOrderBalloon()
    {
        if (dialog) return;
        AnimalMovement anim = GetComponent<AnimalMovement>();
        GameObject.Find("Canvas").transform.Find("Dialogue_window").GetComponent<DialogueManager>().OpenDialogue(anim.AnimalData.code, anim, true);
        ResetBalloon();
    }

    IEnumerator WaitClosingCo() {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
