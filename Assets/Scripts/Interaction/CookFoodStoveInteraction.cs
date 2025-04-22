using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;

public class CookFoodStoveInteraction : BaseInteraction
{
    [SerializeField]
    private VoidEventChannelSO onDangerousFoodEatenEC;

    [SerializeField]
    private GameObject OvenDoorParent;
    [SerializeField]
    private int doorOpenTargetRotation = 0;
    [SerializeField]
    private float doorOpenTime = 0;
    private int cookingTime = 0;
    private int mealHunger = 0, mealHydration = 0, mealMentalWellbeing = 0, mealHealth = 0;
    private int timer = 0;
    private float mealMultiplier = 1;
    private float rotateTimer = 0;
    private List<ConsumableBase> rawFoods = new();
    private StoveItem thisStove;

    private bool spoiltChannelRaised = false;
    private bool debug = false;
    protected override void Awake()
    {
        base.Awake();
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
        thisStove = thisItem as StoveItem;
        thisStove.MakingFood = true;

        mealHunger = 0;
        mealHydration = 0;
        mealMentalWellbeing = 0;
        cookingTime = 2;

        //Multiplier to make meal more than the sum of its parts
        mealMultiplier = 1;
        spoiltChannelRaised = false;

        //Go through each slot on each surface and see if it has food.
        foreach (SurfaceSlot slot in thisItem.itemManager.AllSlots)
        {
            //If food is found, add it a list
            if (slot.itemInSlot != null)
            {
                if (debug)
                    Debug.Log("Found Food: " + slot.itemInSlot.name);
                var slotItem = slot.itemInSlot.GetComponent<ConsumableBase>();

                mealHunger += slotItem.HungerChange;
                mealHydration += slotItem.HydrationChange;
                //if food gives negative mental wellbeing (= it's not meant to be eaten raw)
                //in meal the food gives mentalwellbeing instead of reducing it
                if (slotItem.MentalWellbeingChange < 0)
                {
                    mealMentalWellbeing += (slotItem.MentalWellbeingChange * -1) / 2;
                }
                else
                    mealMentalWellbeing += slotItem.MentalWellbeingChange;

                if (slotItem.HasSpoiled)
                {
                    mealHealth += slotItem.HealthChange;
                    if (!spoiltChannelRaised)
                    {
                        onDangerousFoodEatenEC.RaiseEvent();
                        spoiltChannelRaised = true;
                    }
                }

                mealMultiplier += 0.1f;
                cookingTime++;

                Destroy(slot.itemInSlot);

            }
        }

        //Reduce 0.1f from mealMultiplier to make it so that only multiple ingridients give bonus
        mealMultiplier -= 0.1f;


        MoveOvenDoor();

    }


    public void CookingTimer()
    {
        timer++;

        if (timer == cookingTime - 1)
        {
            MoveOvenDoor();
        }
        else if (timer >= cookingTime)
        {
            //cap multiplier, apply values to meal object and eat the meal
            if (mealMultiplier > 1.5)
                mealMultiplier = 1.5f;
            ConsumableBase meal = new();
            meal.HungerChange = (int)(mealHunger * mealMultiplier);
            meal.HydrationChange = (int)(mealHydration * mealMultiplier);
            meal.MentalWellbeingChange = (int)(mealMentalWellbeing * mealMultiplier);
            //If meal includes spoiled food, its health reduction effect depends on how many food items the meal includes
            //meal.HealthChange = (int)(mealHealth * (mealMultiplier - 1));
            meal.HealthChange = (int)(mealHealth * mealMultiplier);


            meal.EatFood(interactionManager, meal);
            if (debug)
                Debug.Log("MHu: " + mealHunger + "MHy: " + mealHydration + "MMeWe: " + mealMentalWellbeing
    + "MM: " + mealMultiplier);

            timer = 0;
            EndInteraction();
        }


    }

    private void MoveOvenDoor()
    {
        StartCoroutine(MoveOvenDoorCoro());
    }
    private IEnumerator MoveOvenDoorCoro()
    {
        rotateTimer = 0;

        //Open Door
        float anglePerFrame = doorOpenTargetRotation / doorOpenTime;
        while (rotateTimer <= doorOpenTime)
        {
            float angleThisFrame = anglePerFrame * Time.deltaTime;
            OvenDoorParent.transform.GetChild(0).Rotate(angleThisFrame, 0, 0, Space.Self);
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
            OvenDoorParent.transform.GetChild(0).Rotate(angleThisFrame, 0, 0, Space.Self);
            rotateTimer += Time.deltaTime;
            yield return null;
        }

    }

    protected override void EndInteraction()
    {
        thisStove.MakingFood = false;
        base.EndInteraction();
    }
}
