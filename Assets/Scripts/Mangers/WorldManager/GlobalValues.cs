using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class storing values to be usable by any class in any scene
public static class GlobalValues
{
    //Used for information quality, air quality etc.
    public enum Quality
    {
        Dangerous,
        Terrible,
        Bad,
        Normal,
        Good,
    }

    public enum Weather
    {
        Clear,
        Snow,
        Rain,
        Thunder,
        Blizzard,
        Cloudy,
        //  Sunshine,
    }

    //Used to sort out foods and to give different effects
    public enum FoodItemType
    {
        Default,
        Meat,
        Vegetable,
        Pasta,
        CannedFood,
        Milk,
        Water,
        CarbonatedDrink,
        Juice,
        Chips,
    }
    //Time passed in scenario scene
    public static int SceneTime = -1;

    //Slots of items on top of surfaces before scene exit
    public static List<SavedSlot> savedSlots = new();

    //Player Values 
    public static List<int> PlayerValues = new();

    //World Values
    public static List<int> WorldValues = new();

    //Used to hande scenario shop
    public static ScenarioSO CurrentScenario;
    public static int ScenarioSituationIndex = 0;
    public static int ScenarioSituationTimer = 0;

    //ShopStock saver
    public static Dictionary<int, int> ShopStockLeft;
    public static List<GameObject> AvailableShopBoolItems = new();
    public static bool FirstShopVisitDone = false;

    //Item state
    public static bool DryToiletPrepared = false;
    public static bool WinbowSealed = false;
    public static int UsedUnpoweredToiletUsedCapasity = -1;

    //Scoring
    public static List<string> scoringStrings = new();
    public static List<int> scoringScores = new();


    public static void ClearGlobalValues()
    {
        SceneTime = -1;

        savedSlots = new();

        PlayerValues = new();

        WorldValues = new();


        CurrentScenario = null;
        ScenarioSituationIndex = 0;
        ScenarioSituationTimer = 0;

        ShopStockLeft = new();
        AvailableShopBoolItems = new();
        FirstShopVisitDone = false;

        DryToiletPrepared = false;
        DryToiletPrepared = false;
        UsedUnpoweredToiletUsedCapasity = -1;


        scoringStrings = new();
        scoringScores = new();
    }
}

