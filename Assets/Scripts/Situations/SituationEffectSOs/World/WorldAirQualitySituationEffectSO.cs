using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValues;

[CreateAssetMenu(menuName = "Situations/Effects/World/WorldAirQualitySituatioEffect", fileName = "WorldAirQualitySituatioEffectSO")]
public class WorldAirQualitySituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("New Air Quality set by this situation effect")]
    public Quality newAirQuality;
}