using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class SinkItem : BaseItem, IRequiresWater, IRequiresPower
{

    public int SinkContainerCapasity;
    public int SinkContainersInUse;
    public bool NoWater { get; set; }
    public bool NoPower { get; set; }

    [HideInInspector]
    public bool drinking = false;
    [HideInInspector]
    public bool fillingContainer = false;

    [HideInInspector]
    public UseSink_Interaction useSinkInteraction;
    [HideInInspector]
    public FillContainerSink_Interaction fillContainerSink_Interaction;

    private PlayerResourceData resourceData;

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

        resourceData = FindFirstObjectByType<PlayerResourceData>();
        SinkContainerCapasity = resourceData.LiquidContainersCapasity;
        SinkContainersInUse = resourceData.usedLiquidContainers;

        useSinkInteraction = gameObject.GetComponent<UseSink_Interaction>();
        fillContainerSink_Interaction = gameObject.GetComponent<FillContainerSink_Interaction>();

        SignUpOnItemManagerRequiresWater();
        SignUpOnItemManagerRequiresPower();
    }

    protected override void TimeBeat()
    {
        if (drinking)
            useSinkInteraction.SinkDrinkTick();
        else if (fillingContainer)
            fillContainerSink_Interaction.SinkTick();
    }

    public void OnWaterOuttageBegin()
    {
        NoWater = true;
    }

    public void OnWaterResume()
    {
        NoWater = false;
        useSinkInteraction.enabled = true;
        Interactions.Add(useSinkInteraction);
        fillContainerSink_Interaction.enabled = true;
        Interactions.Add(fillContainerSink_Interaction);
    }
    public void OnPowerOuttageBegin()
    {
        NoPower = true;
    }

    public void OnPowerResume()
    {
        NoPower = false;
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
}
