using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorySwap : MonoBehaviour
{
    public Color BtnColor_Sel;   //체크O 버튼 컬러
    public Color BtnColor;      //체크X 버튼 컬러
    public Color LineColor_Sel;  //체크O 라인 컬러
    public Color LineColor;     //체크X 라인 컬러
    public Color TxtColor_Sel;   //체크O 글자 컬러
    public Color TxtColor;      //체크X 글자 컬러

    /*
    public Sprite checkedSprite;    //체크O 버튼 이미지
    public Sprite unchecedkSprite;  //체크X 버튼 이미지
    */

    public void SelectBtn(Image selBtnImg)
    {
        foreach(Transform i in transform)
        {
            
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Image btnImage = transform.GetChild(i).GetComponent<Image>();
            btnImage.color = BtnColor;
            btnImage.gameObject.transform.Find("Text").GetComponent<Text>().color = TxtColor;
            btnImage.gameObject.transform.Find("Line").GetComponent<Image>().color = LineColor;
        }
        selBtnImg.color = BtnColor_Sel;
        selBtnImg.gameObject.transform.Find("Text").GetComponent<Text>().color = TxtColor_Sel;
        selBtnImg.gameObject.transform.Find("Line").GetComponent<Image>().color = LineColor_Sel;
    }

}
