using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public DrinkManager orderDrink;

    public void OpenOrderBalloon(Recipe orderRecipe)
    {
        orderDrink.MakeDrink(orderRecipe.drink);
    }
}
