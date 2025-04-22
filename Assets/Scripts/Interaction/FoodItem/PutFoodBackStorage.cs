using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutFoodBackStorage : BaseInteraction
{
    [SerializeField]
    protected VoidEventChannelSO playerAtDestinationSecondaryEC;
    private ConsumableBase thisConsumable;
    //[SerializeField]
    //private Transform outOfViewTransform;


    protected override void Awake()
    {
        base.Awake();

    }
    protected override void Start()
    {
        thisConsumable = GetComponent<ConsumableBase>();
    }
    public override void OnInteractionBegin()
    {
        base.OnInteractionBegin();
        // Route to interaction spot, if applicaple 
        playerAtDestinationEC.OnEventRaised += AtDestination;
        interactionManager.SetDestination(thisItem.InteractionSpotTransform.position);

    }

    //Raised when player arrives to their current destination
    protected override void AtDestination()
    {
        base.AtDestination();
        //Stuff player does when arriving to the interaction spot

        DuringInteraction();

    }


    //The "Main" of the Interaction
    private void DuringInteraction()
    {
        //Route to the item
        //"pick" item up
        gameObject.transform.localPosition= new Vector3(100, 100, 100);

        //Route to target storage
        if (thisConsumable.foodType == GlobalValues.FoodItemType.Meat || thisConsumable.foodType == GlobalValues.FoodItemType.Milk)
        {
            playerAtDestinationSecondaryEC.OnEventRaised += AtDestinationSecondary;
           interactionManager.SetDestination(interactionManager.SceneFridge.InteractionSpotTransform.position);

        }
        //Assume kitchen counter storage type
        else
        {
            playerAtDestinationSecondaryEC.OnEventRaised += AtDestinationSecondary;
            interactionManager.SetDestination(interactionManager.SceneKitchenCounter.InteractionSpotTransform.position);

        }
    }

    private void AtDestinationSecondary()
    {
        playerAtDestinationSecondaryEC.OnEventRaised -= AtDestinationSecondary;
        Debug.Log("Happenings!");

        switch (thisConsumable.foodType)
        {
            case GlobalValues.FoodItemType.Meat:
                interactionManager.PlayerResourceData.MeatFoodAmount++;
                Destroy(gameObject);
                break;
            case GlobalValues.FoodItemType.Vegetable:
                interactionManager.PlayerResourceData.VegetableFoodAmount++;
                Destroy(gameObject);
                break;
            case GlobalValues.FoodItemType.Pasta:
                interactionManager.PlayerResourceData.PastaFoodAmount++;
                Destroy(gameObject);
                break;
            case GlobalValues.FoodItemType.CannedFood:
                interactionManager.PlayerResourceData.CannedFoodAmount++;
                Destroy(gameObject);
                break;
            case GlobalValues.FoodItemType.Milk:
                interactionManager.PlayerResourceData.MilkDrinkAmount++;
                Destroy(gameObject);
                break;
            case GlobalValues.FoodItemType.Water:
                interactionManager.PlayerResourceData.WaterDrinkAmount++;
                Destroy(gameObject);
                break;
            case GlobalValues.FoodItemType.Juice:
                interactionManager.PlayerResourceData.JuiceDrinkAmount++;
                Destroy(gameObject);
                break;
            default:
                Debug.LogWarning("Tried to put unknown foodtype to storage");
                break;

        }

        EndInteraction();
    }


}