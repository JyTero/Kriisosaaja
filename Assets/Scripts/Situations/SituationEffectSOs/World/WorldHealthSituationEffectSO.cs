using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValues;

[CreateAssetMenu(menuName = "Situations/Effects/World/WorldHealthSituationEffect", fileName = "WorldHealthSituationEffectSO")]
public class WorldHealthSituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("Target value set by this situation effect")]
    public Quality newHealth;

}
