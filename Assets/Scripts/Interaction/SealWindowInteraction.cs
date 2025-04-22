using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SealWindowInteraction : BaseInteraction
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
windowItem.sealingWindow = true;       
    }

    public void SealWindowTick()
    {
        timer++;

        if (timer >= sealingTime)
        {
            SealWindow();
            EndInteraction();
        }
    }
    private void SealWindow()
    {

        thisItem.itemManager.WindowIsSealed = true;
        onWindowSealChangeEC.RaiseEvent(true);

        //Remove Sealing from item interactions
        this.enabled = false;
        thisItem.Interactions.Remove(this);

        //Add Unseal window to item interactions
        windowItem.unsealWindowInteraction.enabled = true;
        thisItem.Interactions.Add(windowItem.unsealWindowInteraction);  
    }

    protected override void EndInteraction()
    {
        timer = 0;
        windowItem.sealingWindow = false;
        base.EndInteraction();
    }

    public void SealWindowOnLoad()
    {
        SealWindow();
        windowItem.sealingWindow = false;
    }
}
