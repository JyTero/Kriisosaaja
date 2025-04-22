using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOT IN USE, HAS NOT BEEN TESTED
public class GoEatFastFood : BaseInteraction
{

    [SerializeField]
    private Transform interactionSpotTransform;

    [SerializeField]
    [Tooltip("Lenght of interaction, in-game time units")]
    private int interactionLength = 5;


    //Upon Clicking the interaction from object
    public override void OnInteractionBegin()
    {
        base.OnInteractionBegin();
        // Route to interaction spot, if applicaple 
        playerAtDestinationEC.OnEventRaised += AtDestination;
        interactionManager.SetDestination(interactionSpotTransform.position);
    }

    //Raised when player arrives to their current destination
    protected override void AtDestination()
    {
        base.AtDestination();
        //Stuff player does when arriving to the interaction spot
        //In this case it could include changing to outdoor clothing, opening door etc.

        DuringInteraction();

    }


    //The "Main" of the Interaction
    private void DuringInteraction()
    {
        interactionOutcome = "";
        StartCoroutine(EatFastFood());
    }

    IEnumerator EatFastFood()
    {
        //To track the interaction lenght
        float startTime = Time.time;
        //Debug.Log("StarTime");


        while (Time.time - startTime <= interactionLength)
        {
            yield return new WaitForSeconds(2);
        }

        Debug.Log("Had Lovely Fast Food (Not really)");

        yield return null;
    }
}
