using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRequiresWater 
{
    public bool NoWater { get; set; }

    //Used to add item to ItemManager requiresPowerItems list
    public void SignUpOnItemManagerRequiresWater();
    public void SignOffFromItemManagerRequiresWater();

    //Used to handle what happens when the power is actually cut
    public void OnWaterOuttageBegin();

    public void OnWaterResume();
}

