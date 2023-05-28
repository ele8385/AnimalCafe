using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickChange : MonoBehaviour {

    public SpriteRenderer spriteRenderer;
    public Sprite orignImg;
    public Sprite changeImge;


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
    public void GetbackImg()
    {
        spriteRenderer.sprite = orignImg;
    }
}
