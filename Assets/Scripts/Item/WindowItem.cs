using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowItem : BaseItem
{

    [HideInInspector]
    public bool sealingWindow = false;
    [HideInInspector]
    public bool unsealingWindow = false;

    [HideInInspector]
    public SealWindowInteraction sealWindowInteraction;
    [HideInInspector]
    public UnsealWindowInteraction unsealWindowInteraction;

    [SerializeField]
    private int showNotificationTimer;
    [SerializeField]
    private Color clearWeather;
    [SerializeField]
    private Color snowWeather;
    [SerializeField]
    private Color rainWeather;
    [SerializeField]
    private Color thunderWeather;
    [SerializeField]
    private Color blizzardWeather;
    [SerializeField]
    private Color nightTint;

    private MeshRenderer meshRenderer;

    private int showWeatherTimer = 0;
    protected override void Start()
    {
        base.Start();

        meshRenderer = GetComponent<MeshRenderer>();
        sealWindowInteraction = GetComponent<SealWindowInteraction>();
        unsealWindowInteraction = GetComponent<UnsealWindowInteraction>();

        TimeBeatEC.OnEventRaised += TimeBeat;
    }

    protected override void TimeBeat()
    {
        if (sealingWindow)
            sealWindowInteraction.SealWindowTick();
        if (unsealingWindow)
            unsealWindowInteraction.UnsealWindowTick();
        //Fix to run only when the interaction was used
        showWeatherTimer++;
    }

    //Called when returning to scenario scene with window sealed
    public void SealWindowOnLoad()
    {
        sealWindowInteraction.SealWindowOnLoad();
    }
    private void OnDisable()
    {
        TimeBeatEC.OnEventRaised -= TimeBeat;
    }
}
