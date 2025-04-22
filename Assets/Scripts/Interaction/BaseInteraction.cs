using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValues;

public class BaseInteraction : MonoBehaviour
{
    public string InteractionName;
    [HideInInspector]
    public bool InteractionInitialised = false;

    [SerializeField]
    protected VoidEventChannelSO playerAtDestinationEC;

    protected InteractionManager interactionManager;

    protected BaseItem thisItem;

    protected string interactionOutcome;

    protected virtual void Awake()
    {
        interactionManager = FindObjectOfType<InteractionManager>();
        if (interactionManager == null)
            Debug.LogWarning("No interaction manager found!");

        thisItem = GetComponent<BaseItem>();
        if (thisItem == null)
            Debug.LogWarning("No this item found!");
    }

    protected virtual void Start()
    {


    }

    public virtual void OnInteractionBegin()
    {

    }



    protected virtual void AtDestination()
    {
        Debug.Log("Player arrived to object!");
        playerAtDestinationEC.OnEventRaised -= AtDestination;

    }
    protected virtual void EndInteraction()
    {
        interactionManager.EndInteraction();   
    }
}
