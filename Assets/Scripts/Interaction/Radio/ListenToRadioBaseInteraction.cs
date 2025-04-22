using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenToRadioBaseInteraction : BaseInteraction
{
    protected string radioText;
    [SerializeField]
    protected int radioTextDuration;

    [SerializeField]
    [Tooltip("Texts to to string together to from a radio notifcation")]
    [TextArea(1, 4)]
    protected List<string> radioTexts = new();

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
        interactionOutcome = "";
        Radio radio = thisItem as Radio;
    }
}

