using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSink_Interaction : BaseInteraction
{

    [SerializeField]
    private int waterPerUse = 10;
    [SerializeField]
    private string unpoweredSinkNotification = "";
    [SerializeField]
    private string unwateredSinkNotification = "";
    [SerializeField]
    private string noContainersSinkNotification = "";
    private SinkItem sink;
    private PlayerResourceData resourceData;
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
        sink.drinking = true;
    }

    //Drink from tap
    public void SinkDrinkTick()
    {
        //if has electrcity and water, normal increase
        if (!sink.NoPower && !sink.NoWater)
        {
            
            interactionManager.AdjustPlayerHydration(waterPerUse);
            Debug.Log("Player hydration: " + interactionManager.GetPlayerHydration());

            if (interactionManager.GetPlayerHydration() >= 100)
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
                interactionManager.ShowNoticationText(unwateredSinkNotification, 5);
            }
            else if (sink.SinkContainersInUse <= 0)
            {
                interactionManager.ShowNoticationText(noContainersSinkNotification, 5);
            }
            EndInteraction();
            
        }
    }

    protected override void EndInteraction()
    {
        resourceData.usedLiquidContainers = sink.SinkContainersInUse;
        sink.drinking = false;
        base.EndInteraction();

    }
}
