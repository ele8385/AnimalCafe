using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour {
    public float screenR;
    public float val;
    void Awake()
    {
        screenR = (float)Screen.width / (float)Screen.height;
        //screenR = w / h;
        if (screenR > 0.7f)
        {
            if (gameObject.name == "Camera")
                GetComponent<Camera>().orthographicSize = 1f;
            else if (gameObject.name == "Main Camera")
                GetComponent<Camera>().orthographicSize = 10f;
            //Debug.Log(screenR + "아이패드 등 완전 낮은화면");

        }
        else if (screenR > 0.510f)
        {
            if (gameObject.name == "Camera")
                GetComponent<Camera>().orthographicSize = 1f;
            else if (gameObject.name == "Main Camera")
                GetComponent<Camera>().orthographicSize = 10f;
            //Debug.Log(screenR + "9:16 정도 낮은화면");
        }
        else
        {
            if (gameObject.name == "Camera")
                GetComponent<Camera>().orthographicSize = (screenR - val) * -2.189f + 1f;
            else if (gameObject.name == "Main Camera")
                GetComponent<Camera>().orthographicSize = (screenR - val) * -18.905f + 8.7f;
            //Debug.Log(screenR + "9:18 정도 높은화면");
        }
    }
}
