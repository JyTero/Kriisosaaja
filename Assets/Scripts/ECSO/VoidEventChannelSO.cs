using System.Data.Common;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/VoidEventChannel", fileName = "VoidEventChannel")]

public class VoidEventChannelSO : DescriptionSO
{
    [Tooltip("The action to perform")]
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        if (OnEventRaised == null)
        {
            Debug.LogWarning("Event channel error, OnEventRaised is null for channel with parameter value");
            return;
        }
        OnEventRaised.Invoke();
    }
}
