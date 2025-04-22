using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Situations/Effects/Player/PlayerEnergySituationEffectSO", fileName = "PlayerEnergySituationSO")]

public class PlayerEnergySituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("Energy Offset caused by this effect")]
    public int energyOffset;
}
