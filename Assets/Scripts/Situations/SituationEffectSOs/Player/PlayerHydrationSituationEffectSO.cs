using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Situations/Effects/Player/PlayerHydrationSituationEffectSO", fileName = "PlayerHydrationSituationSO")]

public class PlayerHydrationSituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("Hydration Offset caused by this effect")]
    public int hydrationOffset;
}
