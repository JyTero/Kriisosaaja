using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : BaseItem
{
    [HideInInspector]
    public bool playerSleeps = false;
    
    private Sleep_Interaction sleepInteraction; 

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();   
        sleepInteraction = gameObject.GetComponent<Sleep_Interaction>();
    }

    protected override void TimeBeat()
    {
        if(playerSleeps)
        {
            sleepInteraction.SleepTick();
        }
    }
}
