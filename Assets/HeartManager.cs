using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    public GameObject heartImg;
    void AddHeart()
    {
        gameObject.SetActive(true);
        /*
        //금액 텍스트 넣고 올라가는 효과
        text.GetComponent<TextMesh>().text = "+" + string.Format("{0:#,0}", _val);
        TextUp();


        //금액증가
        State.instance.PlusMoney(_val);*/
    }
}
