using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatFoodFromStorageBase : BaseInteraction
{
    public GlobalValues.FoodItemType FoodType;

    public override void OnInteractionBegin()
    {
        base.OnInteractionBegin();
        // Route to interaction spot, if applicaple 
        playerAtDestinationEC.OnEventRaised += AtDestination;
        interactionManager.SetDestination(thisItem.InteractionSpotTransform.position);
    }

    //Raised when player arrives to their current destination
    protected override void AtDestination()
    {
        base.AtDestination();

        DuringInteraction();

    }

    //The "Main" of the Interaction
    protected virtual void DuringInteraction()
    {
        //Create food
        ConsumableBase consumable = GetConsumableScript();
        consumable.HasSpoiled = SpoiltInStorage(consumable);

        //Consume food
        consumable.EatFood(interactionManager, consumable);

        //PostInteraction
        var foodStorage = (IFoodStorage)thisItem;
        foodStorage.FoodLeftCheck(interactionManager.PlayerResourceData);

        interactionManager.UpdateResourcesToList();
        if (consumable.HasSpoiled)
        {
            interactionManager.ShowNoticationText("Ewh, this food smells bad, it may be spoiled", 0);
        }

        EndInteraction();
    }
    //Check if the food should've spoiled while in the storage
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

    protected virtual ConsumableBase GetConsumableScript()
    {
        return null;
    }
}
