using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatVegetableFromStorage : EatFoodFromStorageBase, IFoodItemInteraction
{
  [SerializeField]
    private GameObject vegetablePrefab;
    

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        StartCoroutine(StartCoro());

    }
    IEnumerator StartCoro()
    {
        yield return new WaitUntil(() => interactionManager.ScenarioInitialised == true);
        if (interactionManager.PlayerResourceData.VegetableFoodAmount <= 0)
            this.enabled = false;

        InteractionInitialised = true;
        yield return null;
    }

    public override void OnInteractionBegin()
    {
        base.OnInteractionBegin();
    }

    //Raised when player arrives to their current destination
    protected override void AtDestination()
    {
        base.AtDestination();

    }

    //The "Main" of the Interaction
    protected override void DuringInteraction()
    {
        base.DuringInteraction();
    }

    protected override ConsumableBase GetConsumableScript()
    {
        base.GetConsumableScript();
        interactionManager.PlayerResourceData.VegetableFoodAmount--;
        return vegetablePrefab.GetComponent<ConsumableBase>();

    }

    //Used via IFoodItemInteraction interface to get food type
    public override GlobalValues.FoodItemType GetFoodItemType()
    {
        return FoodType;
    }
}
