using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GlobalValues;

public class ScenarioShop : MonoBehaviour
{
    [SerializeField]
    private ShopShelfVegetables vegetableShelf;
    [SerializeField]
    private ShopShelfMeat meatShelf;
    [SerializeField]
    private ShopShelfPasta pastaShelf;
    [SerializeField]
    private ShopShelfCannedFood cannedFoodShelf;
    [SerializeField]
    private ShopShelfMilk milkShelf;
    [SerializeField]
    private ShopShelfWater waterShelf;
    [SerializeField]
    private ShopShelfJuice juiceShelf;
    [SerializeField]
    private BoolItem radioBitem;
    [SerializeField]
    private BoolItem portableStoveBitem;
    [SerializeField]
    private bool isDebug;
    [SerializeField]
    private ScenarioSO scenarioShopDebug;

    private ShopStockSaver stockSaver;

    // Start is called before the first frame update
    void Start()
    {
        //DEBUG
        if (isDebug)
            CurrentScenario = scenarioShopDebug;

        //if not shop secnario, disable this
        if (CurrentScenario == null)
        {
            this.enabled = false;
            return;
        }


        stockSaver = FindObjectOfType<ShopStockSaver>();
        stockSaver.LoadShopState();
        PrepareScenarioShop();
    }

    private void PrepareScenarioShop()
    {
        //things to get refrence to scenario shop rules list ("scenarioShopItems")
        
        foreach (ScenarioShopSellableData scenarioSellable in CurrentScenario.ScenarioShopItems)
        {
            //Get food type, act accordingly
            switch (scenarioSellable.ScenarioShopSellableFood)
            {
                case FoodItemType foodType when foodType == FoodItemType.Default:
                    //bool item figure outtage
                    FigureBoolItem(scenarioSellable);
                    break;

                case FoodItemType foodType when foodType == FoodItemType.Vegetable:
                    vegetableShelf.gameObject.SetActive(true);
                    AdjustPriceAndStock(vegetableShelf, scenarioSellable);
                    break;

                case FoodItemType foodType when foodType == FoodItemType.Meat:
                    meatShelf.gameObject.SetActive(true);
                    AdjustPriceAndStock(meatShelf, scenarioSellable);
                    break;

                case FoodItemType foodType when foodType == FoodItemType.Pasta:
                    pastaShelf.gameObject.SetActive(true);
                    AdjustPriceAndStock(pastaShelf, scenarioSellable);
                    break;

                case FoodItemType foodType when foodType == FoodItemType.CannedFood:
                    cannedFoodShelf.gameObject.SetActive(true);
                    AdjustPriceAndStock(cannedFoodShelf, scenarioSellable);
                    break;

                case FoodItemType foodType when foodType == FoodItemType.Milk:
                    milkShelf.gameObject.SetActive(true);
                    AdjustPriceAndStock(milkShelf, scenarioSellable);
                    break;

                case FoodItemType foodType when foodType == FoodItemType.Water:
                    waterShelf.gameObject.SetActive(true);
                    AdjustPriceAndStock(waterShelf, scenarioSellable);
                    break;

                case FoodItemType foodType when foodType == FoodItemType.Juice:
                    juiceShelf.gameObject.SetActive(true);
                    AdjustPriceAndStock(juiceShelf, scenarioSellable);
                    break;

                default:
                    Debug.LogError("Unknown foodtype in scenarioShopItems!");
                    break;

            }
        }
    }


    private void AdjustPriceAndStock(ShopShelfBase shelf, ScenarioShopSellableData scenarioSellable)
    {
        shelf.ShopPrice = shelf.ShopPrice * scenarioSellable.ScenarioPriceMultiplier;

        //IF scenario stock is less than shop stock, always apply shop stock
        if (!FirstShopVisitDone)
        {
            if (scenarioSellable.ScenarioStock < shelf.stockLeft)
            {
                shelf.stockLeft = scenarioSellable.ScenarioStock;
            }
            //Otherwise if shop stock is less than scenario stock AND restock is enabled, apply that
            else if (scenarioSellable.Restock)
            {
                shelf.stockLeft = scenarioSellable.ScenarioStock;
            }

            if (shelf.stockLeft <= 0)
            {
                shelf.enabled = false;
            }
        }
    }
    private void FigureBoolItem(ScenarioShopSellableData scenarioSellable)
    {
        //Each bool item has its own if check
        if (scenarioSellable.ScenarioShopSellableBoolItem.GetComponent<ShopRadioItem>())
        {
            if (radioBitem != null)
            {
                if (scenarioSellable.ScenarioStock == 0)
                    radioBitem.gameObject.SetActive(false);
                else
                    radioBitem.ShopPrice = radioBitem.ShopPrice * scenarioSellable.ScenarioPriceMultiplier;
            }
        }
        else if (scenarioSellable.ScenarioShopSellableBoolItem.GetComponent<ShopPortableStoveItem>())
        {
            if (portableStoveBitem != null)
            {
                if (scenarioSellable.ScenarioStock == 0)
                    portableStoveBitem.gameObject.SetActive(false);
                else
                    portableStoveBitem.ShopPrice = portableStoveBitem.ShopPrice * scenarioSellable.ScenarioPriceMultiplier;
            }
        }
    }

    private void OnDestroy()
    {
        if (this.enabled == true)
            FirstShopVisitDone = true;
    }
}
