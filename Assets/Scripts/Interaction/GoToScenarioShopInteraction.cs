using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScenarioShopInteraction : BaseInteraction
{

    //Upon Clicking the interaction from object
    public override void OnInteractionBegin()
    {
        base.OnInteractionBegin();
        // Route to interaction spot, if applicaple 
        playerAtDestinationEC.OnEventRaised += AtDestination;
        interactionManager.SetDestination(thisItem.InteractionSpotTransform.position);
        // Once routed, begin interaction animations by getting alert from Player via InteractionManager
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
        SaveScenario();
        Debug.Log("GOING TO SCENARIO SHOP");
        SceneManager.LoadSceneAsync("JyriShop_UI");
    }

    public void SaveScenario()
    {
        ScenarioSceneSaveManager scenarioSaveManager = FindObjectOfType<ScenarioSceneSaveManager>();
        scenarioSaveManager.SaveScenarioSceneSurfaceSlotItems();
        scenarioSaveManager.SavePlayerValues();
        scenarioSaveManager.SaveWorldValues();
        scenarioSaveManager.SaveScenarioData();
        scenarioSaveManager.SaveScenarioObjectsState();
        scenarioSaveManager.SaveScoringLists();
    }
}
