using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    private float distY;
    private float distZ;
    private Vector3 MouseStart;
    private Vector3 derp;

    public float maxLeft;
    public float maxRight;

    public float deviceRatio;

    public bool windowMoving;

    void Start()
    {
        distY = transform.position.y;  // Distance camera is above map
        distZ = transform.position.z;  // Distance camera is above map
        
        //기기 비율에 따른 좌우 스크롤 제한 수치 설정
        deviceRatio = (float)Screen.width / Screen.height;// 기기 비율 저장(9:20 = 0.45, 9:16 = 0.56)
        if (deviceRatio > 9 / 18f) deviceRatio = 9 / 18f; else if (deviceRatio < 9 / 20f) deviceRatio = 9 / 20f;
        maxLeft = maxLeft - ( 12f * (9 / 18f - deviceRatio));
        maxRight = maxRight + ( 12f * (9 / 18f - deviceRatio));
    }

    void Update()
    {
        if (! windowMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MouseStart = new Vector3(Input.mousePosition.x, distY, distZ);
                MouseStart = Camera.main.ScreenToWorldPoint(MouseStart);
                MouseStart.y = transform.position.y;
                MouseStart.z = transform.position.z;

            }
            else if (Input.GetMouseButton(0))
            {
                var MouseMove = new Vector3(Input.mousePosition.x, distY, distZ);
                MouseMove = Camera.main.ScreenToWorldPoint(MouseMove);
                MouseMove.y = transform.position.y;
                MouseMove.z = transform.position.z;
                transform.position = transform.position - (MouseMove - MouseStart);
            }
            
            if (transform.position.x < maxLeft)
            {
                var MouseMove = new Vector3(maxLeft, distY, distZ);
                MouseMove.y = transform.position.y;
                MouseMove.z = transform.position.z;
                transform.position = new Vector3(maxLeft, distY, distZ);
            }
            if (transform.position.x > maxRight)
            {
                var MouseMove = new Vector3(maxRight, distY, distZ);
                MouseMove.y = transform.position.y;
                MouseMove.z = transform.position.z;
                transform.position = new Vector3(maxRight, distY, distZ);
            }
        }
       
    }
}