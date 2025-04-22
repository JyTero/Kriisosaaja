using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.LowLevel;

public class ScenarioSceneSaveManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> consumablePrefabs;
    private List<IFoodStorage> foodStorageItems = new();

    private InteractionManager interactionManager;
    private ScenarioManager scenarioManager;
    private ScenarioScoring scenarioScoring;

    // Start is called before the first frame update
    void Awake()
    {
        //if the list count is 0, this is the fist time this scene is loaded for this scenario
        if (foodStorageItems.Count == 0)
        {
            var tmpStorage = FindObjectsOfType<MonoBehaviour>().OfType<IFoodStorage>();
            foreach (IFoodStorage foodStorage in tmpStorage)
            {
                foodStorageItems.Add(foodStorage);
                Debug.Log("FoodStorageItem");
            }
            interactionManager = FindObjectOfType<InteractionManager>();
            scenarioManager = FindObjectOfType<ScenarioManager>();
        }
    }

    public void SaveScenarioData()
    {
        GlobalValues.ScenarioSituationIndex = scenarioManager.scenarioSituationIndex;
        GlobalValues.ScenarioSituationTimer = scenarioManager.SituationTimer;
    }

    public void LoadScenarioData()
    {
        scenarioManager = FindObjectOfType<ScenarioManager>();
        scenarioManager.scenarioSituationIndex = GlobalValues.ScenarioSituationIndex;
        scenarioManager.SituationTimer = GlobalValues.ScenarioSituationTimer;
    }

    public void SaveWorldValues()
    {
        //Save all values as ints to a list
        GlobalValues.WorldValues = new()
        {
            interactionManager.GetWorldTemperature(),
            Convert.ToInt32(interactionManager.GetWorldWeather()),
            Convert.ToInt32(interactionManager.GetWorldHealthQ()),
            Convert.ToInt32(interactionManager.GetWorldAirQ()),
            Convert.ToInt32(interactionManager.GetWorldInfoQ()),
            Convert.ToInt32(interactionManager.GetWorldIsAllowedOut()),
        };
    }

    public void LoadWorldValues()
    {
        //If list is empty, there is no save data to load
        if (GlobalValues.PlayerValues.Count == 0)
            return;
        //Apply values form list to the scene, convert to proper types as needed
        interactionManager = FindObjectOfType<InteractionManager>();
        interactionManager.AdjustWorldTemperature(GlobalValues.WorldValues[0]);
        interactionManager.AdjustWorldWeather((GlobalValues.Weather)GlobalValues.WorldValues[1]);
        interactionManager.AdjustWorldAirQ((GlobalValues.Quality)GlobalValues.WorldValues[2]);
        interactionManager.AdjustWorldHealthQ((GlobalValues.Quality)GlobalValues.WorldValues[3]);
        interactionManager.AdjustWorldInfoQ((GlobalValues.Quality)GlobalValues.WorldValues[4]);
        interactionManager.AdjustWorldIsAllowedOut(Convert.ToBoolean(GlobalValues.WorldValues[5]));
    }

    
    public void SaveScoringLists()
    {
        int i = 0;
        scenarioScoring = FindObjectOfType<ScenarioScoring>();
        //Iterate through one list, add items on both scoring and string lists to appropriate savingLists.
        foreach (string s in scenarioScoring.scoringStrings)
        {
            GlobalValues.scoringStrings.Add(s);
            GlobalValues.scoringScores.Add(scenarioScoring.scoringScores[i]);
            i++;
        }
    }

    public void LoadScoringLists()
    {
        int i = 0;
        scenarioScoring = FindObjectOfType<ScenarioScoring>();
        scenarioScoring.scoringStrings = new();
        scenarioScoring.scoringScores  = new();
        //Iterate through one list, add items on both scoring and string lists to appropriate savingLists.
        foreach (string s in GlobalValues.scoringStrings)
        {
            scenarioScoring.scoringStrings.Add(s);
            scenarioScoring.scoringScores.Add(GlobalValues.scoringScores[i]);
            i++;
        }
    }

    public void SavePlayerValues()
    {
        GlobalValues.PlayerValues = new()
        {
            //(int)interactionManager.GetPlayerMoney(),
            interactionManager.GetPlayerMentalWellbeing(),
            interactionManager.GetPlayerHunger(),
            interactionManager.GetPlayerHydration(),
            interactionManager.GetPlayerHealth(),
            interactionManager.GetPlayerEnergy(),
            Convert.ToInt32(interactionManager.GetPlayerHasElectricity()),
            Convert.ToInt32(interactionManager.GetPlayerHasWater()),

        };
    }

    public void LoadPlayerValues()
    {
        //If list is empty, there is no save data to load
        if (GlobalValues.PlayerValues.Count == 0)
            return;
        interactionManager = FindObjectOfType<InteractionManager>();
       // interactionManager.AdjustPlayerMoney(GlobalValues.PlayerValues[0]);
        interactionManager.AdjustPlayerMentalWellbeing(GlobalValues.PlayerValues[1]);
        interactionManager.AdjustPlayerHunger(GlobalValues.PlayerValues[2]);
        interactionManager.AdjustPlayerHydration(GlobalValues.PlayerValues[3]);
        interactionManager.AdjustPlayerHealth(GlobalValues.PlayerValues[4]);
        interactionManager.AdjustPlayerEnergy(GlobalValues.PlayerValues[5]);
        interactionManager.AdjustPlayerHasElectricity(Convert.ToBoolean(GlobalValues.PlayerValues[5]));
        interactionManager.AdjustPlayerHasWater(Convert.ToBoolean(GlobalValues.PlayerValues[6]));
    }

    //Save state of scenario objects that have something to save. Each item needs its own implementation
    public void SaveScenarioObjectsState()
    {
        var bathroom = FindObjectOfType<BathroomItem>();
        //Dry toilet
        if(bathroom.dryBathroomReady)
            GlobalValues.DryToiletPrepared= true;
        else
            GlobalValues.DryToiletPrepared= false;
        //Unpowered bathroom capasity
        if(bathroom.usedUnpowerBathroomCapasity != -1)
        {
            GlobalValues.UsedUnpoweredToiletUsedCapasity = bathroom.usedUnpowerBathroomCapasity;
        } 
        //WindowState
        if (FindObjectOfType<ItemManager>().WindowIsSealed)
            GlobalValues.WinbowSealed = true;
        else
            GlobalValues.WinbowSealed= false;

    }
    public void LoadScenarioObjectsState()
    {
        var bathroom = FindObjectOfType<BathroomItem>();
        if (GlobalValues.DryToiletPrepared)
            bathroom.PrepareDryToiletOnLoad();
        //Unpowered bathroom
        bathroom.usedUnpowerBathroomCapasity = GlobalValues.UsedUnpoweredToiletUsedCapasity;
        if (GlobalValues.WinbowSealed)
            FindObjectOfType<WindowItem>().SealWindowOnLoad();
    }

    public void SaveScenarioSceneSurfaceSlotItems()
    {
        foreach (IFoodStorage foodStorage in foodStorageItems)
        {
            foreach (SurfaceSlot slot in foodStorage.SurfaceSlots)
            {
                if (slot.itemInSlot == null)
                {
                    continue;
                }
                else
                {
                    var consumable = slot.itemInSlot.GetComponent<ConsumableBase>();
                    SavedSlot newSave = new(slot.gameObject, foodStorageItems.IndexOf(foodStorage), foodStorage.SurfaceSlots.IndexOf(slot), consumable.HasSpoiled, consumable.WillSpoilTime);
                    Debug.Log("Saved! " + newSave.savedConsumableScript.ConsumableName);
                    GlobalValues.savedSlots.Add(newSave);
                }
            }
        }
    }

    //Go through each saved slot, find a matching consumable prefab and load that item to the slot
    public void LoadScenarioSceneSurfaceSlotItems()
    {
        foreach (SavedSlot savedSlot in GlobalValues.savedSlots)
        {
            foreach (GameObject consumablePrefabGo in consumablePrefabs)
            {
                if (consumablePrefabGo.GetComponent<ConsumableBase>().ConsumableName == savedSlot.savedConsumableScript.ConsumableName)
                {
                    //GameObject newFood = Instantiate(consumablePrefabGo, savedSlot.slot.itemPosition, savedSlot.slot.itemRotation, foodStorageItems[savedSlot.SourceItemIndex].SurfaceSlots[savedSlot.sourceSlotListIndex].itemTransform);
                    GameObject newFood = Instantiate(consumablePrefabGo); //, foodStorageItems[savedSlot.SourceItemIndex].SurfaceSlots[savedSlot.sourceSlotListIndex].itemTransform
                    newFood.transform.position = foodStorageItems[savedSlot.SourceItemIndex].SurfaceSlots[savedSlot.sourceSlotListIndex].transform.position;
                    newFood.transform.rotation= foodStorageItems[savedSlot.SourceItemIndex].SurfaceSlots[savedSlot.sourceSlotListIndex].transform.rotation;

                    if (foodStorageItems[savedSlot.SourceItemIndex] is Fridge)  
                        FindObjectOfType<Fridge>().LoadItemToSlot(newFood, savedSlot.sourceSlotListIndex, savedSlot.spoiled, savedSlot.spoilTime);
                    else if (foodStorageItems[savedSlot.SourceItemIndex] is KitchenCounter)
                        FindObjectOfType<KitchenCounter>().LoadItemToSlot(newFood, savedSlot.sourceSlotListIndex, savedSlot.spoiled, savedSlot.spoilTime);
                    else
                        Debug.LogError("Unknown FoodStorage item on Load!");
                    break;
                }
                else
                {
                    // Debug.Log("Nay");
                    continue;
                }
            }
        }
    }
}

[System.Serializable]
public class SavedSlot
{
    public ConsumableBase savedConsumableScript;
    public int SourceItemIndex;
    public int sourceSlotListIndex;
    public SurfaceSlot slot;
    public bool spoiled;
    public int spoilTime;

    public SavedSlot(GameObject surfaceSlotGo, int itemIndex, int slotIndex, bool isSpoiled, int spoilTime)
    {
        this.slot = surfaceSlotGo.GetComponent<SurfaceSlot>();

        this.savedConsumableScript = this.slot.itemInSlot.GetComponent<ConsumableBase>();
        this.SourceItemIndex = itemIndex;
        this.sourceSlotListIndex = slotIndex;
        this.spoiled = isSpoiled;
        this.spoilTime = spoilTime;
    }
}
