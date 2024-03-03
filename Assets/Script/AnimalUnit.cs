using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class AnimalUnit : MonoBehaviour
{
    public GameObject Info;
    public AnimalData AnimalData = new AnimalData();
    public Image Thumnail;
    public Image Shadow;
    public GameObject Hearts;
    public Text AnimalName;
    public Text Hint;
    public Sprite[] sprites;


    public void SetUnit(AnimalData _AnimalData)
    {
        AnimalData = _AnimalData;
        Animal myAnimal = State.instance.GetMyAnimal(AnimalData.code);

        gameObject.SetActive(true);
        //동물이미지 및 그림자 넣기
        try
        {
            string path = "Character/" + Regex.Replace(AnimalData.name, @"\d", ""); //임시테스트 중이라 동물 이름 뒤에 숫자 붙인 거 지우기
            sprites = Resources.LoadAll<Sprite>(path);
            Thumnail.sprite = sprites[0];
            Shadow.sprite = sprites[0];
        }
        catch { Debug.Log("동물 이미지 연결 오류"); }


        //동물 오픈
        if (myAnimal != null && myAnimal.heart > 0)
        {
            //동물이름
            AnimalName.text = AnimalData.name;

            //동물그림자없애기
            Shadow.transform.Find("Color").GetComponent<Image>().color = new Color(1, 1, 1, 0);

            //힌트 없애고 하트 표시
            Hint.gameObject.SetActive(false);
            Hearts.gameObject.SetActive(true);

            for(int i = 0; i < Hearts.transform.childCount; i++)
            {
                if (i < State.instance.GetHeartStep(myAnimal.code))
                    Hearts.transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI_Heart");
                else
                    Hearts.transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI_Heart_Blank");
            }
        }
        //동물 미오픈
        else
        {
            Shadow.transform.Find("Color").GetComponent<Image>().color = new Color(123 / 255f, 88 / 255f, 69 / 255f, 1);
            //동물이름 및 힌트
            AnimalName.text = "???";
            Hearts.gameObject.SetActive(false);
            Hint.gameObject.SetActive(true);
            Hint.text = AnimalData.hello;

        }
    }
    public void OpenInfo()
    {
        //미오픈 동물이면 종료
        if (AnimalData.name == null) return;

        //정보창 할당
        Info = GameObject.Find("Canvas").transform.Find("Collect_window").transform.Find("Info").gameObject;
        Info.SetActive(true);

        try
        {
            //정보 넣기
            Sprite[] sprites = Resources.LoadAll<Sprite>("Character/" + AnimalData.name);
            Info.transform.Find("InfoBg").transform.Find("Thumnail").transform.Find("Img").gameObject.GetComponent<Image>().sprite = sprites[0];
            Info.transform.Find("InfoBg").transform.Find("Name").gameObject.GetComponent<Text>().text = "이름: " + AnimalData.name;
            Info.transform.Find("InfoBg").transform.Find("Hint").gameObject.GetComponent<Text>().text = AnimalData.info;
        }
        catch
        {
            Debug.Log("동물상세정보창 이미지 오류");
        }
        
        
    }
}
