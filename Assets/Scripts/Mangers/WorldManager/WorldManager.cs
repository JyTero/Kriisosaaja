using Unity.VisualScripting;
using UnityEngine;
using static GlobalValues;

public class WorldManager : MonoBehaviour
{
    //The world values, get value form Scenario situations
    private int currentWorldTemp;
    private Weather currentWorldWeather;
    private Quality currentWorldAirQuality;
    private Quality currentWorldHealth;
    private Quality currentWorldInfoQuality;
    private bool currentIsAllowedOut;


    [Header("Channels To Adjust Stats")]
    [SerializeField]
    private IntEventChannelSO worldTemperatureAdjustEventChannel;
    [SerializeField]
    private WeatherEventChannelSO worldWeatherAdjustEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldHealthAdjustEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldInfoQualityAdjustEventChannel;
    [SerializeField]
    private BoolEventChannelSO worldIsAllowedOutAdjustEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldAirQualityAdjustEventChannel;


    [Header("Channels To Read Stats")]
    [SerializeField]
    private IntEventChannelSO worldTemperatureReadEventChannel;
    [SerializeField]
    private WeatherEventChannelSO worldWeatherReadEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldHealthReadEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldInfoQualityReadEventChannel;
    [SerializeField]
    private BoolEventChannelSO worldIsAllowedOutReadEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldAirQualityReadEventChannel;

    [Header("Other ECs")]
    [SerializeField]
    private WeatherEventChannelSO onWeatherChangeEC;
    [SerializeField]
    private QualityEventChannelSO onAirQualityChange;

    [Header("DEBUG")]
    [SerializeField]
    private bool enableWorldDebugLogs;

    private UI_Handler uiHandler;

    //Can and probably should be refactored away due to EC adjust methods exsiting and able to do the job
    public int WorldTemperature
    {
        get { return currentWorldTemp; }
        set
        {
            currentWorldTemp = value;

            //if (currentWorldTemp == 10)
            //    worldTemperatureAdjustEventChannel.OnEventRaised(currentWorldTemp);

            if (uiHandler != null)
            {
                uiHandler.worldTemperature.text = "World temperature: " + currentWorldTemp + "";
            }
        }
    }
    public Weather worldWeather
    {
        get { return currentWorldWeather; }
        set
        {
            currentWorldWeather = value;
            onWeatherChangeEC.RaiseEvent(worldWeather);
            if (uiHandler != null)
            {
                uiHandler.worldWeather.text = "World weather: " + currentWorldWeather;
            }
        }
    }
    public Quality worldAirQuality
    {
        get { return currentWorldAirQuality; }
        set
        {
            currentWorldAirQuality = value;
            onAirQualityChange.RaiseEvent(worldAirQuality);
            if (uiHandler != null)
            {
                uiHandler.worldAirQ.text = "World air quality: " + currentWorldAirQuality;
            }
        }
    }
    public Quality worldHealth
    {
        get { return currentWorldHealth; }
        set
        {
            currentWorldHealth = value;
            if (uiHandler != null)
            {
                uiHandler.worldHealth.text = "World health: " + currentWorldHealth;
            }
        }
    }
    public Quality worldInfoQuality
    {
        get { return currentWorldInfoQuality; }
        set
        {
            currentWorldInfoQuality = value;
            if (uiHandler != null)
            {
                uiHandler.worldInfo.text = "World information quality: " + currentWorldInfoQuality;
            }
        }
    }
    public bool isAllowedOut
    {
        get { return currentIsAllowedOut; }
        set
        {
            currentIsAllowedOut = value;
            if (uiHandler != null)
            {
                uiHandler.worldCurfew.text = "Is allowed out: " + currentIsAllowedOut;
            }
        }
    }

