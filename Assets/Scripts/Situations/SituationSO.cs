using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Situations/Situation", fileName = "SituationSO")]

public class SituationSO : ScriptableObject
{

    [SerializeField]
    public string situationName;
    
    [TextArea(5, 20)]
    [SerializeField]
    public string situationDescription;

    [SerializeField]
    public List<BaseSituationEffectSO> situationEffects;

    [Tooltip("How long after this situation before the next one is ran.")]
    [SerializeField]
    public int SituationDuration;   
    

}
