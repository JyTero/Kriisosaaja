using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeVegetableFromStorage : TakeFoodFromStorageBase, IFoodItemInteraction
{
  [SerializeField]

    private GameObject vegetablePrefab;
    [SerializeField]
    private List<GameObject> vegetableFoodGOs;

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

 

    protected override GameObject InstantiateConsumablePrefab(SurfaceSlot slot)
    {
        base.InstantiateConsumablePrefab(slot);
        interactionManager.PlayerResourceData.VegetableFoodAmount--;

        int r = Random.Range(0, 3);
        GameObject foodPrefab;
        if (r == 0)
            foodPrefab = vegetableFoodGOs[0];
        else if (r == 1)
            foodPrefab = vegetableFoodGOs[1];
        else if (r == 2)
            foodPrefab = vegetableFoodGOs[2];
        else
            foodPrefab = vegetableFoodGOs[3];

        return Instantiate(foodPrefab, slot.itemTransform.position, slot.itemTransform.rotation, slot.itemTransform);

    }

    //Used via IFoodItemInteraction interface to get food type
    public override GlobalValues.FoodItemType GetFoodItemType()
    {
        return FoodType;
    }
}

