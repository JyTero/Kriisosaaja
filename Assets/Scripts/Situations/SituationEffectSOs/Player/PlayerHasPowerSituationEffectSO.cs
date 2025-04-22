using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Situations/Effects/Player/PlayerHealthSituationEffectSO", fileName = "PlayerHealthSituationSO")]

public class PlayerHealthSituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("Health Offset caused by this effect")]
    public int healthOffset;
}
