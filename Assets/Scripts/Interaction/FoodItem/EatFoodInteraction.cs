using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatFoodInteraction : BaseInteraction
{
    [SerializeField]
    private VoidEventChannelSO onDangerousFoodEatenEC;

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
    private void DuringInteraction()
    {
        var consumable = GetComponent<ConsumableBase>();
        consumable.EatFood(interactionManager, consumable);
        
        if (consumable.HasSpoiled)
            onDangerousFoodEatenEC.RaiseEvent();
        else if (consumable.foodType == GlobalValues.FoodItemType.Meat)
            onDangerousFoodEatenEC.RaiseEvent();
        EndInteraction();

        Destroy(gameObject);

    }
}
