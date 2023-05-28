using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeInfo : MonoBehaviour
{
    public RecipeManager recipeManager;
    // Start is called before the first frame update
    public void OpenRecipeInfo(Recipe recipe)
    {
        gameObject.SetActive(true);
        recipeManager.MakeRecipe(recipe);
    }
}
