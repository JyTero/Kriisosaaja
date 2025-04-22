using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseBathroomDry_Interaction : BaseInteraction
{
    [SerializeField]
    private int bathroomIncreasePerTick;

    private BathroomItem bathroom;
    [SerializeField]
    private BoolEventChannelSO onPlayerGoesAwayeEC;
    protected override void Start()
    {
        base.Start();
        bathroom = gameObject.GetComponent<BathroomItem>();
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
        onPlayerGoesAwayeEC.RaiseEvent(true);
        DuringInteraction();

    }


    //The "Main" of the Interaction
    private void DuringInteraction()
    {
        bathroom.dryBathroomInUse = true;
    }

    public void ToiletTick()
    {
        interactionManager.AdjustPlayerBathroom(bathroomIncreasePerTick);

        if (interactionManager.GetPlayerBathroom() >= 70)
        {
            EndInteraction();
        }
    }
    protected override void EndInteraction()
    {
        bathroom.dryBathroomInUse = false;
        onPlayerGoesAwayeEC.RaiseEvent(false);
        base.EndInteraction();

    }
}
