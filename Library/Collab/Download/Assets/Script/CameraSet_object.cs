using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet_object : MonoBehaviour {

    public CameraSetting cameraSetting;
    public float screenR;

	// Use this for initialization
	void Start () {
        cameraSetting = GameObject.Find("Main Camera").gameObject.GetComponent<CameraSetting>();
        screenR = cameraSetting.screenR;
        if (screenR > 0.7f)
        {
            Vector3 vector = transform.position;
            vector.y = transform.position.y +0.1f;
            transform.position = vector;
            //Debug.Log(screenR + "아이패드 등 완전 낮은화면");

        }
        else if (screenR > 0.510f)
        {
            /*
            Vector3 vector = transform.position;
            vector.x = transform.position.x - 10f;
            transform.position = vector;
            */
            //Debug.Log(screenR + "9:16 정도 낮은화면");
        }
        else
        {
            Vector3 vector = transform.position;
            vector.y = transform.position.y -1.6f;
            transform.position = vector;
            //Debug.Log(screenR + "9:18 정도 높은화면");
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
