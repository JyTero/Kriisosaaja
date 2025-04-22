using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnsealWindowInteraction : BaseInteraction
{

    [SerializeField]
    private int sealingTime = 0;
    [SerializeField]
    private BoolEventChannelSO onWindowSealChangeEC;

    private int timer = 0;

    private WindowItem windowItem;
    protected override void Start()
    {
        base.Start();
        windowItem = thisItem as WindowItem;
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

        DuringInteraction();

    }

    //The "Main" of the Interaction
    private void DuringInteraction()
    {
        windowItem.unsealingWindow = true;
    }

    public void UnsealWindowTick()
    {
        timer++;
        if (timer >= sealingTime) 
        {
            thisItem.itemManager.WindowIsSealed = false;
            onWindowSealChangeEC.RaiseEvent(false);

            //To trigger check if inside air is lethal
            interactionManager.AdjustWorldAirQ(interactionManager.GetWorldAirQ());

            //Remove unseal from interactions
            this.enabled = false;
            thisItem.Interactions.Remove(this);
            //Add seal to interactions
            windowItem.sealWindowInteraction.enabled = true;
            thisItem.Interactions.Add(windowItem.sealWindowInteraction);

            EndInteraction();
            
        }
    }

    protected override void EndInteraction()
    {
        timer = 0;
        windowItem.unsealingWindow= false;
        base.EndInteraction();
    }
}
