using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDryToilet_Interaction : BaseInteraction
{
    [HideInInspector]
    public bool dryBathroomPrep = false;
    [SerializeField]
    private int dryToiletRemoveTime = 0;
    [SerializeField]
    private BoolEventChannelSO onDrytoiletPrep;

    [SerializeField]
    private BoolEventChannelSO onPlayerGoesAwayeEC;

    private BathroomItem bathroom;
    private int toiletPrepTimer = 0;


    protected override  void Start()
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
        bathroom.dryBathroomBeingRemoved = true;
        //disable this interaction
        this.enabled = false;
    }
    public void RemoveDryToiletTick()
    {
        if (toiletPrepTimer >= dryToiletRemoveTime)
        {
            //Remove remove dry toilet from item interactions
            this.enabled = false;
            bathroom.Interactions.Remove(this);
            //Remove using dry toilet from item interactions
            bathroom.useDryBathroomInteraction.enabled = false;
            thisItem.Interactions.Remove(bathroom.useDryBathroomInteraction);
            //If hasWater, Add using normal toilet to interactions
            if (!bathroom.NoWater)
            {
                bathroom.useBathroomInteraction.enabled = transform;
                thisItem.Interactions.Add(bathroom.useBathroomInteraction);
            }
            //Add prepare Dry toilet interaction
            bathroom.prepareDryToiletInteraction.enabled = true;
            thisItem.Interactions.Add(bathroom.prepareDryToiletInteraction);

            onDrytoiletPrep.RaiseEvent(false);
            EndInteraction();
            return;
        }
        else
        {
            toiletPrepTimer++;
        }

    }
    protected override void EndInteraction()
    {
        bathroom.dryBathroomBeingRemoved = false;
        bathroom.dryBathroomReady = false;
        onPlayerGoesAwayeEC.RaiseEvent(false);
        base.EndInteraction();

    }
}
