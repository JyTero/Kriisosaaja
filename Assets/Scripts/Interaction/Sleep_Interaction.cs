using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep_Interaction : BaseInteraction
{
    [Tooltip("Increases sleep by this amount every tick and by /2 every other tick")]
    [SerializeField]
    private int sleepIncreasePerTick;
    [SerializeField]
    private int timeSpeedMultiplierDuringSleep;

    [SerializeField]
    private VoidEventChannelSO cancelLongInteractionEC;

    [SerializeField]
    private BoolEventChannelSO onPlayerGoesAwayeEC;

    //Used to vary energy gain during sleep
    private bool tickBool = true;

    private Bed bed;

    protected override void Start()
    {
        bed = thisItem as Bed;
    }
    public override void OnInteractionBegin()
    {
        base.OnInteractionBegin();
        // Route to interaction spot, if applicaple 
        playerAtDestinationEC.OnEventRaised += AtDestination;
        cancelLongInteractionEC.OnEventRaised += EndInteraction;
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
        interactionManager.TimeManager.SetTimeSpeed(timeSpeedMultiplierDuringSleep);


        interactionManager.TimeManager.PlayerAwayFastForward = true;
        bed.playerSleeps = true;
        interactionManager.LongInteractionBegin();

    }

    //Called from the item (bed) on timeBeat when bed.playerSleeps == true
    public void SleepTick()
    {
        if (tickBool)
        {
            interactionManager.AdjustPlayerEnergy(sleepIncreasePerTick);
        }
        else
        {
            interactionManager.AdjustPlayerEnergy(sleepIncreasePerTick/2);
        }

        if(interactionManager.GetPlayerEnergy() >= 90)
        { 
            EndInteraction();
        }
    }

    protected override void EndInteraction()
    {
        interactionManager.TimeManager.SetTimeSpeed(1);
        onPlayerGoesAwayeEC.RaiseEvent(false);

        bed.playerSleeps = false;
        interactionManager.TimeManager.PlayerAwayFastForward = false;
        cancelLongInteractionEC.OnEventRaised -= EndInteraction;

        base.EndInteraction();
        
    }
}
