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
    public int lastPage = 0;
    public int unitCount = 9;

    public void Open()
    {
        gameObject.SetActive(true);
        UnitSetting();
    }

    public void UnitSetting()
    {
        for (int i = 0; i < unitCount; i++)
        {
            //유닛 인덱스
            int index = (page * unitCount) + i;
            Animal animal = Database.instance.animals[index];

            //동물 수 초과하면 비활성화
            if (State.instance.myState.myAnimals.Count <= index)
            {
                Unit[i].gameObject.SetActive(false);
            }
            else
            {
                Unit[i].gameObject.SetActive(true);
                //동물이미지 및 그림자 넣기
                string filename = System.Text.RegularExpressions.Regex.Replace(animal.name, @"\d", "");

                Sprite[] sprites = Resources.LoadAll<Sprite>("Character/" + filename);
                Unit[i].transform.Find("Thumnail").transform.Find("Img").GetComponent<Image>().sprite = sprites[0];
                Unit[i].transform.Find("Thumnail").transform.Find("Shadow").GetComponent<Image>().sprite = sprites[0];
                
                //동물 오픈
                if (State.instance.myState.myAnimals[index].heart > 0)
                {
                    //동물이름
                    Unit[i].transform.Find("Name").GetComponent<Text>().text = animal.name;
                    //동물그림자없애기
                    Unit[i].transform.Find("Thumnail").transform.Find("Shadow").transform.Find("Color").GetComponent<Image>().color = new Color(1, 1, 1, 0);

                    //힌트 없애고 하트 표시
                    Unit[i].transform.Find("Hint").gameObject.SetActive(false);
                    Unit[i].transform.Find("Heart").gameObject.SetActive(true);
                    for (int j = 0; j < 5; j++)
                    {
                        if (j < (int)State.instance.GetHeartStep(index))
                            Unit[i].transform.Find("Heart").transform.GetChild(j).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI_Heart");
                        else
                            Unit[i].transform.Find("Heart").transform.GetChild(j).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI_Heart_Blank");

                    }
                    //유닛 프리팹의 스크립트에 동물 정보 넣기
                    Unit[i].gameObject.GetComponent<AnimalUnit>().animal = animal;
                }
                //동물 미오픈
                else
                {
                    Unit[i].transform.Find("Thumnail").transform.Find("Shadow").transform.Find("Color").GetComponent<Image>().color = new Color(123 / 255f, 88 / 255f, 69 / 255f, 1);
                    //동물이름 및 힌트
                    Unit[i].transform.Find("Name").GetComponent<Text>().text = "???";
                    Unit[i].transform.Find("Hint").GetComponent<Text>().text = animal.hello;

                    Unit[i].transform.Find("Hint").gameObject.SetActive(true);
                    Unit[i].transform.Find("Heart").gameObject.SetActive(false);

                    //유닛 프리팹의 스크립트에 동물 정보 빼기
                    Unit[i].gameObject.GetComponent<AnimalUnit>().animal = new Animal();
                }
            }
        }
        //페이지 표기
        lastPage = (State.instance.myState.myAnimals.Count / unitCount);
        PageText.text = (page + 1) + " / " + (lastPage + 1);
        if (page <= 0) PrevBtn.SetActive(false); else PrevBtn.SetActive(true);
        if (page >= lastPage) NextBtn.SetActive(false); else NextBtn.SetActive(true);

    }
    public void PrevPage()
    {
        if (page <= 0) return;
        page--;
        UnitSetting();
    }
    public void nextPage()
    {
        if (page >= lastPage) return;
        page++;
        UnitSetting();
    }
   
}
