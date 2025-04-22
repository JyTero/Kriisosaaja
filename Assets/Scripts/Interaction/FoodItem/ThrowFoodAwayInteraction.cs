using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFoodAwayInteraction : BaseInteraction
{
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
        DuringInteraction();

    }


    //The "Main" of the Interaction
    private void DuringInteraction()
    {
        Debug.Log("Threw " + GetComponent<ConsumableBase>().ConsumableName + " away");
        EndInteraction();
        Destroy(gameObject);
    }
}
