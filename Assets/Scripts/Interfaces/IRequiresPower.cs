using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRequiresPower 
{
    public bool NoPower { get; set; }
    //Used to add item to ItemManager requiresPowerItems list
    public void SignUpOnItemManagerRequiresPower();
    public void SignOffFromItemManagerRequiresPower();

    //Used to handle what happens when the power is actually cut
    public void OnPowerOuttageBegin();

    public void OnPowerResume();
}
