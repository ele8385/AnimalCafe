using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    public List<GameObject> ThumDrinks;
    // Start is called before the first frame update
    void Start()
    {
        //썸네일드링크 모두 끄기
        foreach(GameObject Thum in ThumDrinks) Thum.SetActive(false);
    }

    //자리 있는지 체크 (check랑 add함수 따로 있는 이유: check와 add의 실행타이밍 시간차 있음)
    public bool ChenkPickUp()
    {
        foreach (GameObject Thum in ThumDrinks)
        {
            if (Thum.activeSelf) continue; 
            return true; //빈 자리 있으면 true
        }
        return false; //빈 자리 없으면 false
    }

    //픽업대에 썸넬드링크 만들기
    public void AddPickUpDrink(Drink drink)
    {
        foreach (GameObject Thum in ThumDrinks)
        {
            if (Thum.activeSelf) continue;
            else
            {
                Thum.SetActive(true);
                Thum.GetComponent<DrinkManager>().MakeDrink(drink);
                return;
            }
        }
    }
}
