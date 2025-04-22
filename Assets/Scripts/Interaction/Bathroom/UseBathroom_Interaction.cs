using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class UseBathroom_Interaction : BaseInteraction
{
    [SerializeField]
    private int bathroomIncreasePerTick;
    [SerializeField]
    private int unpoweredBathroomNeedCap = 0;
    [SerializeField]
    [Tooltip("How many need units unpowered toilet can be used before large penalty")]
    private int unpoweredBathroomCapasity = 0;
    [SerializeField]
    private string unpoweredBathroomFullNotification = "";
    [SerializeField]
    private string unwateredBathroomFullNotification = "";

    [SerializeField]
    private BoolEventChannelSO onPlayerGoesAwayeEC;
    [SerializeField]
    private BoolEventChannelSO onUnpoweredBathroomUse;

    private BathroomItem bathroom;

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
        bathroom.bathroomInUse = true;
    }

    public void ToiletTick()
    {
        //if has electrcity and water, normal increase
        if (!bathroom.NoPower && !bathroom.NoWater)
        {
            interactionManager.AdjustPlayerBathroom(bathroomIncreasePerTick);

            if (interactionManager.GetPlayerBathroom() >= 100)
            {
                EndInteraction();
                return;
            }
        }
        //else lower max need and add to counter which tells how much toilet can be used.
        //This value is penalty for using toilet during power outtage
        else
        {
            if (bathroom.usedUnpowerBathroomCapasity < unpoweredBathroomCapasity)
            {
                interactionManager.AdjustPlayerBathroom(bathroomIncreasePerTick);
                bathroom.usedUnpowerBathroomCapasity += bathroomIncreasePerTick;

                if (interactionManager.GetPlayerBathroom() >= unpoweredBathroomNeedCap)
                {
                    //For scoring, bool tells if unpowered capasity is used
                    onUnpoweredBathroomUse.RaiseEvent(false);
                    EndInteraction();
                }

            }
            //unpowered capasity used, toilet need power&water to function normally
            else
            {
                if (bathroom.NoPower)
                {
                    interactionManager.ShowNoticationText(unpoweredBathroomFullNotification, 0);
                }
                else if (bathroom.NoWater)
                {
                    interactionManager.ShowNoticationText(unpoweredBathroomFullNotification, 0);
                }
                //For scoring, bool tells if unpowered capasity is used
                onUnpoweredBathroomUse.RaiseEvent(true);
                EndInteraction();
            }

        }
    }

    protected override void EndInteraction()
    {
        bathroom.bathroomInUse = false;
        onPlayerGoesAwayeEC.RaiseEvent(false);
        base.EndInteraction();

    }

    public void ResetUnusedUnpoweredCapasity()
    {
        bathroom.usedUnpowerBathroomCapasity = -1;
    }

}
