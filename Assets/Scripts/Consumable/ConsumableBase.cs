using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ConsumableBase : BaseItem
{
    public string ConsumableName;
    public bool IsDangerous;
    public int HungerChange;
    public int HydrationChange;
    public int MentalWellbeingChange;
    public int HealthChange;

    public GlobalValues.FoodItemType foodType;

    public bool RequiresRefrdigeration;
    public bool RequiresFreezing;

    public int FoodSpoilTime;
    public int spoilProgeres = 0;

    [HideInInspector]
    public int WillSpoilTime = -1;
    public bool HasSpoiled
    {
        get { return hasSpoiled; }
        set
        {
            hasSpoiled = value;
            if (hasSpoiled)
            {
                OnFoodSpoil();
            }
        }
    }
    private bool hasSpoiled = false;


    [SerializeField]
    private Color materialColorDEBUG;

    private void Awake()
    {
        GetComponent<MeshRenderer>().material.color = materialColorDEBUG;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        InteractionSpotTransform = gameObject.transform.parent.transform.parent.GetComponent<SurfaceSlotParent>().SurfaceSlotInteractionTransform;
    }

    public void EatFood(InteractionManager interactManager, ConsumableBase foodEaten)
    {
        interactManager.AdjustPlayerHunger(foodEaten.HungerChange);
        interactManager.AdjustPlayerHydration(foodEaten.HydrationChange);
        interactManager.AdjustPlayerMentalWellbeing(foodEaten.MentalWellbeingChange);
        interactManager.AdjustPlayerHealth(foodEaten.HealthChange);
    }


    private void OnFoodSpoil()
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer.sharedMaterial != null)
        {
            var baseColor = meshRenderer.sharedMaterial.color;
            var newColor = baseColor * 0.4f;
            meshRenderer.sharedMaterial.color = newColor;

        }

        this.HungerChange /= 2;
        this.HydrationChange /= 2;
        //If item gives mentalwellbeing, invert and double that for new mw value. If no Mw adjustment, give a value.
        //Food already reducing MW double their reduction
        if (this.MentalWellbeingChange > 0)
        {
            this.MentalWellbeingChange *= -2;

        }
        else if (this.MentalWellbeingChange == 0)
        {
            this.MentalWellbeingChange -= 15;
        }
        else
            this.MentalWellbeingChange *= 2;

        if (this.HealthChange == 0)
            this.HealthChange = -5;
        else if (this.HealthChange > 0)
            this.HealthChange *= -1;
        else
            //Adjust should be 1.5, would require further code to support trucating float to int
            this.HealthChange *= 2;

    }

    protected override void TimeBeat()
    {
        //If spoiltime is -1, food is unspoilable
        if (FoodSpoilTime == -1)
            return;

        if (!HasSpoiled)
        {
            spoilProgeres++;
            if (spoilProgeres >= FoodSpoilTime)
            {
                HasSpoiled = true;
            }
        }

    }

}
