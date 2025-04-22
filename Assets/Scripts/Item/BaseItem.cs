using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static GlobalValues;

//Base Class for all in-game objects that the player is meant to interact with in Scene.
public class BaseItem : MonoBehaviour
{
    public string ItemName;
    public List<BaseInteraction> Interactions;
    public Transform InteractionSpotTransform;

    [HideInInspector]
    public ItemManager itemManager;
    [SerializeField]
    public VoidEventChannelSO TimeBeatEC;

    [SerializeField]
    private bool debug = false;
    private void OnEnable()
    {
        if (TimeBeatEC != null)
            TimeBeatEC.OnEventRaised += TimeBeat;
    }
    protected virtual void Awake()
    {
        itemManager = FindObjectOfType<ItemManager>();
        Interactions = GetComponents<BaseInteraction>().ToList<BaseInteraction>();
        Interactions.RemoveAll(interaction => !interaction.enabled);
        if (debug)
            Debug.Log(ItemName + ": " + Interactions.Count);

    }
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    protected virtual void TimeBeat()
    {

    }
    private void OnDisable()
    {
        if (TimeBeatEC != null)
            TimeBeatEC.OnEventRaised -= TimeBeat;
    }
}
