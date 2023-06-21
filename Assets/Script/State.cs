using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MyState
{
    public int version;// { get; set; } //앱 버전

    public int nowMoney;// { get; set; }  //현재 금액
    public int totalMoney;// { get; set; }  //지금까지 번 총 금액
    public int maxMoney;// { get; set; }
    public int repute;// { get; set; }  //평판
    public int moodClassic;// { get; set; }  //클래식수치
    public int moodRomantic;// { get; set; }  //로맨틱수치
    public int moodTrendy;// { get; set; }  //트렌디수치
    public int maxHeart;// { get; set; }  //최대 하트
    public List<string> mood = new List<string>();// { get; set; }  //분위기 워드

    public List<Animal> myAnimals = new List<Animal>(); //동물리스트
    public List<Recipe> myRecipe = new List<Recipe>(); //보유한 음료리스트
    public List<Item> myItems = new List<Item>();



    public ItemData Wallpaper, WallBand, WallBand2, Floor;
    public ItemData Table0, Table1, Table2, Table3, Table4, Table5;

    //C:\Users\ele83\AppData\LocalLow\PandaPie\AnimalCafe
}
public class State : MonoBehaviour {

    public static State instance;
    public MyState myState;
    string filePath;

    public Text MoneyText;
    public PopUpManager popUp;
    public TextPopUpManager textPopUp;
    public ItemSetting ItemSetting;

    private void Awake()
    {
        instance = this;
        filePath = Application.persistentDataPath + "/MyStateJson.json";
        myState = new MyState();
        //Load(); 동물과 음료 배열 길이 불러오기 위해 Database에서 호출

    }
    // Use this for initialization
    void Start () {
        MoneyText = GameObject.Find("Canvas").transform.Find("Top").transform.Find("Money").transform.Find("Text").GetComponent<Text>();
        Print(MoneyText);
    }

    public void Print(Text _text)
    {
        _text.text = string.Format("{0:#,0}", myState.nowMoney);
    }

    //동물코드로 동물데이터 가져오기
    public Animal GetMyAnimal(int _code)
    {
        return myState.myAnimals.Find(delegate (Animal bk) { return bk.code == _code; });
    }
    //동물이름으로 MyAnimal객체 가져오기
    public Animal GetMyAnimal(string _name)
    {
        int code = myState.myAnimals.FindIndex(item => item.name.Equals(_name));
        return myState.myAnimals[code];
    }
    //코드로 Item 객체 가져오기
    public Item GetMyItem(int _code)
    {
        return myState.myItems.Find(delegate (Item bk) { return bk.code == _code; });
    }

    public Recipe GetRandomRecipe()
    {
        int randomCode;
        Recipe recipe;
        while (true)  // 해당 음료 오픈되었는지 여부
        {
            randomCode = Random.Range(0, Database.instance.recipes.Count);
            recipe = Database.instance.GetRecipeData(randomCode);
            if (recipe.condition <= myState.totalMoney) break;
        }
        return recipe;
    }
    

    
    //동물의 음료구매 수 5씩 증가할 때마다 이벤트 트루 리턴
    public bool GetBoolEvent(int code)
    {
        if ((myState.myAnimals[code].orderCount % (myState.maxHeart / 5f)) == 0) return true;  
        else return false; //아닐 땐 -1 리턴
    }
    //해당 동물의 주문수 증가
    public void AddOrderCount(int code)
    {
        Animal AnimalData = GetMyAnimal(code);
        AnimalData.orderCount ++;
        Save();
    }
    //해당 동물의 구매수 증가
    public void AddBuyCount(int code)
    {
        Animal AnimalData = GetMyAnimal(code);
        AnimalData.buyCount++;
        if (AnimalData.buyCount == 1) {
            textPopUp.OpenPopUp("'" + AnimalData.name + "'" + " 손님이 수첩에 추가되었어요.", -381f, "notice", AnimalData.name);
        }
        Save();
    }
    //현재 대화 넘버 수정
    public void AddDialNum(int code, int dialNum)
    {
        myState.myAnimals[code].dialNum = dialNum;
        Save();
    }
    //호감도 적립
    public void AddHeart(int code, int heart)
    {
        if (myState.myAnimals[code].heart > myState.maxHeart) return; //최대치 넘음
        myState.myAnimals[code].heart += heart;
        if (myState.myAnimals[code].heart < 0) myState.myAnimals[code].heart = 0; //음수면 0으로

        Save();
    }
    //호감도 단계 가져오기
    public float GetHeartStep(int code)
    {
        return myState.myAnimals[code].heart / (myState.maxHeart / 5);
    }


    //레시피 습득 성공여부 반환
    public bool AddRecipe(Recipe _recipe)
    {
        //중복 레시피면 false
        if (myState.myRecipe.Contains(_recipe)) return false;
        else
        {
            myState.myRecipe.Add(_recipe);
            textPopUp.OpenPopUp("'" + _recipe.name + "'" + " 의 레시피를 습득했어요.", -381f, "notice", _recipe.name);
            Save();
            return true;
        }
    }

    //돈 적립
    public void PlusMoney(int val)
    {
        if (val == 0) return;
        if(myState.maxMoney > myState.nowMoney)
        {
            CountEffet(myState.nowMoney + val, MoneyText);
            myState.totalMoney += val;
        }
        else
        {
            Debug.Log("보유한도초과");
        }
        Save();
    }
    //돈 차감
    public bool MinusMoney(int price)
    {
        if(myState.nowMoney > price) //가격보다 현재 돈이 크면 돈 차감
        {
            myState.nowMoney -= price;
            Debug.Log(price + "차감");
        }
        else // 돈 부족
        {
            textPopUp.OpenPopUp("코인이 부족해요", -381f);
            return false;
        }
        Print(MoneyText);
        Save();
        return true;
    }

