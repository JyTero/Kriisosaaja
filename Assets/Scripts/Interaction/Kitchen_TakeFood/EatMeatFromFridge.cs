using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatMeatFromFridge : EatFoodFromStorageBase, IFoodItemInteraction
{
    [SerializeField]
    private GameObject meatPrefab;
   
    [SerializeField]
    private VoidEventChannelSO onDangerousFoodEatenEC;

    [SerializeField]
    private GameObject FridgeDoorParent;
    [SerializeField]
    private int doorOpenTargetRotation = 0;
    [SerializeField]
    private float doorOpenTime = 0;
    private float rotateTimer = 0;


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
        if (interactionManager.PlayerResourceData.MeatFoodAmount <= 0)
            this.enabled = false;

        InteractionInitialised= true;
        yield return null;
    }

    public override void OnInteractionBegin()
    {
        
        base.OnInteractionBegin();
    }
    //Raised when player arrives to their current destination
    protected override void AtDestination()
    {
        StartCoroutine(OpenDoorCoro());
        base.AtDestination();

    }


    //The "Main" of the Interaction
    protected override void DuringInteraction()
    {
        base.DuringInteraction();
    }

    private IEnumerator OpenDoorCoro()
    {
        rotateTimer = 0;

        //Open Door
        float anglePerFrame = doorOpenTargetRotation / doorOpenTime;
        while (rotateTimer <= doorOpenTime)
        {
            float angleThisFrame = anglePerFrame * Time.deltaTime;
            FridgeDoorParent.transform.GetChild(2).Rotate( 0, 0, angleThisFrame, Space.Self);
            rotateTimer += Time.deltaTime;
            yield return null;
        }

        //Wait
        yield return new WaitForSecondsRealtime(0.2f);
        rotateTimer = 0;

        //Close door
        anglePerFrame = -doorOpenTargetRotation / doorOpenTime;
        while (rotateTimer <= doorOpenTime)
        {
            float angleThisFrame = anglePerFrame * Time.deltaTime;
            FridgeDoorParent.transform.GetChild(2).Rotate( 0, 0, angleThisFrame, Space.Self);
            rotateTimer += Time.deltaTime;
            yield return null;
        }

    }
    protected override ConsumableBase GetConsumableScript()
    {
        base.GetConsumableScript();
        interactionManager.PlayerResourceData.MeatFoodAmount--;

        //Raise event for scoring purposes
        onDangerousFoodEatenEC.RaiseEvent();

        return meatPrefab.GetComponent<ConsumableBase>();

    }
    //Used via IFoodItemInteraction interface to get food type
    public override GlobalValues.FoodItemType GetFoodItemType()
    {
        return FoodType;
    }


}

