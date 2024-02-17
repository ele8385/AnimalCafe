using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ingredient
{
    public int[] ingerdientArr; // = { c, m, y, k, r, g, b, w, 칸 수, graient여부 }

    public Ingredient() { }

    //string으로 된 성분레시피를 각 문자마다 int형으로 변환하여 배열로 넣기
    public Ingredient(string _text)
    {
        ingerdientArr = new int[_text.Length];
        
        for (int i = 0; i < _text.Length; i++)
        {
            ingerdientArr[i] = int.Parse(_text[i].ToString());
        }
    }

}
[System.Serializable]
public class Recipe //등록된 음료메뉴
{
    public int code;        //음료 코드
    public string name;     //음료 이름
    public int price;       //음료 가격
    public int condition;       //음료 등장 조건
    public string info;     //음료 설명
    public string ingredients;   //레시피 힌트
    public Drink drink;     //등록 음료

    public Recipe()
    {
    }

    public Recipe(int _code, string _name, int _price, int _condition, string _info, List<Ingredient> _ingredients, Drink _drink)
    {
        code = _code;
        name = _name;
        price = _price;
        condition = _condition;
        info = _info;
        ingredients = TextIngredient(_ingredients);
        drink = _drink;
    }

    // "cmykrgbw0,cmykrgbw1,cmykrgbw0 1. "딸기 1 : 바나나 1" 비율의 액체 2칸 붓기 2. 층층 파우더 넣기 3. "우유 1" 비율의 액체 2칸 붓기
    public string TextIngredient(List<Ingredient> ingredients)
    {
        string text = "";
        int num = 0;

        for (int j = 0; j < ingredients.Count; j++) //레시피 리스트 인덱스
        {
            num++;                       //레시피 목차 번호
            text += num + ". [";                //"1. ["
            int[] arr = ingredients[j].ingerdientArr;

            for (int i = 0; i < arr.Length; i++) //각 성분별
            {
                if (arr[i] == 0) continue;
                switch (i)
                {
                    case 0: text += "물 " + arr[i] + " : "; break;
                    case 1: text += "베리 " + arr[i] + " : "; break;
                    case 2: text += "레몬 " + arr[i] + " : "; break;
                    case 3: text += "원두 " + arr[i] + " : "; break;
                    case 4: text += "딸기 " + arr[i] + " : "; break;
                    case 5: text += "녹차 " + arr[i] + " : "; break;
                    case 6: text += "블루베리 " + arr[i] + " : "; break;
                    case 7: text += "우유 " + arr[i] + " : "; break;
                }
            }
           text =  text.Remove(text.Length - 3); // 끝에 ':' 지우기
            text += "] 비율로 " + arr[8] + "칸 담기\n";

            if (j == ingredients.Count - 1) break; //마지막 라인이면 탈출
            num++;
            if (arr[9] == 0) { text += num + ". 층층 파우더 넣기\n"; }
            else { text += num + ". 그라데이션 파우더 넣기\n"; }
        }
        return text;

        /*
        ingredient = new List<Ingredient>();
        ingredient.Add(new Ingredient());
        ingredient[0].TextIngredient;
        */
    }
}
