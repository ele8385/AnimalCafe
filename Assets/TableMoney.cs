using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMoney : MonoBehaviour 
{
    public GameObject text;
    public string type;
    public int value;
    public List<GameObject> Coins;
    public CoinManager coinManager;
    
    public void MakeTableMoney(string _obj, int _val)
    {
        gameObject.SetActive(true);
        text.SetActive(false);
        type = _obj;
        value = _val;
        foreach (GameObject i in Coins)
        {
            i.SetActive(true);
        }
    }
    //_obj: coin이나 cherry UI오브젝트 네임
    //테이블 머니 클릭 시
    public void ClickMoney()
    {
        coinManager.AddMoney(type, value);

        //테이블머니 누르면 해당 좌석 비우기
        GameObject Chair = gameObject.transform.parent.gameObject;
        Chair.GetComponent<SeatManager>().full = false;
    }

}
