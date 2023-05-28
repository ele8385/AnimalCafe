using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPopUpManager : MonoBehaviour
{
    public Text text;
    public RectTransform rect;

    public void OpenPopUp(string messege)
    {
        Debug.Log("aa");

        OpenPopUp(messege, 0);
        Debug.Log("nn");

    }

    public void OpenPopUp(string messege, float positionY)
    {
        gameObject.SetActive(true);
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, positionY);
        text.text = messege;
        StartCoroutine("OpenPopUpCo");

    }

    IEnumerator OpenPopUpCo()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
