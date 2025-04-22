using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeJuiceFromStorage : TakeFoodFromStorageBase, IFoodItemInteraction
{
    [SerializeField]
    private GameObject juicePrefab;

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
        if (interactionManager.PlayerResourceData.JuiceDrinkAmount <= 0)
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
        interactionManager.PlayerResourceData.JuiceDrinkAmount--;
        GameObject foodItem = Instantiate(juicePrefab, slot.itemTransform.position, slot.itemTransform.rotation, slot.itemTransform);
        foodItem.transform.Rotate(-90, 0, 0, Space.Self);
        return foodItem;
    }

    //Used via IFoodItemInteraction interface to get food type
    public override GlobalValues.FoodItemType GetFoodItemType()
    {
        return FoodType;
    }
}