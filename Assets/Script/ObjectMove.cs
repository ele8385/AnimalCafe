using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public float moveTime;
    private Vector3 originPos;
    private Vector3 toPos;
    public bool moving;

    public void ObjectMovingX(float x)
    {
        originPos = transform.position;
        toPos = new Vector3(x, transform.position.y, transform.position.z);
        StartCoroutine("moveBlockTime");
    }
    private IEnumerator moveBlockTime()
    {
        moving = true;

        float elapsedTime = 0.0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(originPos, toPos, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = toPos;

        moving = false;
    }
}
