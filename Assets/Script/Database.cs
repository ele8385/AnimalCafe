using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using System;
using System.Linq;

public class Database : MonoBehaviour
{
    public static Database instance;
    public string[] paths;

    public int version;      //새로운 앱 버전
    public List<AnimalData> animals = new List<AnimalData>();
    public List<Recipe> recipes = new List<Recipe>();
    public List<Dialogue> dialogues = new List<Dialogue>();
    public List<ItemData> items = new List<ItemData>();
    
    private void Awake()
    {
        instance = this;
        BetterStreamingAssets.Initialize();

    }

    public void DataLoad()
    {
        //Asset/StreamingAsset/Data 폴더
        RecipeLoad("Data/RecipeJson.json");
        AnimalsLoad("Data/AnimalsJson.json");
        DialogueLoad("Data/Dialogue.json");
        ItemLoad("Data/ItemJson.json");
    }
    private void Start()
    {
    }
    public AnimalData GetAnimalData(int animalCode)
    {
        return animals.Find(delegate (AnimalData bk) { return bk.code == animalCode; });
    }

    public Recipe GetRecipeData(int recipeCode)
    {
        return recipes.Find(delegate (Recipe bk) { return bk.code == recipeCode; });
    }

    public AnimalData GetAnimalData(string animalName)
    {
        return animals.Find(delegate (AnimalData bk) { return bk.name == animalName; });
    }

    public Recipe GetRecipeData(string recipeName)
    {
        return recipes.Find(delegate (Recipe bk) { return bk.name == recipeName; });
    }

    public ItemData GetItemData(int itemCode)
    {
        return items.Find(delegate (ItemData bk) { return bk.code == itemCode; });
    }

    public ItemData GetItemData(string itemName)
    {
        return items.Find(delegate (ItemData bk) { return bk.name == itemName; });
    }

    //동물 이름으로 해당 동물의 대화 가져오기
    public Dialogue GetDial(string name)
    {
        return dialogues.Find(delegate (Dialogue dial) { return dial.name == name; });
    }

