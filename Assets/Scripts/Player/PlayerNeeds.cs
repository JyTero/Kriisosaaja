using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerNeeds : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannelSO timeBeat;
    [SerializeField]
    private StringEventChannelSO scenarioLostStringEC;
    [SerializeField]
    private QualityEventChannelSO onAirQualityChange;
    [SerializeField]
    private bool DEBUGFreezeHunger = false;
    [SerializeField]
    private bool DEBUGFreezeHydration = false;
    [SerializeField]
    private string badAirAlertText = "";

    private Player player;
    private ItemManager itemManager;
    private InteractionManager interactionManager;
    private bool everyOther = false;
    private bool badAir = false;

    private int curMentalWellbeing, curHunger, curHydration, curBathroom, curHealth, curEnergy;

    private int ticker = 0;
    private void OnEnable()
    {
        timeBeat.OnEventRaised += TimeBeat;
        onAirQualityChange.OnEventRaised += OnAirQChange;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        itemManager = FindObjectOfType<ItemManager>();
        interactionManager = FindObjectOfType<InteractionManager>();
    }
    private void TimeBeat()
    {
        //Get all needs
        if (interactionManager != null)
        {
            curMentalWellbeing = interactionManager.GetPlayerMentalWellbeing();
            curHunger = interactionManager.GetPlayerHunger();
            curHydration = interactionManager.GetPlayerHydration();
            curBathroom = interactionManager.GetPlayerBathroom();
            curHealth = interactionManager.GetPlayerHealth();
            curEnergy = interactionManager.GetPlayerEnergy();
        }

        //Needs Decline
        //Hunger
        HungerNeed();

        //Hydration
        HydrationNeed();


        //Bathroom 
        if (everyOther)
        {
            interactionManager.AdjustPlayerBathroom(-1);
            //DEBUG
            //interactionManager.AdjustPlayerBathroom(-5);

            if (!player.IsSleeping)
            {
                if (curBathroom <= 20)
                {
                    if (everyOther)
                        interactionManager.AdjustPlayerMentalWellbeing(-1);
                }
            }
        }
        else
        {
        }

        //Energy
        if (ticker % 4 == 0)
        {
            interactionManager.AdjustPlayerEnergy(-1);
        }


        //high hunger
        if (curHunger > 70)
        {
            if (curMentalWellbeing <= 70)
                interactionManager.AdjustPlayerMentalWellbeing(1);
            if (curHealth <= 70)
                interactionManager.AdjustPlayerHealth(1);
        }
        //Low Hunger
        else if (curHunger <= 20)
        {
            if (everyOther)
            {
                interactionManager.AdjustPlayerMentalWellbeing(-1);
                interactionManager.AdjustPlayerHealth(-1);
            }
        }

        //High Hydration
        if (curHydration >= 70)
        {
            if (curMentalWellbeing <= 70)
                interactionManager.AdjustPlayerMentalWellbeing(1);
            if (curHealth <= 70)
                interactionManager.AdjustPlayerHealth(1);
        }
        //Low Hydration
        else if (curHydration <= 20)
        {
            if (!everyOther)
            {
                interactionManager.AdjustPlayerMentalWellbeing(-1);
                interactionManager.AdjustPlayerHealth(-1);
            }
        }

        //Low Energy
        if (curEnergy <= 20)
        {
            if (ticker % 4 != 0)
                interactionManager.AdjustPlayerMentalWellbeing(-1);

        }

        //Bad air
        if (badAir)
        {
            if (everyOther)
                interactionManager.AdjustPlayerHealth(-1);
        }

        //Health depletion end
        if (curHealth <= 0)
        {
            //Cause: Override  (toxic air, eating spoiled food)
            //if(conditionHere)
            //Cause: Low Hydration
            if (curHydration <= 0)
            {
                //Show end screen with info on how and why the game ended
                scenarioLostStringEC.RaiseEvent("dehydration!");
            }
            //Cause: Low Hunger
            if (curHunger <= 0)
            {
                scenarioLostStringEC.RaiseEvent("malnourishment!");
            }
        }
        everyOther = !everyOther;
        ticker++;
        if (ticker >= 24)
            ticker = 1;
    }

    private void HungerNeed()
    {
        if (DEBUGFreezeHunger)
            return;
        if (player.IsSleeping)
        {
            if (ticker % 20 == 0)
                interactionManager.AdjustPlayerHunger(-1);
        }
        else
        {
            if (everyOther)
                interactionManager.AdjustPlayerHunger(-1);
        }
    }

    private void HydrationNeed()
    {
        if (DEBUGFreezeHydration)
            return;
        if (player.IsSleeping)
        {
            if (ticker % 20 == 0)
                interactionManager.AdjustPlayerHydration(-1);
        }
        else
        {
            interactionManager.AdjustPlayerHydration(-1);
        }
    }

    private void OnAirQChange(GlobalValues.Quality newAirQ)
    {
        if (newAirQ == GlobalValues.Quality.Dangerous)
        {
            if (!itemManager.WindowIsSealed)
            {
                badAir = true;
                itemManager.ShowNoticationText(badAirAlertText, 0);
            }
            else
            {
                badAir = false;
            }
        }
        else
        {
            badAir = false;
        }
    }

    private void OnDisable()
    {
        timeBeat.OnEventRaised -= TimeBeat;
        onAirQualityChange.OnEventRaised -= OnAirQChange;
    }
}
