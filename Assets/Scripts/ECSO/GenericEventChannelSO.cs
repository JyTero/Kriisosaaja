using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//REFACTOR !!

//[CreateAssetMenu(menuName = "EventChannels/GenericEventChannel", fileName = "GenericEventChannel")]
public class GenericEventChannelSO<T> : DescriptionSO
{
    [Tooltip("The action to perform; Listeners subscribe to this UnityAction")]
    public UnityAction<T> OnEventRaised;

   // public Func<T, T> OnEventRaisedFunc;

    public Func<T> OnEventWReturnRaised;

    public void RaiseEvent(T parameter)
    {

        if (OnEventRaised == null)
        {
            Debug.LogWarning("Event channel error, OnEventRaised is null for channel with parameter value" + parameter.ToString());
            return;
        }

        OnEventRaised.Invoke(parameter);

    }

    //public T RaiseEventWReturn()
    //{
    //    if (OnEventRaised == null)
    //    {
    //        Debug.LogWarning("Event channel error, OnEventRaised is null for channel with parameter value");
    //        return default;
    //    }

    //    return OnEventRaisedT.Invoke();
    //}
}
