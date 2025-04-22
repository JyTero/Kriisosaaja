using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Radio : BaseItem
{
    [SerializeField]
    private WeatherEventChannelSO onWeatherChangeEC;
    [SerializeField]
    private QualityEventChannelSO onAirQualityChangeEC;

    private TuneToWeatherRadioInteraction tuneToWeatherChannel;
    private TuneToNationalBroadcastingRadioInteraction tuneToNationalBroadcast;

    protected override void Start()
    {
        base.Start();
        tuneToWeatherChannel = GetComponent<TuneToWeatherRadioInteraction>();
        tuneToNationalBroadcast = GetComponent<TuneToNationalBroadcastingRadioInteraction>();
    }


    public void ShowPassiveNotification(int timerTime, string radioText)
    {
        itemManager.ShowNoticationText(radioText, timerTime);
    }

    public void ListenToWeatherRadio(int timerTime, string radioText)
    {
        onAirQualityChangeEC.OnEventRaised -= RaiseBroadcastRadioAlert;
        itemManager.ShowNoticationText(radioText, timerTime);
        onWeatherChangeEC.OnEventRaised += RaiseWeatherRadioAlert;
    }

    public void ListenToPublicBroadcastRadio(int timerTime, string radioText)
    {
        onWeatherChangeEC.OnEventRaised -= RaiseWeatherRadioAlert;
        itemManager.ShowNoticationText(radioText, timerTime);
        onAirQualityChangeEC.OnEventRaised += RaiseBroadcastRadioAlert;
    }

    private void RaiseWeatherRadioAlert(GlobalValues.Weather weather)
    {
        tuneToWeatherChannel.RaiseWeatherAlert();
    }

    private void RaiseBroadcastRadioAlert(GlobalValues.Quality airQ)
    {
        tuneToNationalBroadcast.RaiseWeatherAlert();
    }

    private void OnDisable()
    {
       UnsubscribeAllRadioECs();
    }

    public void UnsubscribeAllRadioECs()
    {
        onWeatherChangeEC.OnEventRaised -= RaiseWeatherRadioAlert;
        onAirQualityChangeEC.OnEventRaised -= RaiseBroadcastRadioAlert;
    }
}
