using JetBrains.Annotations;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [HideInInspector]
    public List<SurfaceSlot> AllSlots = new();

    [HideInInspector]
    public List<IRequiresPower> requiresPowerItems = new();
    [HideInInspector]
    public List<IRequiresWater> requiresWaterItems = new();

    [HideInInspector]
    public bool WindowIsSealed = false;

    [Header("Channels")]
    //Power and water channels raised from Player
    [SerializeField]
    private VoidEventChannelSO powerTurnedOffEC;
    [SerializeField]
    private VoidEventChannelSO powerTurnedOnEC;
    [SerializeField]
    private VoidEventChannelSO waterTurnedOffEC;
    [SerializeField]
    private VoidEventChannelSO waterTurnedOnEC;
    [SerializeField]
    private IntEventChannelSO readTimeManagerTimeEC;
    [SerializeField]
    public VoidEventChannelSO TimeBeatEC;

    private NotificationManager notifManager;
    private void OnEnable()
    {
        powerTurnedOffEC.OnEventRaised += OnPowerLoss;
        powerTurnedOnEC.OnEventRaised += OnPowerResume;
        waterTurnedOffEC.OnEventRaised += OnWaterLoss;
        waterTurnedOnEC.OnEventRaised += OnWaterResume;


    }

    private void Awake()
    {
        notifManager = FindObjectOfType<NotificationManager>();
        
    }
    private void Start()
    {
    }

    public void ShowNoticationText(string notification, int duration)
    {
        notifManager.NewNotification(notification);

    }

    //Called when the electricity is lost
    private void OnPowerLoss()
    {
        foreach (IRequiresPower requiresPower in requiresPowerItems)
        {
            requiresPower.OnPowerOuttageBegin();
        }
    }

    private void OnPowerResume()
    {
        foreach (IRequiresPower requiresPower in requiresPowerItems)
        {
            requiresPower.OnPowerResume();
        }
    }

    //Called when the water is lost
    private void OnWaterLoss()
    {
        foreach (IRequiresWater requiresWater in requiresWaterItems)
        {
            requiresWater.OnWaterOuttageBegin();
        }
    }

    private void OnWaterResume()
    {
        foreach (IRequiresWater requiresWater in requiresWaterItems)
        {
            requiresWater.OnWaterResume();
        }
    }

    //ECs
    public int GetTimeManagerTime()
    {
        return readTimeManagerTimeEC.OnEventWReturnRaised();
    }

    private void OnDisable()
    {
        powerTurnedOffEC.OnEventRaised -= OnPowerLoss;
        powerTurnedOnEC.OnEventRaised -= OnPowerResume;
        waterTurnedOffEC.OnEventRaised -= OnWaterLoss;
        waterTurnedOnEC.OnEventRaised -= OnWaterResume;
    }

}
