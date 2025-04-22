using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValues;

[CreateAssetMenu(menuName = "Situations/Effects/World/WorldInfoQualitySituationEffect", fileName = "WorldInfoQualitySituationEffectSO")]
public class WorldInfoQualitySituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("Target value set by this situation effect")]
    public Quality newInfoQuality;

}
