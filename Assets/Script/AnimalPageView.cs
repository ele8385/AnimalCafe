using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalPageView : MonoBehaviour
{
    public GameObject PrevBtn;
    public GameObject NextBtn;
    public Text PageText;
    public GameObject[] Unit = new GameObject[9];
    public Transform content;
    public int page = 0;
    public int unitCount = 9;
    public int lastPage = 0;

    public void Open()
    {
        gameObject.SetActive(true);
        UnitSetting();
    }

    public void UnitSetting()
    {
        int index = 0;
        foreach (Transform child in content.transform)
        {
            //할당할 동물 데이터 불러오기
            AnimalData animalData = Database.instance.GetAnimalData( (page * unitCount) + index );
            //할당할 동물 없으면 끝
            if (animalData == null) child.gameObject.SetActive(false);
            else
            {
                //동물 데이터 넣어서 유닛 셋팅
                child.gameObject.SetActive(true);
                child.gameObject.GetComponent<AnimalUnit>().SetUnit(animalData);
            }
            

            index++;
        }
        
        //페이지 표기
        lastPage = Mathf.CeilToInt(Database.instance.animals.Count / unitCount);
        PageText.text = (page + 1) + " / " + (lastPage + 1);
        if (page <= 0) PrevBtn.SetActive(false); else PrevBtn.SetActive(true);
        if (page >= lastPage) NextBtn.SetActive(false); else NextBtn.SetActive(true);

    }
    public void PrevPage()
    {
        if (page <= 0) return;
        page--;
        UnitSetting();
        AudioManager.instance.PlaySFX("Click_Page");
    }
    public void nextPage()
    {
        if (page >= lastPage) return;
        page++;
        UnitSetting();
        AudioManager.instance.PlaySFX("Click_Page");
    }

}
