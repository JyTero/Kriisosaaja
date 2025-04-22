using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillContainerSink_Interaction : BaseInteraction
{
    [SerializeField]
    private int containersUsedIncreasePerTick = 1;

    [SerializeField]
    private string unpoweredSinkNotification = "";
    [SerializeField]
    private string unwateredSinkNotification = "";
    [SerializeField]
    private string noContainersSinkNotification = "";

    private PlayerResourceData resourceData;
    private SinkItem sink;
    protected override void Start()
    {
        base.Start();
        sink = gameObject.GetComponent<SinkItem>();
        resourceData = FindFirstObjectByType<PlayerResourceData>();
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
        sink.fillingContainer = true;
    }

    [SerializeField]
    private BoolEventChannelSO onUnpoweredSinkUse;

    public void SinkTick()
    {
        //if has electrcity and water, normal increase
        if (!sink.NoPower && !sink.NoWater && sink.SinkContainersInUse < 10)
        {
            sink.SinkContainersInUse++;
            resourceData.WaterDrinkAmount++;
            Debug.Log("Filled container with water. Remaining containers: " + (sink.SinkContainerCapasity - sink.SinkContainersInUse));
            
            if(sink.SinkContainersInUse >= 10) 
            {
                EndInteraction();
                return;
            }   
        }

        //else raise event, no water / power
        else
        {
            if (sink.NoPower)
            {
                interactionManager.ShowNoticationText(unpoweredSinkNotification, 5);
            }
            else if (sink.NoWater)
            {
                interactionManager.ShowNoticationText(unpoweredSinkNotification, 5);
            }

            EndInteraction();
            onUnpoweredSinkUse.RaiseEvent(true);
        }
    }
    protected override void EndInteraction()
    {
        resourceData.usedLiquidContainers = sink.SinkContainersInUse;
        sink.fillingContainer = false;
        base.EndInteraction();

    }
}
