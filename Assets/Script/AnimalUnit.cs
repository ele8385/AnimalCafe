using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalUnit : MonoBehaviour
{
    public GameObject Info;
    public Animal animal = new Animal();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenInfo()
    {
        //미오픈 동물이면 종료
        if (animal.name == null) return;

        //정보창 할당
        Info = GameObject.Find("Canvas").transform.Find("Collect_window").transform.Find("Info").gameObject;
        Info.SetActive(true);

        //정보 넣기
        Sprite[] sprites = Resources.LoadAll<Sprite>("Character/" + animal.name);
        Info.transform.Find("InfoBg").transform.Find("Thumnail").transform.Find("Img").gameObject.GetComponent<Image>().sprite = sprites[0];
        Info.transform.Find("InfoBg").transform.Find("Name").gameObject.GetComponent<Text>().text = "이름: " + animal.name;
        Info.transform.Find("InfoBg").transform.Find("Hint").gameObject.GetComponent<Text>().text = animal.info;
        
    }
}
