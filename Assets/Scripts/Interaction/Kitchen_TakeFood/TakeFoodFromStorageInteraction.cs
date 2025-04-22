using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TakeFoodFromStorageBase : BaseInteraction, IFoodItemInteraction
{
    [SerializeField]
    public GlobalValues.FoodItemType FoodType;

    protected override void Awake()
    {
        base.Awake();
    }


    public override void OnInteractionBegin()
    {
        base.OnInteractionBegin();
        // Route to interaction spot, if applicaple 
        playerAtDestinationEC.OnEventRaised += AtDestination;
        interactionManager.SetDestination(thisItem.InteractionSpotTransform.position);
        // Once routed, begin interaction animations by getting alert from Player via InteractionManager
    }

    //Raised when player arrives to their current destination
    protected override void AtDestination()
    {
        base.AtDestination();
        //Stuff player does when arriving to the interaction spot
        //In this case it could include changing to outdoor clothing, opening door etc.

        DuringInteraction();

    }


    //The "Main" of the Interaction
    protected virtual void DuringInteraction()
    {
        var foodStorage = (IFoodStorage)thisItem;

        int i = 1;
        // Create Food Object
        foreach (SurfaceSlot slot in foodStorage.SurfaceSlots)
        {
            //If slot is empty
            if (slot.itemInSlot == null)
            {
                GameObject slotItem = InstantiateConsumablePrefab(slot);

                var consumableBase = slotItem.GetComponent<ConsumableBase>();
                consumableBase.HasSpoiled = SpoiltInStorage(consumableBase);
                consumableBase.WillSpoilTime = interactionManager.GetManagerTime() + consumableBase.FoodSpoilTime;

                slotItem.name = consumableBase.ConsumableName;
                slot.itemInSlot = slotItem;
                if(consumableBase.HasSpoiled)
                {
                    interactionManager.ShowNoticationText("Ewh, this food smells bad, it may be spoiled", 0);
                }
                break;
            }
            else
            {
                if (i == foodStorage.SurfaceSlots.Count)
                {
                    interactionManager.ShowNoticationText("There's no space on the surface to take food out.", 0);
                    break;
                }
                i++;
                continue;
            }
        }

        //PostInteraction
        interactionManager.UpdateResourcesToList();
        foodStorage.FoodLeftCheck(interactionManager.PlayerResourceData);

        EndInteraction();
    }
    private bool SpoiltInStorage(ConsumableBase consumable)
    {
        var fridge = thisItem as Fridge;
        if (fridge == null)
        {
            return false;
        }

        else if (fridge.TotalOutageDuration > consumable.FoodSpoilTime)
        {
            return true;
        }
        else
        {
            consumable.spoilProgeres = fridge.TotalOutageDuration;
            return false;
        }
    }

    //Used via IFoodItemInteraction interface to get food type
    public virtual GlobalValues.FoodItemType GetFoodItemType()
    {
        return FoodType;
    }

    protected virtual GameObject InstantiateConsumablePrefab(SurfaceSlot slot)
    {

        return null;
    }


}
