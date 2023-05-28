using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLock : MonoBehaviour
{
    // UI 창 떴을 때 스크롤이나 오브젝트 클릭 막기
    public void SetOn()
    {
        GameObject.Find("Main Camera").GetComponent<CameraScroll>().windowMoving = true;
        GameObject.Find("Main Camera").GetComponent<DragAndDrop>().enabled = false;
    }
    public void SetOff()
    {
        GameObject.Find("Main Camera").GetComponent<CameraScroll>().windowMoving = false;
        GameObject.Find("Main Camera").GetComponent<DragAndDrop>().enabled = true;

    }
}
