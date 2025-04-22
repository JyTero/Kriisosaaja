using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class PlayerResourceData : MonoBehaviour
{
    public float foodCounter;
    public int MeatFoodAmount;
    public int VegetableFoodAmount;
    public int PastaFoodAmount;
    public int CannedFoodAmount;
    public int ChipsFoodAmount;
    public int MilkDrinkAmount;
    public int WaterDrinkAmount;
    public int JuiceDrinkAmount;
    public float drinkCounter;
    public float bookCounter;
    public float MoneyAmount;
    public int LiquidContainersCapasity;
    public int usedLiquidContainers;
    public bool GotRadio;
    public bool GotVaraVirta;
    public bool GotPortableStove;

    public int PhoneEnergy;
    public int VaraVirtaEnergy;
    public bool scenario3Start = false;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void ResetPlayerResourceData()
    {
        MoneyAmount = 500;
        MeatFoodAmount = 0;
        VegetableFoodAmount = 0;
        PastaFoodAmount = 0;
        CannedFoodAmount = 0;
        ChipsFoodAmount = 0;
        MilkDrinkAmount = 0;
        WaterDrinkAmount = 0;
        JuiceDrinkAmount = 0;
        LiquidContainersCapasity = 10;
        usedLiquidContainers = 0;
        GotRadio = false;
        GotVaraVirta = false;
        GotPortableStove = false;
    }

}
