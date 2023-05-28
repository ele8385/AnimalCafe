using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEvent : MonoBehaviour
{
    //말풍선 누르는 동안 주문창 열기..
    public GameObject LinkObject;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetOn()
    {
        if (LinkObject != null) LinkObject.SetActive(true);
        GameObject.Find("Main Camera").GetComponent<CameraScroll>().enabled = false;

    }
    public void SetOff()
    {
        if (LinkObject != null) LinkObject.SetActive(false);
        GameObject.Find("Main Camera").GetComponent<CameraScroll>().enabled = true;

    }
}
