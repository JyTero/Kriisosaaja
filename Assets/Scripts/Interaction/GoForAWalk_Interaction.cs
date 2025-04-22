using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static GlobalValues;

//The first made interaction that was never updated to use TimeBeat. As such, the timetracking within 
//is done with actual time tracking. Do not use as example when creating new interactions. See cooking,
public class GoForAWalk_Interaction : BaseInteraction
{
    //Interaction spot, the place to which player routes.
    //Will usually be taken from the object script of the interaction owner


    [SerializeField]
    private Transform outOfSightTransform;
    [SerializeField]
    [Tooltip("Lenght of interaction, in-game time units")]
    private int interactionLength = 5;

    [SerializeField]
    private int goodWalkMinTemperature = 0;
    [SerializeField]
    private int goodWalkMaxTemperature = 24;

    [SerializeField]
    private int curfewOnMentalWellbeingOffset = -20;

    [SerializeField]
    private int goodWeatherMentalWellbeingOffset = 10;
    [SerializeField]
    private int badWeatherMentalWellbeingOffset = -15;
    
    [SerializeField]
    private int hotMentallWellbeingOffset = -5;
    [SerializeField]
    private int coldMentallWellbeingOffset = -5;
    [SerializeField]
    private int goodTemperatureMentalWellbeingOffset = 10;

    [SerializeField]
    private int goodAirQMentalWellbeingOffset = 5;
    [SerializeField]
    private int badAirQMentalWellbeingOffset = -15;

    [SerializeField]
    private BoolEventChannelSO onPlayerGoesAwayeEC;
    private int totalWellbeingOffset;


    //Upon Clicking the interaction from object
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

        //raise event on player, teleports them out of sight
        onPlayerGoesAwayeEC.RaiseEvent(true);

        DuringInteraction();

    }


    //The "Main" of the Interaction
    private void DuringInteraction()
    {
        interactionOutcome = "";
        StartCoroutine(OnAWalk());
    }

    IEnumerator OnAWalk()
    {
        //To track the interaction lenght
        float startTime = Time.time;
        //Debug.Log("StarTime");
        totalWellbeingOffset = 0;
        Debug.Log("Player goes for a walk");

        if (interactionManager.GetWorldIsAllowedOut())
        {

        }

        else
        {
            //Curfew is on 
            interactionOutcome += "There are a lot of police around... and there are posters telling people to stay indoors.\n";
            totalWellbeingOffset += curfewOnMentalWellbeingOffset;
            //hadGoodWalk = false;

        }

        switch (interactionManager.GetWorldWeather())
        {
            case Weather w when w == Weather.Clear || w == Weather.Snow || w == Weather.Cloudy:
                interactionOutcome += "Such a nice weather today\n";
                totalWellbeingOffset += goodWeatherMentalWellbeingOffset;
                break;
            //TODO: Seperate to different cases
            case Weather w when w == Weather.Rain || w == Weather.Thunder || w == Weather.Blizzard:
                interactionOutcome += "Bad weather really dampens the mood\n";
                totalWellbeingOffset += badWeatherMentalWellbeingOffset;
                break;

            default:
                Debug.LogWarning($"Default Weather in {InteractionName} interaction");
                break;
        }

        switch (interactionManager.GetWorldTemperature())
        {
            //Too cold  
            case int i when i < goodWalkMinTemperature:
                interactionOutcome += "It's quite cold. Brrr...\n";
                totalWellbeingOffset += coldMentallWellbeingOffset;
                break;
            //Too hot  
            case int i when i > goodWalkMaxTemperature:
                interactionOutcome += "It's hot in here. Sweaty...\n";
                totalWellbeingOffset += hotMentallWellbeingOffset;
                break;
            //Good Temp  
            case int i when i <= goodWalkMaxTemperature && i >= goodWalkMinTemperature:
                interactionOutcome += "Not freezing nor sweating, I' lovin' this\n";
                totalWellbeingOffset += goodTemperatureMentalWellbeingOffset;
                break;
            default:
                Debug.LogWarning($"Default Temp in {InteractionName} interaction");
                break;
        }

        //AirQuality  
        switch (interactionManager.GetWorldAirQ())
        {
            case Quality q when (int)q <= 2:
                interactionOutcome += "The air smells bad...\n";
                totalWellbeingOffset += badAirQMentalWellbeingOffset;
                break;
            case Quality q when (int)q > 2:
                interactionOutcome += "Fresh air really does wonders\n";
                totalWellbeingOffset += goodAirQMentalWellbeingOffset;
                break;
            default:
                Debug.LogWarning($"Default AirQuality in {InteractionName} interaction");
                break;
        }

        while (Time.time - startTime <= interactionLength)
        {
            yield return new WaitForSeconds(2);

        }

        interactionManager.ShowNoticationText(interactionOutcome, 0);
        interactionManager.AdjustPlayerMentalWellbeing(totalWellbeingOffset);

        EndInteraction();
    }

    protected override void EndInteraction()
    {
        onPlayerGoesAwayeEC.RaiseEvent(false);

        base.EndInteraction();
    }
}