    //enum을 string으로 불러와서 캐스팅
    public void AnimalAdd(int _code, string _name, string _type, string _info, string _hiddenInfo, string _hello, string _script_good, string _script_bad, string _recipe, int _moneyCondition, string _moodType, int _moodCondition, int _temperatureCondition)
    {
        AnimalType type = (AnimalType)Enum.Parse(typeof(AnimalType), _type);
        MoodType moodType = (MoodType)Enum.Parse(typeof(MoodType), _moodType);

        Recipe recipe = recipes.Find(delegate (Recipe bk) { return bk.name == _recipe; });

        animals.Add(new AnimalData(_code, _name, type, _info, _hiddenInfo, _hello, _script_good, _script_bad, recipe, _moneyCondition, moodType, _moodCondition, _temperatureCondition));
    }
    public void RecipeAdd(int _code, string _name, int _price, int _condition, string _info, List<Ingredient> _ingredients, Drink _drink)
    {

        recipes.Add(new Recipe(_code, _name, _price, _condition, _info, _ingredients, _drink));
    }
    public void ItemAdd(int _code, string _name, string _category, string _type, string _info, int _price, List<string> _mood)
    {
        items.Add(new ItemData(_code, _name, _category, _type, _info, _price, _mood));
    }
    public void AnimalsLoad(string filename)
    {
        byte[] byteContents = BetterStreamingAssets.ReadAllBytes(filename);
        string contentsString = System.Text.Encoding.GetEncoding("UTF-8").GetString(byteContents);

        JsonData jsonData = JsonMapper.ToObject(contentsString);

        try
        {
            for (int i = 0; i < jsonData.Count; i++)
            {
                AnimalAdd(
                    (int)jsonData[i]["code"],
                    jsonData[i]["name"].ToString() + i.ToString(),
                    jsonData[i]["type"].ToString(),
                    jsonData[i]["info"].ToString(),
                    jsonData[i]["hiddenInfo"].ToString(),
                    jsonData[i]["hello"].ToString(),
                    jsonData[i]["script_good"].ToString(),
                    jsonData[i]["script_bad"].ToString(),
                    jsonData[i]["recipe"].ToString(),
                    (int)jsonData[i]["moneyCondition"],
                    jsonData[i]["moodType"].ToString(),
                    (int)jsonData[i]["moodCondition"],
                    (int)jsonData[i]["temperatureCondition"]);
            }
        }
        catch
        {
            Debug.Log("AnimalsLoad 오류");
        }
        
    }
    public void RecipeLoad(string filename)
    {
        byte[] byteContents = BetterStreamingAssets.ReadAllBytes(filename);
        string contentsString = System.Text.Encoding.GetEncoding("UTF-8").GetString(byteContents);

        JsonData jsonData = JsonMapper.ToObject(contentsString);

        try
        {
            for (int i = 0; i < jsonData.Count; i++)
            {

                //색성분 리스트
                List<Ingredient> ingredients = new List<Ingredient>();
                for (int j = 0; j < jsonData[i]["ingredients"].Count; j++)
                {
                    ingredients.Add(new Ingredient(jsonData[i]["ingredients"][j].ToString()));
                }

                //컬러 리스트
                List<Color> colors = new List<Color>();
                for (int j = 0; j < jsonData[i]["drink"]["colors"].Count; j++)
                {
                    float r = float.Parse(jsonData[i]["drink"]["colors"][j]["color"][0].ToString()) / 255;
                    float g = float.Parse(jsonData[i]["drink"]["colors"][j]["color"][1].ToString()) / 255;
                    float b = float.Parse(jsonData[i]["drink"]["colors"][j]["color"][2].ToString()) / 255;

                    r = Mathf.Clamp(r, 0, 255f);
                    g = Mathf.Clamp(g, 0, 255f);
                    b = Mathf.Clamp(b, 0, 255f);

                    colors.Add(new Color(r, g, b));
                }

                for (int j = jsonData[i]["drink"]["colors"].Count; j < 6; j++)
                {
                    colors.Add(new Color(1, 1, 1, 0));
                }

                //그라디언트넘버 리스트
                List<int> gradient = new List<int>();
                for (int j = 0; j < jsonData[i]["drink"]["gradient"].Count; j++)
                {
                    gradient.Add((int)jsonData[i]["drink"]["gradient"][j]);
                }

                //Drink 생성
                int cupNum = (int)jsonData[i]["drink"]["cupNum"];
                string whipping = jsonData[i]["drink"]["whipping"].ToString();
                string syrup = jsonData[i]["drink"]["syrup"].ToString();
                string topping = jsonData[i]["drink"]["topping"].ToString();
                Drink drink = new Drink(cupNum, colors, gradient, whipping, syrup, topping);

                //Recipe 생성
                RecipeAdd(
                    (int)jsonData[i]["code"],
                    jsonData[i]["name"].ToString(),
                    (int)jsonData[i]["price"],
                    (int)jsonData[i]["condition"],
                    jsonData[i]["info"].ToString(),
                    ingredients,
                    drink
                    );
            }
        }
        catch
        {
            Debug.Log("DrinksLoad 오류");
        }

    }
    public void ItemLoad(string filename)
    {
        byte[] byteContents = BetterStreamingAssets.ReadAllBytes(filename);
        string contentsString = System.Text.Encoding.GetEncoding("UTF-8").GetString(byteContents);

        JsonData jsonData = JsonMapper.ToObject(contentsString);
        try
        {
            for (int i = 0; i < jsonData.Count; i++)
            {
                // 쉼표로 분리하여 배열로 만듦
                string[] dataArray = jsonData[i]["mood"].ToString().Split(',');

                // 배열을 List로 변환
                List<string> mood = new List<string>(dataArray);

                ItemAdd(
                    (int)jsonData[i]["code"],
                    jsonData[i]["name"].ToString(),
                    jsonData[i]["category"].ToString(),
                    jsonData[i]["type"].ToString(),
                    jsonData[i]["info"].ToString(),
                    (int)jsonData[i]["price"],
                    mood
                    );
            }
        }
        catch
        {
            Debug.Log("itemsLoad 오류");
        }
    }

    public void DialogueLoad(string filename)
    {
        byte[] byteContents = BetterStreamingAssets.ReadAllBytes(filename);
        string contentsString = System.Text.Encoding.GetEncoding("UTF-8").GetString(byteContents);

        JsonData jsonData = JsonMapper.ToObject(contentsString);
        
        try
        {
            for (int i = 0; i < jsonData.Count; i++) //동물별 - Dialog(name, dialogs)
            {
                List<Script> scripts = new List<Script>();

                for (int j = 0; j < jsonData[i]["script"].Count; j++) //대화별 - Script(line, anwers)
                {
                    List<Answer> answers = new List<Answer>();
                    Script script = new Script();

                    for (int k = 0; k < jsonData[i]["script"][j]["answers"].Count; k++) //대답별 - ansewers(select,text,nextline)
                    {
                        Answer answer = new Answer();

                        answer.select = jsonData[i]["script"][j]["answers"][k]["select"].ToString();
                        answer.text = jsonData[i]["script"][j]["answers"][k]["text"].ToString();
                        answer.nextLine = (int)jsonData[i]["script"][j]["answers"][k]["nextLine"];
                        answer.heart = (int)jsonData[i]["script"][j]["answers"][k]["heart"];
                        answers.Add(new Answer(answer.select, answer.text, answer.nextLine, answer.heart));
                    }
                    
                    script.line = jsonData[i]["script"][j]["line"].ToString();
                    script.answers = answers;
                    scripts.Add(new Script(j, script.line, script.answers));
                }
                string name = jsonData[i]["name"].ToString();
                dialogues.Add(new Dialogue(name, scripts));
            }
        }
        catch
        {
            Debug.Log("DialogueLoad 오류");
        }

    }



}
