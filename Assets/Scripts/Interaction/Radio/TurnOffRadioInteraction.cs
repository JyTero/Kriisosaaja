using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffRadioInteraction : BaseInteraction
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    //Upon Clicking the interaction from object
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

        DuringInteraction();

    }

    //The "Main" of the Interaction
    protected virtual void DuringInteraction()
    {
        var radio = thisItem as Radio;

        radio.UnsubscribeAllRadioECs();
        EndInteraction();
    }
}
