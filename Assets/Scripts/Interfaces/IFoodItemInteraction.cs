using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValues;

public interface IFoodItemInteraction
{
    //Used to filter food related interactions by which food type they consumed so they may be added to seperate lists.
    public GlobalValues.FoodItemType GetFoodItemType()
    {
        return FoodItemType.Default;
    }
}
