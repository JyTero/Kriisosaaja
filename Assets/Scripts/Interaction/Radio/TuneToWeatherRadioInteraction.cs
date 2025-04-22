using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneToWeatherRadioInteraction : ListenToRadioBaseInteraction
{
    protected override void DuringInteraction()
    {
        radioText = radioTexts[0];
        radioText += interactionManager.GetWorldWeather().ToString();
        radioText += radioTexts[1];
        radioText += interactionManager.GetWorldTemperature().ToString();
        radioText += radioTexts[2];
        Radio radio = thisItem as Radio;
        radio.ListenToWeatherRadio(radioTextDuration, radioText);

        EndInteraction();
    }

    //Copy/Paste from above with only difference being the method called on the last line
    public void RaiseWeatherAlert()
    {
        radioText = radioTexts[0];
        radioText += interactionManager.GetWorldWeather().ToString();
        radioText += radioTexts[1];
        radioText += interactionManager.GetWorldTemperature().ToString();
        radioText += radioTexts[2];
        Radio radio = thisItem as Radio;
        radio.ShowPassiveNotification(radioTextDuration, radioText);
    }

}
