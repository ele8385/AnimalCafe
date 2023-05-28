using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeManager : MonoBehaviour
{
    public bool orign; //true - 원본컵 / false = Thum컵
    public Recipe recipe;
    public Image cupBack;
    public Image cupFrame;
    public Image liquidMask;
    public List<Image> LiquidDiv;
    public Image toppingSprite;
    public Text recipeName;
    public Text recipePrice;
    public Text recipeIngredients;
    public Text recipeInfo;
    public Outline outline;

    public void MakeRecipe(Recipe _recipe)
    {
        ResetDrink();
        recipe = _recipe;

        string path = "";
        if (orign) path = "Cup"; else path = "Cup_thum";

        cupBack.sprite = Resources.Load<Sprite>(path + "/Liquid-" + recipe.drink.cupNum);
        cupFrame.sprite = Resources.Load<Sprite>(path + "/Frame-" + recipe.drink.cupNum);
        liquidMask.sprite = Resources.Load<Sprite>(path + "/Liquid-" + recipe.drink.cupNum);
        if(recipeName != null) recipeName.text = recipe.name;
        if(recipePrice != null) recipePrice.text = string.Format("{0:#,0}", recipe.price.ToString());
        if (recipeIngredients != null) recipeIngredients.text = recipe.ingredients;
        if (recipeInfo != null) recipeInfo.text = recipe.info;
        if (recipe.drink.topping != "none") toppingSprite.sprite = Resources.Load<Sprite>("Cup_thum" + "/Topping_" + recipe.drink.topping); else toppingSprite.sprite = null;
        //if (outline != null) outline.SetOutlineDrink();

        //액체에 색상넣기
        for (int j = 0; j < recipe.drink.colors.Count; j++)
        {
            LiquidDiv[j].color = recipe.drink.colors[j];
        }

    }
    public void ResetDrink()
    {
        for (int i = 0; i < LiquidDiv.Count; i++)
        {
            LiquidDiv[i].color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0);
        }

    }
}
