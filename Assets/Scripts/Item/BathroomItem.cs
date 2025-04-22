using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BathroomItem : BaseItem, IRequiresWater, IRequiresPower
{
    [HideInInspector]
    public bool bathroomInUse = false;
    [HideInInspector]
    public bool dryBathroomInUse = false;
    [HideInInspector]
    public bool dryBathroomUnderPrep = false;
    [HideInInspector]
    public bool dryBathroomBeingRemoved = false;
    [HideInInspector]
    public bool dryBathroomReady = false;

    [HideInInspector]
    public int usedUnpowerBathroomCapasity = -1;

    [HideInInspector]
    public UseBathroom_Interaction useBathroomInteraction;
    [HideInInspector]
    public UseBathroomDry_Interaction useDryBathroomInteraction;
    [HideInInspector]
    public PrepareDryToilet_Interaction prepareDryToiletInteraction;
    [HideInInspector]
    public RemoveDryToilet_Interaction removeDryToiletInteraction;



    public bool NoWater { get; set; }
    public bool NoPower { get; set; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        string s = "";
        foreach (BaseInteraction interaction in Interactions)
        {
            s += interaction.InteractionName + "\n";
        }
        Debug.Log(s);
        useBathroomInteraction = gameObject.GetComponent<UseBathroom_Interaction>();
        useDryBathroomInteraction = gameObject.GetComponent<UseBathroomDry_Interaction>();
        prepareDryToiletInteraction = gameObject.GetComponent<PrepareDryToilet_Interaction>();
        removeDryToiletInteraction = gameObject.GetComponent<RemoveDryToilet_Interaction>();

        SignUpOnItemManagerRequiresWater();
        SignUpOnItemManagerRequiresPower();
    }

    protected override void TimeBeat()
    {

        if (bathroomInUse)
            useBathroomInteraction.ToiletTick();
        else if (dryBathroomInUse)
            useDryBathroomInteraction.ToiletTick();
        else if (dryBathroomUnderPrep)
            prepareDryToiletInteraction.PrepareDryToiletTick();
        else if (dryBathroomBeingRemoved)
            removeDryToiletInteraction.RemoveDryToiletTick();
            

    }

    public void OnWaterOuttageBegin()
    {
        NoWater = true;
    }

    public void OnWaterResume()
    {
        NoWater = false;

        if (useDryBathroomInteraction == false)
        {
            useBathroomInteraction.enabled = true;
            Interactions.Add(useBathroomInteraction);
        }
        //If both utilites exist, reset unpowered capasity
        if (NoPower == false)
        {
            useBathroomInteraction.ResetUnusedUnpoweredCapasity();
        }
    }
    public void OnPowerOuttageBegin()
    {
        NoPower = true;
    }

    public void OnPowerResume()
    {
        NoPower = false;    
        //If both utilites exist, reset unpowered capasity
        if(NoWater == false) 
        {
            useBathroomInteraction.ResetUnusedUnpoweredCapasity();
        }
    }

    public void SignUpOnItemManagerRequiresWater()
    {
        itemManager.requiresWaterItems.Add(this);
    }

    public void SignOffFromItemManagerRequiresWater()
    {
        itemManager.requiresWaterItems.Remove(this);
    }

    public void SignUpOnItemManagerRequiresPower()
    {
        itemManager.requiresPowerItems.Add(this);
    }

    public void SignOffFromItemManagerRequiresPower()
    {
        itemManager.requiresPowerItems.Remove(this);
    }

    //Called when returning to scenario scene with dry toilet prepared
    public void PrepareDryToiletOnLoad()
    {
        prepareDryToiletInteraction.PrepareDryToiletOnLoad();
    }

    private void OnDisable()
    {
        SignOffFromItemManagerRequiresWater();
        SignOffFromItemManagerRequiresPower();
    }

}
