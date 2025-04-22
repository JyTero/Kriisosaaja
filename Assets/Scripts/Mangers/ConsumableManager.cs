using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValues;

public class ConsumableManager : MonoBehaviour
{
    [Header("Channels To Adjust Player Stats")]
    [SerializeField]
    private FloatEventChannelSO playerMoneyAdjustEventChannel;
    [SerializeField]
    private IntEventChannelSO playerMentalWellbeingAdjustEventChannel;
    [SerializeField]
    private IntEventChannelSO playerHungerAdjustEventChannel;
    [SerializeField]
    private IntEventChannelSO playerHydrationAdjustEventChannel;
    [SerializeField]
    private IntEventChannelSO playerBathroomAdjustEventChannel;
    [SerializeField]
    private IntEventChannelSO playerHealthAdjustEventChannel;
    [SerializeField]
    private BoolEventChannelSO playerHasElectrcityAdjustEventChannel;
    [SerializeField]
    private BoolEventChannelSO playerHasWaterAdjustEventChannel;

    [Header("Channels To Adjust World Stats")]
    [SerializeField]
    private IntEventChannelSO worldTemperatureAdjustEventChannel;
    [SerializeField]
    private WeatherEventChannelSO worldWeatherAdjustEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldHealthQualityAdjustEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldInfoQualityAdjustEventChannel;
    [SerializeField]
    private BoolEventChannelSO worldIsAllowedOutAdjustEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldAirQualityAdjustEventChannel;

      //Player Adjust
    public void AdjustPlayerMoney(float offset)
    {
        playerMoneyAdjustEventChannel.OnEventRaised(offset);
    }
    public void AdjustPlayerMentalWellbeing(int offest)
    {
        playerMentalWellbeingAdjustEventChannel.OnEventRaised(offest);
    }
    public void AdjustPlayerHunger(int offset)
    {
        playerHungerAdjustEventChannel.OnEventRaised(offset);
    }
    public void AdjustPlayerHydration(int offest)
    {
        playerHydrationAdjustEventChannel.OnEventRaised(offest);
    }
    public void AdjustPlayerBathroom(int offest)
    {
        playerBathroomAdjustEventChannel.OnEventRaised(offest);
    }
    public void AdjustPlayerHealth(int offest)
    {
        playerHealthAdjustEventChannel.OnEventRaised(offest);
    }
    public void AdjustPlayerHasElectricity(bool hasElectricity)
    {
        playerHasElectrcityAdjustEventChannel.OnEventRaised(hasElectricity);
    }
    public void AdjustPlayerHasWater(bool hasWater)
    {
        playerHasWaterAdjustEventChannel.OnEventRaised(hasWater);
    }
    //World Adjust
    public void AdjustWorldTemperature(int offest)
    {
        worldTemperatureAdjustEventChannel.OnEventRaised(offest);
    }
    public void AdjustWorldWeather(Weather newWeather)
    {
        worldWeatherAdjustEventChannel.OnEventRaised(newWeather);
    }
    public void AdjustWorldHealthQ(Quality newHealthQ)
    {
        worldHealthQualityAdjustEventChannel.OnEventRaised(newHealthQ);
    }
    public void AdjustWorldAirQ(Quality newAirQ)
    {
        worldAirQualityAdjustEventChannel.OnEventRaised(newAirQ);
    }
    public void AdjustWorldInfoQ(Quality newInfoQ)
    {
        worldInfoQualityAdjustEventChannel.OnEventRaised(newInfoQ);
    }
    public void AdjustWorldIsAllowedOut(bool isAllowedOut)
    {
        worldIsAllowedOutAdjustEventChannel.OnEventRaised(isAllowedOut);
    }
}
