using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeInfo : MonoBehaviour
{
    public RecipeManager recipeManager;
    public GameObject RecipeHintBox;
    public TouchLock touchLock;
    private int RecipeHinePrice = 1000;
    // Start is called before the first frame update
    public void OpenRecipeInfo(Recipe recipe)
    {
        gameObject.SetActive(true);
        recipeManager.MakeRecipe(recipe);
        touchLock.SetOn();
    }
    public void CloseRecipeInfo()
    {
        gameObject.SetActive(false);
        touchLock.SetOff();
    }


    public void RecipeHintOpen()
    {
        if (State.instance.MinusMoney(RecipeHinePrice))
        {
            RecipeHintBox.SetActive(false);
        }
    }
}
