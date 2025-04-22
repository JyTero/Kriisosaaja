using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareDryToilet_Interaction : BaseInteraction
{
    [HideInInspector]
    public bool dryBathroomPrep = false;
    [SerializeField]
    private int dryToiletPrepTime = 0;
    [SerializeField]
    private BoolEventChannelSO onDrytoiletPrep;

    [SerializeField]
    private BoolEventChannelSO onPlayerGoesAwayeEC;

    private BathroomItem bathroom;
    private int toiletPrepTimer = 0;

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
        bathroom.dryBathroomUnderPrep = true;
        //disable this interaction
        this.enabled = false;



    }

    public void PrepareDryToiletTick()
    {
        if (toiletPrepTimer >= dryToiletPrepTime)
        {
            PrepareDryToilet();
            onDrytoiletPrep.RaiseEvent(true);
            EndInteraction();
            return;
        }
        else
        {
            toiletPrepTimer++;
        }

    }
    private void PrepareDryToilet()
    {
        //Remove prep dry toilet from item interactions
        this.enabled = false;
        bathroom.Interactions.Remove(this);
        //Add using dry toilet to item interactions
        bathroom.useDryBathroomInteraction.enabled = true;
        thisItem.Interactions.Add(bathroom.useDryBathroomInteraction);
        //Remove using normal toilet from interactions
        bathroom.useBathroomInteraction.enabled = false;
        thisItem.Interactions.Remove(bathroom.useBathroomInteraction);
        //Add Remove dry toilet to interactions
        bathroom.removeDryToiletInteraction.enabled = true;
        thisItem.Interactions.Add(bathroom.removeDryToiletInteraction);

    }
    protected override void EndInteraction()
    {
        toiletPrepTimer = 0;
        bathroom.dryBathroomUnderPrep = false;
        bathroom.dryBathroomReady = true;

        onPlayerGoesAwayeEC.RaiseEvent(false);
        base.EndInteraction();

    }

    public void PrepareDryToiletOnLoad()
    {
        PrepareDryToilet();


        bathroom.dryBathroomUnderPrep = false;
        bathroom.dryBathroomReady = true;
    }
}
