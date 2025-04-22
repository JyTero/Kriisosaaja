using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GlobalValues;

[CreateAssetMenu(menuName = "ScenarioSO", fileName = "ScenarioSO")]
public class ScenarioSO : ScriptableObject
{
    public string scenarioName;
    [TextArea(5, 20)]
    public string scenarioDescription;

    public List<SituationSO> scenarioSituations = new();
    [Tooltip("Displayes the total lenght of all Situations currently within the Scenario. Update by modifying the scenario somehow. (Adjust ScenarioEndTime, for example)")]
    public int ScenarioSituationsTotalLenghtDEBUG = 0;


    [SerializeField]
    public List<ScenarioShopSellableData> ScenarioShopItems = new();

    public int ScenarioEndTime;

    private void OnValidate()
    {
        ScenarioSituationsTotalLenghtDEBUG = 0;
        foreach(SituationSO situation in scenarioSituations)
        {
            ScenarioSituationsTotalLenghtDEBUG += situation.SituationDuration;
        }
    }

}

[Serializable]
public class ScenarioShopSellableData
{
    public FoodItemType ScenarioShopSellableFood;
    public float ScenarioPriceMultiplier;
    public int ScenarioStock;
    [Tooltip("Whether to restock shelves to the amount here or use what ever amount player left to the shop on exit, " +
    "IF that is lower than stock given here. Higher amount gets always capped at amount here.")]
    public bool Restock;

    [Tooltip("Insert shop item this sellable data is about.")]
    public GameObject ScenarioShopSellableBoolItem;
}
