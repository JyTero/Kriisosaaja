using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Situations/Effects/Player/PlayerHasWaterSituationEffectSO", fileName = "PlayerHasWaterSituationSO")]

public class PlayerHasWaterSituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("State of hasWater after this situation happens")]
    public bool hasWater;
}
