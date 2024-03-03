using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public int maxHeart;// { get; set; }  //최대 하트

    public List<Animal> myAnimals = new List<Animal>(); //동물리스트
    public List<Recipe> myRecipe = new List<Recipe>(); //보유한 음료리스트
    public List<Item> myItems = new List<Item>(); //보유한 아이템리스트
    
    public Dictionary<string, ItemData> applyItem = new Dictionary<string, ItemData>(); //적용 중인 아이템 딕셔너리
    public List<ItemDictionary> tempList; //적용 중인 아이템 딕셔너리를 json에 저장하기 위한 임시 리스트


    //C:\Users\ele83\AppData\LocalLow\PandaPie\AnimalCafe
}

[System.Serializable]
public class ItemDictionary //딕셔너리형 변수 json에 저장하도록 만든 클래스
{
    public string name;
    public ItemData itemData;
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
        // 싱글턴 패턴 구현
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);
        }

        filePath = Application.persistentDataPath + "/MyStateJson.json";
        myState = new MyState();
        //Load(); 동물과 음료 배열 길이 불러오기 위해 Database에서 호출

        Database.instance.DataLoad();
        Load();

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

    //동물코드로 myAnimals에서 동물데이터 가져오기
    public Animal GetMyAnimal(int _code)
    {
        return myState.myAnimals.Find(delegate (Animal bk) { return bk.code == _code; });
    }
    //동물이름으로 myAnimals에서 객체 가져오기
    public Animal GetMyAnimal(string _name)
    {
        return myState.myAnimals.Find(delegate (Animal bk) { return bk.name == _name; });
    }

    //레시피코드로 myRecipe 객체 가져오기
    public Recipe GetMyRecipe(int _code)
    {
        return myState.myRecipe.Find(delegate (Recipe bk) { return bk.code == _code; });
    }
    //레시피이름으로 myRecipe 객체 가져오기
    public Recipe GetMyRecipe(string _name)
    {
        return myState.myRecipe.Find(delegate (Recipe bk) { return bk.name == _name; });
    }

    //코드로 myItems에서 Item 객체 가져오기
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
    
    //myAnimals에 Animal 추가 //첫 대화이벤트 실행 시 호출
    public Animal AddMyAnimal(int code)
    {
        myState.myAnimals.Add(new Animal(code));
        return GetMyAnimal(code);
    }

    //동물의 음료구매 수 5씩 증가할 때마다 이벤트 트루 리턴 //수치 바꿀 거면 GetHeartStep도 수정 필요
    public bool GetBoolEvent(int code)
    {
        if (GetMyAnimal(code) == null) return true;

        if (GetMyAnimal(code).heart == 0 ||
            GetMyAnimal(code).heart == 5 ||
            GetMyAnimal(code).heart == 20 ||
            GetMyAnimal(code).heart == 50 ||
            GetMyAnimal(code).heart == 100 ||
            GetMyAnimal(code).heart == 200
            )
            return true;
        else return false;
    }
    //해당 동물의 주문수 증가
    public void AddOrderCount(int code)
    {
        if (GetMyAnimal(code) == null) myState.myAnimals.Add(GetMyAnimal(code)); ;
        GetMyAnimal(code).orderCount ++;
        Save();
    }
    //해당 동물의 구매수 증가
    public void AddBuyCount(int code)
    {
        GetMyAnimal(code).buyCount++;

        //수첩에 등록 팝업
        if (GetMyAnimal(code).buyCount == 1) {
            textPopUp.OpenPopUp("'" + GetMyAnimal(code).name + "'" + " 손님이 수첩에 추가되었어요.", -381f, "notice", GetMyAnimal(code).name);
        }
        Save();
    }
    //현재 대화 넘버 수정
    public void AddDialNum(int code, int dialNum)
    {
        GetMyAnimal(code).dialNum = dialNum;
        Save();
    }
    //호감도 적립
    public void AddHeart(int code, int heart)
    {
        if(GetMyAnimal(code) == null) { GetMyAnimal(code).heart += heart; } //주문수 0이라 아직 myAnimals에 생성 안됐는데 호감도를 어케올림
        else
        {
            if (GetMyAnimal(code).heart > myState.maxHeart) return; //최대치 넘음
            if (GetMyAnimal(code).heart < 0) return; //음수일 때도 있나
            GetMyAnimal(code).heart += heart;
        }
        

        Save();
    }
    //호감도 단계 가져오기 //수치 바꿀 거면 GetBoolEvent도 수정 필요
    public int GetHeartStep(int code)
    {
        int heart = GetMyAnimal(code).heart;
        int step = 0;

        if (heart < 5) { step = 0; }
        else if (heart < 20) { step = 1; }
        else if (heart < 50) { step = 2; }
        else if (heart < 100) { step = 3; }
        else if (heart < myState.maxHeart) { step = 4; }
        else if (heart >= myState.maxHeart) { step = 5; }
        else { step = 0; Debug.Log("호감도오류"); }
        return step;
    }


    //레시피 습득 성공여부 반환
    public bool AddRecipe(Recipe _recipe)
    {
        bool check = myState.myRecipe.Exists(a => a.code == _recipe.code);

        //중복 레시피면 false 반환
        if (check) return false;
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
            AudioManager.instance.PlaySFX("Plus_Money");
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
            AudioManager.instance.PlaySFX("Minus_Money");
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
        if (Resources.Load<Sprite>(itemData.category + "/" + itemData.type + "/" + itemData.category + "_" + itemData.type + "_" + itemData.name) == null) { popUp.OpenOkPopUp("구매할 수 없는 물건이에요."); return false; }//이미지 없으면 리턴
        if (!MinusMoney(itemData.price)) return false; //돈 차감 실패하면 false 리턴

        myState.myItems.Add(new Item(itemData.code));
        Save();
        return true;
    }
    //아이템 적용
    public bool ApplyInteriorItem(ItemData itemData)
    {
        //적용하려는 아이템이 보유 아이템 목록에 없다면 추가
        if (GetMyItem(itemData.code) == null) { myState.myItems.Add(new Item(itemData.code));}


        //해당 아이템 타입이 존재하면 바꿈
        if (myState.applyItem.ContainsKey(itemData.type))
        {
            //기존 아이템 적용 해제
            ItemData _itemData = myState.applyItem[itemData.type];
            Debug.Log(_itemData.name);
            GetMyItem(_itemData.code).apply = false;
            myState.applyItem[itemData.type] = itemData;
        }
        //해당 아이템 타입이 없으면 키 새로 생성
        else {  myState.applyItem.Add(itemData.type, itemData); }

        GetMyItem(itemData.code).apply = true;
        ItemSetting.ItemObjectSet();

        Save();
        return true;
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

    [ContextMenu("Reset")]
    public void ResetData()
    {
        myState.nowMoney = 600000;
        myState.totalMoney = myState.nowMoney;
        myState.maxMoney = 100000000;
        myState.repute = 0;
        myState.maxHeart = 200;

        myState.myAnimals = new List<Animal>();
        myState.myRecipe = new List<Recipe>();
        myState.myItems = new List<Item>();

        myState.applyItem = new Dictionary<string, ItemData>();
        
        ApplyInteriorItem(Database.instance.GetItemData("분홍벽지"));
        ApplyInteriorItem(Database.instance.GetItemData("분홍바닥지"));
        ItemSetting.ItemObjectSet();

        Save();
        Debug.Log("초기화");

    }

    //animals 배열 업데이트
    public void UpdateData()
    {
        // animals에 새로운 데이터 추가되었다면 myaAnimal에도 추가
        foreach (var animalData in Database.instance.animals)
        {
            if (!myState.myAnimals.Any(a => a.code == animalData.code))
            {
                myState.myAnimals.Add(new Animal(animalData.code));
            }
        }

        // myAnimals 내에 더 이상 animals 리스트에 없는 항목 제거
        for (int i = myState.myAnimals.Count - 1; i >= 0; i--)
        {
            if (!Database.instance.animals.Any(a => a.code == myState.myAnimals[i].code))
            {
                myState.myAnimals.RemoveAt(i);
            }
        }
        
    }


    //불러오기 - <Database>().Awake()에서 호출
    [ContextMenu("Load")]
    public void Load()
    {
        if (!File.Exists(filePath)) { ResetData(); return; } //파일 없으면 초기화

        string jsonString = File.ReadAllText(filePath);
        myState = JsonUtility.FromJson<MyState>(jsonString);

        //UpdateData();

        //딕셔너리형 변수 로드 가능하도록 변환
        if (myState.tempList != null)
        {
            foreach (ItemDictionary pair in myState.tempList)
            {
                myState.applyItem.Add(pair.name, pair.itemData);
            }
        }
            
    }

    [ContextMenu("Save")]
    public void Save()
    {
        //딕셔너리형 변수 저장 가능하도록 변환
        if (myState.tempList != null)
        {
            List<ItemDictionary> _tempList = new List<ItemDictionary>();
            foreach (KeyValuePair<string, ItemData> pair in myState.applyItem)
            {
                _tempList.Add(new ItemDictionary { name = pair.Key, itemData = pair.Value });
            }
            myState.tempList = _tempList;
        }

        string jsonData = JsonUtility.ToJson(myState, true);
        File.WriteAllText(filePath, jsonData);
    }
}