    private void OnEnable()
    {
        //Set Channels
        if (worldTemperatureAdjustEventChannel != null)
            worldTemperatureAdjustEventChannel.OnEventRaised += OnWorldTempChange;
        if (worldWeatherAdjustEventChannel != null)
            worldWeatherAdjustEventChannel.OnEventRaised += OnWorldWeatherChange;
        if (worldAirQualityAdjustEventChannel != null)
            worldAirQualityAdjustEventChannel.OnEventRaised += OnAirQualityChange;
        if (worldHealthAdjustEventChannel != null)
            worldHealthAdjustEventChannel.OnEventRaised += OnWorldHealthChange;
        if (worldInfoQualityAdjustEventChannel != null)
            worldInfoQualityAdjustEventChannel.OnEventRaised += OnInfoQualityChange;
        if (worldIsAllowedOutAdjustEventChannel != null)
            worldIsAllowedOutAdjustEventChannel.OnEventRaised += OnIsAllowedOutChange;
        //Read Channesl
        if (worldTemperatureReadEventChannel != null)
            worldTemperatureReadEventChannel.OnEventWReturnRaised += GetTemperature;
        if (worldWeatherReadEventChannel != null)
            worldWeatherReadEventChannel.OnEventWReturnRaised += GetWeather;
        if (worldAirQualityReadEventChannel != null)
            worldAirQualityReadEventChannel.OnEventWReturnRaised += GetAirQ;
        if (worldHealthReadEventChannel != null)
            worldHealthReadEventChannel.OnEventWReturnRaised += GetHealthQ;
        if (worldInfoQualityReadEventChannel != null)
            worldInfoQualityReadEventChannel.OnEventWReturnRaised += GetInfoQ;
        if (worldIsAllowedOutAdjustEventChannel != null)
            worldIsAllowedOutReadEventChannel.OnEventWReturnRaised += IsAllowedOut;

    }
    private void OnDisable()
    {
        worldTemperatureAdjustEventChannel.OnEventRaised -= OnWorldTempChange;
        worldWeatherAdjustEventChannel.OnEventRaised -= OnWorldWeatherChange;
        worldAirQualityAdjustEventChannel.OnEventRaised -= OnAirQualityChange;
        worldHealthAdjustEventChannel.OnEventRaised -= OnWorldHealthChange;
        worldInfoQualityAdjustEventChannel.OnEventRaised -= OnInfoQualityChange;
        worldIsAllowedOutAdjustEventChannel.OnEventRaised -= OnIsAllowedOutChange;

        worldTemperatureReadEventChannel.OnEventWReturnRaised -= GetTemperature;
        worldWeatherReadEventChannel.OnEventWReturnRaised -= GetWeather;
        worldAirQualityReadEventChannel.OnEventWReturnRaised -= GetAirQ;
        worldHealthReadEventChannel.OnEventWReturnRaised -= GetHealthQ;
        worldInfoQualityReadEventChannel.OnEventWReturnRaised -= GetInfoQ;
        worldIsAllowedOutReadEventChannel.OnEventWReturnRaised -= IsAllowedOut;

    }
    private void Start()
    {
        uiHandler = FindObjectOfType<UI_Handler>();
    }

    #region Methods to Adjust Values
    private void OnWorldTempChange(int offset)
    {
        WorldTemperature += offset;
        if (enableWorldDebugLogs)
            Debug.Log("World Temperature Set");
    }
    private void OnWorldWeatherChange(Weather newWeather)
    {
        worldWeather = newWeather;
        if (enableWorldDebugLogs)
            Debug.Log("World Weather Set");
    }
    private void OnAirQualityChange(Quality newQuality)
    {
        worldAirQuality = newQuality;
        if (enableWorldDebugLogs)
            Debug.Log("World Air Quality Set");
    }
    private void OnWorldHealthChange(Quality newHealth)
    {
        worldHealth = newHealth;
        if (enableWorldDebugLogs)
            Debug.Log("World Health Set");
    }
    private void OnInfoQualityChange(Quality newInfoQuality)
    {
        worldInfoQuality = newInfoQuality;
        if (enableWorldDebugLogs)
            Debug.Log("World InfoQuality Set");
    }
    private void OnIsAllowedOutChange(bool allowedOut)
    {
        isAllowedOut = allowedOut;
        if (enableWorldDebugLogs)
            Debug.Log("World IsAllowedOut Set");
    }
    #endregion

    #region Methods to get values
    //Methods to get world values
    private int GetTemperature()
    {
        return WorldTemperature;
    }
    private Weather GetWeather()
    {
        return worldWeather;
    }
    private Quality GetAirQ()
    {
        return worldAirQuality;
    }
    private Quality GetHealthQ()
    {
        return worldHealth;
    }
    private Quality GetInfoQ()
    {
        return worldInfoQuality;
    }
    private bool IsAllowedOut()
    {
        return isAllowedOut;
    }
    #endregion
}

