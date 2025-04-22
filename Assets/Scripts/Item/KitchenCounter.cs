using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class KitchenCounter : BaseItem, IFoodStorage
{
    public GameObject slotParent;

    private List<BaseInteraction> vegetableInteractions = new();
    private List<BaseInteraction> pastaInteractions = new();
    private List<BaseInteraction> cannedFoodInteractions = new();
    private List<BaseInteraction> waterInteractions = new();
    private List<BaseInteraction> juiceInteractions = new();

    public List<SurfaceSlot> SurfaceSlots { get; set; }
    public Transform SlotParentT { get; set; }


    protected override void Awake()
    {
        base.Awake();

        SurfaceSlots = new();

        Debug.Log($"Number of interactions in {ItemName}: {Interactions.Count}");

        // add each child (slot) under slot parent to a list for later access.
        // add each slot to slot "master list" for cooking access
        if (slotParent != null)
        {
            foreach (Transform t in slotParent.transform)
            {
                SurfaceSlots.Add(t.gameObject.GetComponent<SurfaceSlot>());
                itemManager.AllSlots.Add(t.gameObject.GetComponent<SurfaceSlot>());
            }
        }
        else
            Debug.LogWarning("Slot parent not assigned for GO " + gameObject.name);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();


        SlotParentT = SurfaceSlots[0].transform.parent;
        StartCoroutine(FoodStorageBegin());
    }

    IEnumerator FoodStorageBegin()
    {
        //allow interactions to finish their start() 
        yield return new WaitUntil(() => AreFoodInteractionsInitialised());

        //Sort food related interactions by fooditem type for later de/reactiovation
        foreach (BaseInteraction interaction in Interactions)
        {
            IFoodItemInteraction foodItemInteraction = interaction as IFoodItemInteraction;
            switch (foodItemInteraction.GetFoodItemType())
            {
                case GlobalValues.FoodItemType foodtype when foodtype == GlobalValues.FoodItemType.Vegetable:
                    vegetableInteractions.Add(interaction);
                    break;
                case GlobalValues.FoodItemType foodtype when foodtype == GlobalValues.FoodItemType.Pasta:
                    pastaInteractions.Add(interaction);
                    break;
                case GlobalValues.FoodItemType foodType when foodType == GlobalValues.FoodItemType.CannedFood:
                    cannedFoodInteractions.Add(interaction);
                    break;
                case GlobalValues.FoodItemType foodType when foodType == GlobalValues.FoodItemType.Water:
                    waterInteractions.Add(interaction);
                    break;
                case GlobalValues.FoodItemType foodtype when foodtype == GlobalValues.FoodItemType.Juice:
                    juiceInteractions.Add(interaction);
                    break;
                //Similar cases for rest of the food types as they get implemented
                default:
                    break;
            }
        }
        //remove all disabled interactions. Interactions disable themselves on start if they're not valid
        Interactions.RemoveAll(interaction => !interaction.enabled);
    }

    public void LoadItemToSlot(GameObject item, int slotIndex, bool isSpoiled, int spoilTime)
    {
        item.transform.SetParent(slotParent.transform.GetChild(slotIndex));
        SurfaceSlots[slotIndex].itemInSlot = item;

        item.GetComponent<ConsumableBase>().HasSpoiled = isSpoiled;
        item.GetComponent<ConsumableBase>().WillSpoilTime = spoilTime;
    }

    //Void to check if food remains, if not, remove action(s)
    //from Item's interactions list
    public void FoodLeftCheck(PlayerResourceData playerResourseData)
    {
        //If no food remain, disable related interactions
        if (playerResourseData.VegetableFoodAmount <= 0)
        {
            foreach (BaseInteraction interaction in vegetableInteractions)
            {
                interaction.enabled = false;
            }
        }
        if (playerResourseData.PastaFoodAmount<= 0)
        {
            foreach(BaseInteraction interaction in pastaInteractions)
            {
                interaction.enabled = false;
            }
        }
        if (playerResourseData.CannedFoodAmount <= 0)
        {
            foreach (BaseInteraction interaction in cannedFoodInteractions)
            {
                interaction.enabled = false;
            }
        }
        if (playerResourseData.WaterDrinkAmount <= 0)
        {
            foreach (BaseInteraction interaction in waterInteractions)
            {
                interaction.enabled = false;
            }
        }
        if (playerResourseData.JuiceDrinkAmount <= 0)
        {
            foreach (BaseInteraction interaction in juiceInteractions)
            {
                interaction.enabled = false;
            }
        }
        Interactions.RemoveAll(interaction => !interaction.enabled);
    }


    private bool AreFoodInteractionsInitialised()
    {
        foreach (BaseInteraction interaction in Interactions)
        {
            if (interaction.InteractionInitialised)
                continue;
            else
                return false;
        }
        return true;
    }
}