    //아이템 구매
    public bool AddInteriorItem(ItemData itemData)
    {
        if (GetMyItem(itemData.code) != null) { popUp.OpenOkPopUp("이미 구입한 물건이에요."); return false; } //이미 구입했으면 false리턴
        if (Resources.Load<Sprite>("Item/" + itemData.name) == null) { popUp.OpenOkPopUp("구매할 수 없는 물건이에요."); return false; }//이미지 없으면 리턴
        if (!MinusMoney(itemData.price)) return false; //돈 차감 실패하면 false 리턴

        myState.myItems.Add(new Item(itemData.code));
        Save();
        return true;
    }
    //아이템 적용
    public bool ApplyInteriorItem(ItemData itemData)
    {
        //기존 아이템 해제(특성치 빼기)하고 새로 넣기
        if (itemData.type == "벽지") { ClearInteriorItem(myState.Wallpaper); myState.Wallpaper = itemData; }
        else if (itemData.type == "바닥지") { ClearInteriorItem(myState.Floor); myState.Floor = itemData; }

        Item item = GetMyItem(itemData.code);
        item.apply = true;

        //새 아이템 능력치 넣기
        moodSet(itemData);
        Save();
        return true;
    }

    //아이템 해제
    public bool ClearInteriorItem(ItemData itemData)
    {
        moodSet(itemData);
        Save();
        return true;
    }
    
    //아이템 적용 유무 반환
    public bool CheckApplyItem(string itemObject)
    {
        if (myState.Wallpaper.name    == itemObject) return true;
        /*
        if (myState.floor.  GetComponent<Item>().itemName   == itemObject) return true;
        if (myState.table1. GetComponent<Item>().itemName  == itemObject) return true;
        if (myState.table2. GetComponent<Item>().itemName  == itemObject) return true;
        if (myState.table3. GetComponent<Item>().itemName  == itemObject) return true;
        if (myState.table4. GetComponent<Item>().itemName  == itemObject) return true;
        if (myState.table5. GetComponent<Item>().itemName  == itemObject) return true;
        if (myState.table6. GetComponent<Item>().itemName  == itemObject) return true;
        if (myState.Object1.GetComponent<Item>().itemName == itemObject) return true;
        if (myState.Object3.GetComponent<Item>().itemName == itemObject) return true;
        if (myState.Object2.GetComponent<Item>().itemName == itemObject) return true;*/
        return false;
    }

    public void moodSet(ItemData itemData)
    {
        myState.mood.AddRange(itemData.mood);
    }

    public void CountEffet(int num, Text txt)
    {
        StartCoroutine(CountEffetCo(num, txt));
    }
    IEnumerator CountEffetCo(int num, Text txt)
    {
        float sec = 60f;
        while (myState.nowMoney != num)
        {
            if (myState.nowMoney < num) myState.nowMoney += 10;
            else myState.nowMoney -= 10;
            Print(txt);
            sec--;
            if (sec < 0) break; //일정 시간 지나면 효과 정지
            yield return new WaitForSeconds(0.01f);
        }
        myState.nowMoney = num;
        Print(txt);

        Save();
    }

    public void ResetData()
    {
        myState.nowMoney = 600000;
        myState.totalMoney = myState.nowMoney;
        myState.maxMoney = 100000000;
        myState.repute = 0;
        myState.moodClassic = 0;
        myState.moodRomantic = 0;
        myState.moodTrendy = 0;
        myState.maxHeart = 200;
        myState.Wallpaper = GetComponent<DefaltItemList>().wall.gameObject.GetComponent<ItemUnit>().itemData; //?

        myState.myAnimals = new List<Animal>();
        myState.myRecipe = new List<Recipe>();
        myState.myItems = new List<Item>();

        //디폴트 할당
        for (int i = 0; i < Database.instance.animals.Count; i++)
        {
            myState.myAnimals.Add(new Animal(i));
        }
        myState.myItems.Add(new Item(0));


        Save();
        Debug.Log("초기화");

    }

    //animals 배열 업데이트
    public void UpdateData()
    {
        int stateCount = myState.myAnimals.Count;
        int dataCount = Database.instance.animals.Count;

        if (stateCount == dataCount) return;

        //동물코드가 중간에 삭제되는 경우는 생각하지 않기로 해...

        //현재 동물리스트보다 불러온 DB 동물리스트가 더 큰 경우 빈 만큼 Animal 요소 추가
        else if (stateCount < dataCount) for (int i = stateCount; i < dataCount; i++) myState.myAnimals.Add(new Animal(i));
        //현재 동물리스트보다 불러온 DB 동물리스트가 더 작은 경우 그만큼 현재 동물리스트의 요소 삭제
        else myState.myAnimals.RemoveRange(dataCount, stateCount - dataCount);
    }

    //불러오기 - <Database>().Awake()에서 호출
    public void Load()
    {
        if(!File.Exists(filePath)) { ResetData(); return; } //파일 없으면 초기화

        string jsonString = File.ReadAllText(filePath);
        myState = JsonUtility.FromJson<MyState>(jsonString);

        if (Database.instance.version != myState.version) { myState.version = Database.instance.version; UpdateData(); } //버전 다르면 버전 올리고 DB업데이트
        else { UpdateData(); }
    }
    [ContextMenu("Save")]
    public void Save()
    {
        string jsonData = JsonUtility.ToJson(myState, true);
        File.WriteAllText(filePath, jsonData);
    }
}
