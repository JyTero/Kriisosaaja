using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoveItem : BaseItem, IRequiresPower
{
    public bool NoPower { get; set; }
    [HideInInspector]
    public bool MakingFood = false;

    private CookFoodStoveInteraction cookFoodInteraction;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SignUpOnItemManagerRequiresPower();
        cookFoodInteraction = GetComponent<CookFoodStoveInteraction>();

       // TimeBeatEC.OnEventRaised += TimeBeat;

    }
    public void OnPowerOuttageBegin()
    {
        Interactions = new();
    }

    public void OnPowerResume()
    {
        Interactions = GetComponents<BaseInteraction>().ToList<BaseInteraction>();
    }

    public void SignUpOnItemManagerRequiresPower()
    {
        //Add self to a list of items requiring electrcity 
        itemManager.requiresPowerItems.Add(this);
    }

    public void SignOffFromItemManagerRequiresPower()
    {
        itemManager.requiresPowerItems.Remove(this);
    }

    protected override void TimeBeat()
    {
        if (MakingFood)
            cookFoodInteraction.CookingTimer();

    }

    private void OnDisable()
    {
        SignOffFromItemManagerRequiresPower();
        TimeBeatEC.OnEventRaised -= TimeBeat;
    }
}
