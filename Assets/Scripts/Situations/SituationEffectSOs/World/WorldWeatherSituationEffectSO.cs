using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValues;

[CreateAssetMenu(menuName = "Situations/Effects/World/WorldWeatherSituatioEffect", fileName = "WorldWeatherSituatioEffectSO")]
public class WorldWeatherSituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("New Weather set by this situation effect")]
    public Weather newWeather;
}
