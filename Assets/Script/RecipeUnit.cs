using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUnit : MonoBehaviour
{
    public GameObject Shadow;
    public Text Price;
    public RecipeManager recipeManager;
   
    public void OpenRecipeInfo()
    {
        if(recipeManager.recipeName.text != "???")
        {
            RecipeInfo recipeInfo = GameObject.Find("Canvas").gameObject.transform.Find("RecipeInfo").gameObject.GetComponent<RecipeInfo>();
            if (recipeInfo != null) recipeInfo.OpenRecipeInfo(recipeManager.recipe);
        }
        
    }
    public void ShowRecipe()
    {
        Price.text = string.Format("{0:#,0}", recipeManager.recipe.price);

    }
    public void HideRecipe()
    {
        Shadow.gameObject.SetActive(true);
        Price.text = "???";
        recipeManager.recipeName.text = "???";
        recipeManager.gameObject.SetActive(false);
    }
}
