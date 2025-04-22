using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValues;

[CreateAssetMenu(menuName = "Situations/Effects/World/WorldIsAllowedOutSituationEffect", fileName = "WorldIsAllowedOutSituationEffectSO")]
public class WorldIsAllowedOutSituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("State of isAllowedOut after this situation happens")]
    public bool isAllowedOut;

}
