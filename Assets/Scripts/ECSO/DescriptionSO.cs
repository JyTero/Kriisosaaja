using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionSO : ScriptableObject
{
        [TextArea(5, 20)]
        [SerializeField] protected string m_Description;
 
}
