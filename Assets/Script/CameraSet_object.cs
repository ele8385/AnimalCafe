using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet_object : MonoBehaviour {

    public float cameraSize;

    // Use this for initialization
    void Start()
    {
        cameraSize = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize;

        Vector3 vector = transform.position;
        vector.y = transform.position.y * (cameraSize / 12f) ;
        transform.position = vector;
    }
}
