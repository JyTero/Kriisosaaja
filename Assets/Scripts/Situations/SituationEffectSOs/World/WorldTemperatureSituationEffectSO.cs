using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Situations/Effects/World/WorldTemperatureSituationEffect", fileName = "WorldTemperatureSituationEffectSO")]
public class WorldTemperatureSituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("Target value set by this situation effect")]
    public int temperatureOffset;
}