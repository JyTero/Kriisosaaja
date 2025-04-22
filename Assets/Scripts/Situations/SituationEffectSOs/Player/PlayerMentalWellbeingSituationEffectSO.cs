using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Situations/Effects/Player/PlayerMentalWellbeingSituationEffectSO", fileName = "PlayerMentalWellbeingSituationSO")]

public class PlayerMentalWellbeingSituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("Mentall Wellbeing Offset caused by this effect")]
    public int mentalWellbeingOffset;
}
