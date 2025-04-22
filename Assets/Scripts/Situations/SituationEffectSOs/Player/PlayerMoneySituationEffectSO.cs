using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Situations/Effects/Player/PlayerMoneySituationEffect", fileName = "MoneySituationEffectSO")]
public class PlayerMoneySituationEffectSO : BaseSituationEffectSO
{
    [Tooltip("Value adjustment by this situation effect")]
    public float moneyOffset;

}
