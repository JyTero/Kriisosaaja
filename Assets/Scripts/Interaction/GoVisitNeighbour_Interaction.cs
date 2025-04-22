using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static GlobalValues;

//This interaction is based on GoForAWalk_Interaction which itself is outdated in the way it handles interaction timing.
//If looking for inspiration on how to do interactions properly, see CookFoodStove interaction.
public class GoVisitNeighbour_Interaction : BaseInteraction
{
    [SerializeField]
    [Tooltip("Lenght of interaction, in-game time units")]
    private int interactionLength = 5;

    [SerializeField]
    private int hadProperHUmanInteractionWellbeingOffset = 20;
    [SerializeField]
    private int youReassuredEachotherWellbeingOffset = 10;
    [SerializeField]
    private int youHadSomeSnacks = 1;
    [SerializeField]
    private int youHadADrink = 1;
    [SerializeField]
    private int youBothJustWorriedEachotherWellbeingOffset = -10;

    [SerializeField]
    private int curfewOnMentalWellbeingOffset = -20;

    [SerializeField]
    private BoolEventChannelSO onPlayerGoesAwayeEC;

    private int totalWellbeingOffset;

    private bool hasVisitedToday;
    private int ADaysWait = 288;

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
        //In this case it could include changing to outdoor clothing, opening door etc.
        onPlayerGoesAwayeEC.RaiseEvent(true);
        DuringInteraction();

    }

    //The "Main" of the Interaction
    private void DuringInteraction()
    {
        interactionOutcome = "";
        if (hasVisitedToday == false)
        {
            StartCoroutine(Visiting());
        }
        else
        {
            interactionOutcome += "You visited them earlier today, you should give them some time to themselves.";
        }
        
    }

    IEnumerator Visiting()
    {
        //To track the interaction lenght
        float startTime = Time.time;
        //Debug.Log("StarTime");
        totalWellbeingOffset = 0;
        Debug.Log("Player goes for to visit their neighbour");

        if (interactionManager.GetWorldIsAllowedOut())
        {
            interactionOutcome += "You had some proper face to face interaction.\n";
            totalWellbeingOffset += hadProperHUmanInteractionWellbeingOffset;
        }

        else
        {
            //Curfew is on 
            interactionOutcome += "There are a lot of police around... and there are posters telling people to stay indoors.\n";
            totalWellbeingOffset += curfewOnMentalWellbeingOffset;
            

        }

        float temp = UnityEngine.Random.Range(1, 2);
        if(temp == 1)
        {
            interactionOutcome += "Your neighbour served some snacks.\n";
            interactionManager.AdjustPlayerHunger(youHadSomeSnacks);
        }
        else if(temp == 2)
        {
            interactionOutcome += "Your neighbour served you a drink.\n";
            interactionManager.AdjustPlayerHydration(youHadADrink);
        }

        float temp2 = UnityEngine.Random.Range(1, 3);
        if (temp2 == 1)
        {
            interactionOutcome += "You both ended up worrying eachother.\n";
            totalWellbeingOffset += youBothJustWorriedEachotherWellbeingOffset;
        }
        else if (temp2 == 2 || temp2 == 3)
        {
            interactionOutcome += "You reasured eachother, taking a load off your shoulders.\n";
            totalWellbeingOffset += youReassuredEachotherWellbeingOffset;
        }

        while (Time.time - startTime <= interactionLength)
        {
            yield return new WaitForSeconds(1);

        }
        
        Debug.Log(interactionOutcome + "\nWellbeing adjustment: " + totalWellbeingOffset);
        interactionManager.ShowNoticationText(interactionOutcome, 0);
        interactionManager.AdjustPlayerMentalWellbeing(totalWellbeingOffset);

        hasVisitedToday = true;
        StartCoroutine(CanVisitAgainClock());
        EndInteraction();
    }

    protected override void EndInteraction()
    {
        onPlayerGoesAwayeEC.RaiseEvent(false);
        base.EndInteraction();
    }

    IEnumerator CanVisitAgainClock()
    {
        while (ADaysWait > 0)
        {
            yield return new WaitForSeconds(1);
            ADaysWait--;
            //Debug.Log("TimeRemaining: " + ADaysWait);
        }

        interactionOutcome += "You feel like you could visit your neighbour again.";
        hasVisitedToday = false;
        ADaysWait = 288;
    }

        //DEBUG
        private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log("Begin interaction neighbour");
            OnInteractionBegin();
        }
    }
}
