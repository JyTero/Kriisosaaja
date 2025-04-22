using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableStove : BaseItem
{
    [HideInInspector]
    public bool MakingFood = false;

    private CookFoodPortableStoveInteraction cookfoodPortableInteraction;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        cookfoodPortableInteraction = GetComponent<CookFoodPortableStoveInteraction>();

    }
    protected override void TimeBeat()
    {
        if (MakingFood)
            cookfoodPortableInteraction.CookingTimer();
    }

}
