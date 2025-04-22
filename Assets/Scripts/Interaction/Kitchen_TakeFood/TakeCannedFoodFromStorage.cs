using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.ResourceManagement.ResourceProviders.AssetBundleResource;

public class TakeCannedFoodFromStorage : TakeFoodFromStorageBase, IFoodItemInteraction
{
    [SerializeField]
    private GameObject cannedFoodPrefab;

    [SerializeField]
    private Material canMat01;
    [SerializeField]
    private Material canMat02;



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
        if (interactionManager.PlayerResourceData.CannedFoodAmount <= 0)
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
        interactionManager.PlayerResourceData.CannedFoodAmount--;
        //Instantiate new foodprefab, psuedo randomize material by gametime;
        GameObject foodItem = Instantiate(cannedFoodPrefab, slot.itemTransform.position, slot.itemTransform.rotation, slot.itemTransform);
        MeshRenderer foodMesh = foodItem.GetComponent<MeshRenderer>();

        if (interactionManager.GetManagerTime() % 2 == 0)
            foodMesh.material = canMat01;
        else
            foodMesh.material = canMat02;
        foodItem.transform.Rotate(-90, 0, 0, Space.Self);
        return foodItem;

    }
    //Used via IFoodItemInteraction interface to get food type
    public override GlobalValues.FoodItemType GetFoodItemType()
    {
        return FoodType;

    }
}
