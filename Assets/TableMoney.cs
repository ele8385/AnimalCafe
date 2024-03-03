using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMoney : MonoBehaviour 
{
    public GameObject TextObj;
    public CoinManager coinManager;
    public int value;
    
    //테이블에 팁 두기
    public void MakeTableMoney(int _val)
    {
        gameObject.SetActive(true);
        TextObj.gameObject.SetActive(false);
        value = _val;
    }
    //테이블 위 팁 클릭 시
    public void ClickMoney()
    {
        TextObj.gameObject.SetActive(true);
        coinManager.AddCoin(value);

        //테이블머니 누르면 해당 좌석 비우기
        GameObject Chair = gameObject.transform.parent.gameObject;
        Chair.GetComponent<SeatManager>().full = false;
    }

}
