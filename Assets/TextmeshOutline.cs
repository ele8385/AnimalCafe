using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextmeshOutline : MonoBehaviour
{
    public float offset;
    public Color textColor;
    public bool startSet = false; //시작할 때 호출해야 하는지

    public void Start()
    {
    }
    
    public void SetOutline()
    {
        GameObject target = transform.parent.gameObject;
        Vector3 temp = Vector3.zero;
        temp.z += 0.1f;
        gameObject.transform.localPosition = temp;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform obj = gameObject.transform.GetChild(i).gameObject.transform;

            TextMesh textMesh = obj.gameObject.GetComponent<TextMesh>();
            textMesh.text = target.GetComponent<TextMesh>().text;
            textMesh.color = textColor;

            obj.localPosition = Vector3.zero;

            if (i < 3) obj.localPosition = new Vector3(offset, obj.localPosition.y, obj.localPosition.z);
            else if (i < 6) obj.localPosition = new Vector3(-offset, obj.localPosition.y, obj.localPosition.z);
            else obj.localPosition = new Vector3(0, obj.localPosition.y, obj.localPosition.z);

            if (i % 3 == 0) obj.localPosition = new Vector3(obj.localPosition.x, offset, obj.localPosition.z);
            else if (i % 3 == 1 % 3) obj.localPosition = new Vector3(obj.localPosition.x, -offset, obj.localPosition.z);
            else obj.localPosition = new Vector3(obj.localPosition.x, 0, obj.localPosition.z);
        }
    }
}
