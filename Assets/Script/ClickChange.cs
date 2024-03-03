using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickChange : MonoBehaviour {

    public SpriteRenderer spriteRenderer;
    public Sprite orignImg;
    public Sprite changeImge;
    public Sprite changeImge2;


	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        orignImg = spriteRenderer.sprite;
    }
    public void ChangeImg()
    {
        if(changeImge != null)
        spriteRenderer.sprite = changeImge;
    }
    public void Change2Img()
    {
        if (changeImge2 != null)
            spriteRenderer.sprite = changeImge2;
    }
    public void GetbackImg()
    {
        spriteRenderer.sprite = orignImg;
    }
}
