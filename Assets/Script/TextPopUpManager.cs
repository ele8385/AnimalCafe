using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPopUpManager : MonoBehaviour
{
    public Text text;
    public Image bg;
    public RectTransform rect;
    public Sprite warning, notice, system;
    public Color warningColor, noticeColor, systemColor;

    public void OpenPopUp(string messege)
    {
        OpenPopUp(messege, rect.anchoredPosition.y, "warning");
    }

    public void OpenPopUp(string messege, float positionY)
    {
        OpenPopUp(messege, positionY, "warning");
    }
    public void OpenPopUp(string messege, float positionY, string type, string colorText) //메시지, 팝업 높이, 팝업 타입, 하이라이트 넣을 문자열
    {
        OpenPopUp(messege, positionY, type);
        
        text.color = warningColor;
        messege = messege.Replace(colorText, "<color=#f3c57c>" + colorText + "</color>");
        text.text = messege;
    }

    public void OpenPopUp(string messege, float positionY, string type)
    {
        gameObject.SetActive(true);
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, positionY);
        text.text = messege;
        StartCoroutine("OpenPopUpCo");

        if (type == "warning")
        {
            text.color = warningColor;
            bg.sprite = warning;
        }
        else if (type == "notice")
        {
            text.color = noticeColor;
            bg.sprite = notice;
        }
        else if (type == "system")
        {
            text.color = systemColor;
            bg.sprite = system;
        }
    }

    IEnumerator OpenPopUpCo()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

}
