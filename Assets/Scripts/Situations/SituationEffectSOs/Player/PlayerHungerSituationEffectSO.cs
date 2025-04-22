using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Situations/Effects/Player/PlayerHungerSituationEffectSO", fileName = "PlayerHungerSituationSO")]

public class PlayerHungerSituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("Hunger Offset caused by this effect")]
    public int hungerOffset;
}
