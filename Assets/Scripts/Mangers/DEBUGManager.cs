using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGManager : MonoBehaviour
{
    [SerializeField]
    private BoolEventChannelSO playerHasWaterAdjustEventChannel;
    GlobalValues.Quality[] allQuality;
    ScenarioSceneSaveManager saveScene;
    InteractionManager interactionManager;
    TimeManager timeManager;

    // Start is called before the first frame update
    void Start()
    {
        allQuality = (GlobalValues.Quality[])Enum.GetValues(typeof(GlobalValues.Quality));
         saveScene = FindObjectOfType<ScenarioSceneSaveManager>();
         interactionManager = FindObjectOfType<InteractionManager>();
         timeManager = FindObjectOfType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            FindObjectOfType<TakeVegetableFromStorage>().OnInteractionBegin();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            int time = interactionManager.GetManagerTime();
            Debug.Log("Manager Time: " + time);

        }

        //Adjust Time Speed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            timeManager.SetTimeSpeed(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            timeManager.SetTimeSpeed(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            timeManager.SetTimeSpeed(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            timeManager.SetTimeSpeed(10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            timeManager.SetTimeSpeed(20);
        }
        //Toggle electricity
        if (Input.GetKeyDown(KeyCode.E))
        {
            var hasPower = interactionManager.GetPlayerHasElectricity();
            if (hasPower)
            {
                interactionManager.AdjustPlayerHasElectricity(false);
            }
            else
                interactionManager.AdjustPlayerHasElectricity(true);

        }
        //Toggle Weather
        if (Input.GetKeyDown(KeyCode.W))
        {
            GlobalValues.Weather[] allWeather = (GlobalValues.Weather[])Enum.GetValues(typeof(GlobalValues.Weather));
            GlobalValues.Weather currentWeather = interactionManager.GetWorldWeather();

            int currentIndex = Array.IndexOf(allWeather, currentWeather);
            int nextIndex = (currentIndex + 1) % allWeather.Length;

            currentWeather = allWeather[nextIndex];
            interactionManager.AdjustWorldWeather(currentWeather);
        }
        //Toggle water
        if (Input.GetKeyDown(KeyCode.V))
        {
            var hasWater = interactionManager.GetPlayerHasWater();
            if (hasWater)
            {
                interactionManager.AdjustPlayerHasWater(false);
            }
            else
            {
                interactionManager.AdjustPlayerHasWater(true);
            }
        }
        //Toggele Air Quality
        if (Input.GetKeyDown(KeyCode.A))
        {
            GlobalValues.Quality currentAirQ = interactionManager.GetWorldAirQ();

            int currentIndex = Array.IndexOf(allQuality, currentAirQ);
            int nextIndex = (currentIndex + 1) % allQuality.Length;

            currentAirQ = allQuality[nextIndex];
            interactionManager.AdjustWorldAirQ(currentAirQ);
        }
        //Toggle Time of Day
        if (Input.GetKeyDown(KeyCode.T))
        {
            timeManager.IsNight = !timeManager.IsNight;
        }
        //toggle Weather
        if (Input.GetKeyDown(KeyCode.E))
        {
            var curWeather = interactionManager.GetWorldWeather();
            interactionManager.AdjustWorldWeather((GlobalValues.Weather)curWeather + 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            saveScene.SaveScenarioSceneSurfaceSlotItems();
            Debug.Log("Scenario Saved");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            saveScene.LoadScenarioSceneSurfaceSlotItems();
            Debug.Log("Scenario Loaded");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ItemManager itemManager = FindObjectOfType<ItemManager>();
            //foreach (SurfaceSlot slot in itemManager.AllSlots)
            //{
            //    if (slot.itemInSlot != null)
            //    {
            //        Debug.Log("FOund item! " + slot.itemInSlot.GetComponent
            //            <ConsumableBase>().ConsumableName);
            //    }
            //}
            foreach (SurfaceSlot slot in FindObjectOfType<Fridge>().SurfaceSlots)
            {
                Debug.Log("SlotGO " + ": " + slot.gameObject.name);
            }
            string s = "Saved Items: ";
            Debug.Log("List count: " + GlobalValues.savedSlots.Count);
            foreach (SavedSlot savedSlot in GlobalValues.savedSlots)
            {

                Debug.Log("TRANSFORM: " + savedSlot.slot.itemTransform);
                s += savedSlot.savedConsumableScript.ConsumableName + ", ";
            }
            Debug.Log(s);
        }
        //Get Bathroom Interactions
        if (Input.GetKeyDown(KeyCode.B))
        {
            var bathroom = FindObjectOfType<BathroomItem>();
            string s = "";
            foreach (BaseInteraction interaction in bathroom.Interactions)
            {
                s += interaction.InteractionName + "\n";
            }
            Debug.Log(s);
        }
    }
}
