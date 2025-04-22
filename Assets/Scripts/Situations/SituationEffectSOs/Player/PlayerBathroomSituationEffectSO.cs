using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Situations/Effects/Player/PlayerBathroomSituationEffectSO", fileName = "PlayerBathroomSituationSO")]

public class PlayerBathroomSituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("Bathroom Offset caused by this effect")]
    public int bathroomOffset;
}
