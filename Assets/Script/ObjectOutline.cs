using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOutline : MonoBehaviour
{
    public GameObject target;
    public float offset;
    public bool startSet = false; //시작할 때 호출해야 하는지

    public void Start()
    {
        if (startSet) SetOutline();
    }

    public void SetOutlineDrink()
    {
        //음료 크기가 0.8보다 작으면 컵 이미지 뒤에 상하좌우로 붙여서 아웃라인 있어보이게

        if (transform.parent.transform.parent.transform.localScale.x < 0.8f) 
        SetOutline();
    }
    

    public void SetOutline()
    {
        Vector3 temp = Vector3.zero;
        temp.z += 0.1f;
        gameObject.transform.localPosition = temp;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform obj = gameObject.transform.GetChild(i).gameObject.transform;

            if (obj.gameObject.GetComponent<SpriteRenderer>())
            obj.gameObject.GetComponent<SpriteRenderer>().sprite = target.GetComponent<SpriteRenderer>().sprite;

            if (obj.gameObject.GetComponent<TextMesh>())
                obj.gameObject.GetComponent<TextMesh>().text = target.GetComponent<TextMesh>().text;

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
