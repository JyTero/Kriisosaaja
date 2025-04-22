using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValues;

public class TuneToNationalBroadcastingRadioInteraction : ListenToRadioBaseInteraction
{
    protected override void DuringInteraction()
    {
        var airQ = interactionManager.GetWorldAirQ();
        
        radioText = radioTexts[0];
        radioText += airQ.ToString();
        Radio radio = thisItem as Radio;
        radio.ListenToPublicBroadcastRadio(radioTextDuration, radioText);
        EndInteraction();
    }


    //Copy/Paste from above with only difference being the method called on the last line
    public void RaiseWeatherAlert()
    {
        radioText = radioTexts[0];
        radioText += interactionManager.GetWorldAirQ().ToString();
        Radio radio = thisItem as Radio;
        radio.ShowPassiveNotification(radioTextDuration, radioText);
    }
    }
