using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkManager : MonoBehaviour {

    public bool orign; //true - 원본컵 / false = Thum컵
    public Drink drink;
    public GameObject Liquid;
    public List<SpriteRenderer> LiquidDiv;
    public ObjectOutline outline;
    public SpriteRenderer cupFrame;
    public SpriteRenderer cupBack;
    public SpriteRenderer toppingSprite;
    public SpriteMask liquidMask;
    public int cupHeight; //drink클래스로 보내야 됨
    public int dropMaxNum = 6;

    public float offset = 2.7f; //액체 높이 보정
    
    public void MakeDrink(Drink _drink)
    {
        ResetDrink();
        string path = "";
        if (orign) path = "Cup"; else path = "Cup_thum";

        drink = _drink;
        cupBack.sprite = Resources.Load<Sprite>(path + "/Liquid-" + drink.cupNum);
        cupFrame.sprite = Resources.Load<Sprite>(path + "/Frame-" + drink.cupNum);
        liquidMask.sprite = Resources.Load<Sprite>(path + "/Liquid-" + drink.cupNum);

        if (drink.topping != "none") toppingSprite.sprite = Resources.Load<Sprite>(path + "/Topping_" + drink.topping); else toppingSprite.sprite = null;
        if(outline != null) outline.SetOutlineDrink();

        //Debug.Log(drink.colors.Count + "," + j);Debug.Log(drink.colors.Count + "," + j);  
        try
        {
            //액체에 색상넣기
            for (int j = 0; j < dropMaxNum; j++)
            {
                if (j < drink.colors.Count) { LiquidDiv[j].color = drink.colors[j]; }
                else { LiquidDiv[j].color = new Color(1, 1, 1, 0); }
            }
        }
        catch
        {
            Debug.Log("왜오류뜨다가말음");
        }

        cupHeight = 0; //이거해야할지..

        //액체 높이에 따른 조정
        if (cupHeight == 0) { offset = 0.23f; }  //중간컵
        else if (cupHeight == 1) { offset = 1.9f; }  //낮은컵
        else if (cupHeight == 2) { offset = 2.3f; }  //높은컵(와인잔)
        else { Debug.Log("액체 높이 연결 오류"); }

        //액체 바닥 위치 조정
        toppingSprite.gameObject.transform.localPosition = new Vector3(0, toppingSprite.gameObject.transform.localPosition.y + (offset * drink.colors.Count), toppingSprite.gameObject.transform.localPosition.z);

        //그라데이션
        /*
        if (gradient)
        {
            for (int i = 0; i < dropNum - 1; i++)
            {
                LiquidDiv[i].GetComponent<SpriteRenderer>().sprite = liquid_gradient;
                LiquidDiv[i].transform.localPosition = new Vector3(liquidPos[i].x, liquidPos[i].y + (height / 6) / 2f, liquidPos[i].z); ;
            }
        }*/
    }
    public void ResetDrink()
    {
        for (int i = 0; i < dropMaxNum; i++)
        {
            LiquidDiv[i].color = new Color(1, 1, 1, 0);
        }

    }

}
