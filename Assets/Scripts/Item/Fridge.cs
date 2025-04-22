using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Fridge : BaseItem, IRequiresPower, IFoodStorage
{

    public GameObject slotParent;
    // public List<SurfaceSlot> SurfaceSlots = new();
    [HideInInspector]
    public bool NoPower { get; set; }
    [HideInInspector]
    public int powerOutageTime = -1;
    [HideInInspector]
    public int TotalOutageDuration = 0;

    public List<SurfaceSlot> SurfaceSlots { get; set; }
    public Transform SlotParentT { get; set; }

    private List<BaseInteraction> meatInteractions = new();
    private List<BaseInteraction> milkInteractions = new();

    protected override void Start()
    {
        base.Start();

        SurfaceSlots = new();

        SignUpOnItemManagerRequiresPower();

        // add each child (slot) under slot parent to a list for later access.
        // add each slot to slot "master list" for cooking access
        if (slotParent != null)
        {
            SurfaceSlots.Clear();
            foreach (Transform t in slotParent.transform)
            {
                SurfaceSlots.Add(t.gameObject.GetComponent<SurfaceSlot>());
                itemManager.AllSlots.Add(t.gameObject.GetComponent<SurfaceSlot>());
            }
        }
        else
            Debug.LogWarning("Slot parent not assigned for GO " + gameObject.name);

        SlotParentT = SurfaceSlots[0].transform.parent;
        StartCoroutine(FoodStorageBegin());
    }

    private IEnumerator FoodStorageBegin()
    {
        //allow interactions to finish their start() 
        yield return new WaitUntil(() => AreFoodInteractionsInitialised());

        //Sort food related interactions by fooditem type for later de/reactiovation
        foreach (BaseInteraction interaction in Interactions)
        {
            IFoodItemInteraction foodItemInteraction = interaction as IFoodItemInteraction;
            switch (foodItemInteraction.GetFoodItemType())
            {
                case GlobalValues.FoodItemType foodType when foodType == GlobalValues.FoodItemType.Meat:
                    meatInteractions.Add(interaction);
                    break;
                case GlobalValues.FoodItemType foodType when foodType == GlobalValues.FoodItemType.Milk:
                    milkInteractions.Add(interaction);
                    break;
                default:
                    break;
            }
        }
        //remove all disabled interactions. Interactions disable themselves on start if they're not valid
        Interactions.RemoveAll(interaction => !interaction.enabled);

    }

    public void FoodLeftCheck(PlayerResourceData playerResourseData)
    {
        //If no food remain, disable related interactions
        if (playerResourseData.MeatFoodAmount <= 0)
        {
            foreach (BaseInteraction interaction in meatInteractions)
            {
                interaction.enabled = false;
            }
        }
        if (playerResourseData.MilkDrinkAmount <= 0)
        {
            foreach (BaseInteraction interaction in milkInteractions)
            {
                interaction.enabled = false;
            }
        }
        Interactions.RemoveAll(interaction => !interaction.enabled);

    }
    public void LoadItemToSlot(GameObject item, int slotIndex, bool isSpoiled, int spoilTime)
    {

        item.transform.SetParent(slotParent.transform.GetChild(slotIndex));
        SurfaceSlots[slotIndex].itemInSlot = item;

        item.GetComponent<ConsumableBase>().HasSpoiled = isSpoiled;
        item.GetComponent<ConsumableBase>().WillSpoilTime = spoilTime;

    }


    protected override void  TimeBeat()
    {
        if (NoPower)
        {
            TotalOutageDuration++;
        }
        
    }

    public void OnPowerOuttageBegin()
    {
        Debug.Log("The Fridge is sad due to lack of power");

        powerOutageTime = itemManager.GetTimeManagerTime();
        NoPower = true;
       
    }

    public void OnPowerResume()
    {
        NoPower = false;
    }

    public void SignUpOnItemManagerRequiresPower()
    {
        //Add self to a list of items requiring electrcity 
        itemManager.requiresPowerItems.Add(this);
    }

    public void SignOffFromItemManagerRequiresPower()
    {
        itemManager.requiresPowerItems.Remove(this);
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

    private void OnDisable()
    {
        SignOffFromItemManagerRequiresPower();
    }
}
