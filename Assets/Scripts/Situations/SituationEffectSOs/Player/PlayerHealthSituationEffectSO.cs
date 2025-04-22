using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Situations/Effects/Player/PlayerHasPowerSituationEffectSO", fileName = "PlayerHasPowerSituationSO")]

public class PlayerHasPowerSituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("State of hasPower after this situation happens")]
    public bool hasPower;
}
