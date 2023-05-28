using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public bool change;
    public bool inCounter;
    public Vector3 originPos;
    public Vector3 toScene;
    public CameraScroll cameraScroll;

    private float distY;
    private float distZ;

    public void Start()
    {
        inCounter = true;
        originPos = transform.position;
        cameraScroll = GetComponent<CameraScroll>();
        distY = transform.position.y;  // Distance camera is above map
        distZ = transform.position.z;  // Distance camera is above map
    }
    
    public void CameraMoving(GameObject toObject)
    {
        //cameraScroll.enabled = inCounter ?  true : false;

        if (inCounter) {
            cameraScroll.enabled = false;
            transform.position = originPos;
        }
        else {
        }
        inCounter = !inCounter;

        change = true;
        Vector3 vector = new Vector3(0, 0, distZ);
        vector.x = toObject.gameObject.GetComponent<Transform>().position.x;
        vector.y = toObject.gameObject.GetComponent<Transform>().position.y;
        toScene = vector;

        //카운터 안이면 스크롤 활성화, 밖이면 스크롤 비활성화
    }
    void Update()
    {
        //카메라 빠르게 이동효과
        if (change)
        {
            transform.position = Vector3.Lerp(transform.position, toScene, 0.5f);
            if (transform.position == toScene)
            {
                change = false;
                if (inCounter) cameraScroll.enabled = true;
            }
        }
    }
}
