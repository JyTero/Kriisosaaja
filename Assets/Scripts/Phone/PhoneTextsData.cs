using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SomeoneSentText/SentText",fileName = "TextSO")]
public class PhoneTextEvent : ScriptableObject
{
    [SerializeField]
    public string textSender;

    [TextArea(5, 20)]
    [SerializeField]
    private List<string> phoneTexts = new List<string>();

    [TextArea(5, 20)]
    [SerializeField]
    private List<string> phoneTextsResponses = new List<string>();
    public string TheText1 
    {
        get => phoneTexts[0];
    }
    
    public string TheText1Response
    {
        get => phoneTextsResponses[0];
    }

    
}
