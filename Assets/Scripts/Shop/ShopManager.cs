using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public AudioSource BuySfx;
    public PlayerResourceData resourceData;

    //List of all items in the shop scene, item being a product the player can buy.
    public List<ShopItem> boughtItemsList = new List<ShopItem>();

    public List<TextMeshProUGUI> ShoppingListList;
    public TextMeshProUGUI BasicResources;
    public GameObject purchaseConfirmation;
    public TextMeshProUGUI shopNotifTextField;

    [HideInInspector]
    public GameObject SelectedItemGO;
    
    [SerializeField]
    private bool debug = false;

    private string BoughtStringSaver = "";
    private ShopStockSaver shopStockSaver;
  
    // Initialise things in start
    void Start()
    {
        shopStockSaver = FindObjectOfType<ShopStockSaver>();
        resourceData = FindObjectOfType<PlayerResourceData>();
        if (debug)
            resourceData.MoneyAmount = 500;

        InitializeBoolItems();
        UpdateInventoryList();

    }

    public void YesButton()
    {
        ShopItem shopItem = SelectedItemGO.GetComponent<ShopItem>();
        //Check if item can be afforded
        if (resourceData.MoneyAmount >= shopItem.ShopPrice)
        {
            ShopShelfBase selectedShelf = SelectedItemGO.GetComponent<ShopShelfBase>();
            BoolItem boolItem = SelectedItemGO.GetComponent<BoolItem>();
            //Check item type, handle accordingly
            //Food
            if (SelectedItemGO.tag == "Food" || SelectedItemGO.tag == "Drink")
            {

                selectedShelf.BuyItem(resourceData);

                //Add the bought item to the list, which is used for undo functionality
                //Add bought items name in shopping list
                boughtItemsList.Add(selectedShelf);
                UpdateInventoryList();
            }
            //Single buy items, either simlar else if or create a switch case
            else if (SelectedItemGO.GetComponent<BoolItem>())
            {
                BuyBoolItem(boolItem);

            }
            else
                Debug.Log("Attempted to buy unknown item type!");
        }
        else
        {
            shopNotifTextField.transform.parent.gameObject.SetActive(true);
            shopNotifTextField.gameObject.SetActive(true);
            shopNotifTextField.text = "Can't afford!";
        }
        purchaseConfirmation.gameObject.SetActive(false);

    }

    public void YesBuy5Button()
    {
        BoolItem boolItem = SelectedItemGO.GetComponent<BoolItem>();
        if (boolItem != null)
        {
            shopNotifTextField.gameObject.SetActive(true);

            shopNotifTextField.text = "Can't buy multiple bool items";
            return;
        }

        ShopItem shopItem = SelectedItemGO.GetComponent<ShopItem>();
        if (shopItem != null)
        {
            //Check if item can be afforded
            if (resourceData.MoneyAmount >= shopItem.ShopPrice * 5)
            {
                ShopShelfBase selectedShelf = SelectedItemGO.GetComponent<ShopShelfBase>();

                for (int i = 0; i <= 4; i++)
                {

                    selectedShelf.BuyItem(resourceData);

                    //Add the bought item to the list, which is used for undo functionality
                    //Add bought items name in shopping list

                    boughtItemsList.Add(selectedShelf);
                }
                UpdateInventoryList();

            }
            else
            {
                shopNotifTextField.transform.parent.gameObject.SetActive(true);
                shopNotifTextField.gameObject.SetActive(true);
                shopNotifTextField.text = "Can't afford!";
            }
        }
        else
            Debug.Log("Attempted to buy unknown item type!");

    }

    public void NoButton()
    {
        purchaseConfirmation.gameObject.SetActive(false);
    }

    private void BuyBoolItem(BoolItem boolItem)
    {
        boolItem.BuyItem(resourceData);
        boughtItemsList.Add(boolItem);
        UpdateInventoryList();

        SelectedItemGO.SetActive(false);

    }

    private void InitializeBoolItems()
    {
        FindObjectOfType<ShopRadioItem>().InitializeSelf(resourceData);
        FindObjectOfType<ShopPortableStoveItem>().InitializeSelf(resourceData);
    }

    public void UpdateInventoryList()
    {
        BasicResources.text = resourceData.MoneyAmount.ToString() + "€ <br>" +
            "Meat: " + resourceData.MeatFoodAmount.ToString() + "<br>" +
            "Vegetables: " + resourceData.VegetableFoodAmount.ToString() + "<br>" +
            "Pasta: " + resourceData.PastaFoodAmount.ToString() + "<br>" +
            "Canned Foods: " + resourceData.CannedFoodAmount.ToString() + "<br>" +
           "Chips: " + resourceData.ChipsFoodAmount.ToString() + "<br>" +
            "Milk: " + resourceData.MilkDrinkAmount.ToString() + "<br>" +
            "Water: " + resourceData.WaterDrinkAmount.ToString() + "<br>" +
            "Juice: " + resourceData.JuiceDrinkAmount.ToString();
    }

    //Needs to be fixed, keep logic similar for sake of simplicity
    public void UndoPurchase()
    {
        //gets the newest item in list
        int boughtItemsCount = boughtItemsList.Count;
        ShopItem newest = boughtItemsList[boughtItemsCount - 1];
        if (boughtItemsList.Any())
        {
            //checks what type of item it is and then gives back money and takes away resources
            if (newest.GetComponent<BoolItem>())
            {
                newest.gameObject.SetActive(true);
                newest.UndoBuy(resourceData);

                //Remove the item from list
                boughtItemsList.RemoveAt(boughtItemsCount - 1);
                UpdateInventoryList();
            }

            else if (newest.GetComponent<ShopItem>())
            {

                newest.UndoBuy(resourceData);

                //Remove the item from list
                boughtItemsList.RemoveAt(boughtItemsCount - 1);
                UpdateInventoryList();
            }
            else
            {
                Debug.Log("Unknown item in undo list!");
            }
        }
    }


    public void SaveAndChangeScene()
    {
        FindObjectOfType<ShopStockSaver>().SaveAndChangeScene();
    }

}
