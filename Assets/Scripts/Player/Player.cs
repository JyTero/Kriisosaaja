using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public BaseInteraction CurrentInteraction;
    [HideInInspector]
    public Vector3 currentDestination;
    [HideInInspector]
    public bool IsSleeping = false;

    [Header("Destination Channels")]
    [SerializeField]
    private Vector3EventChannelSO playerDesinationRecieveEC;
    [SerializeField]
    private VoidEventChannelSO playerAtDestinationEC;
    [SerializeField]
    private VoidEventChannelSO playerAtDestinationSecondaryEC;

    [Header("Channels To Adjust Stats")]
    [SerializeField]
    private IntEventChannelSO playerMentalWellbeingAdjustEventChannel;
    [SerializeField]
    private IntEventChannelSO playerHungerAdjustEventChannel;
    [SerializeField]
    private IntEventChannelSO playerHydrationAdjustEventChannel;
    [SerializeField]
    private IntEventChannelSO playerBathroomAdjustEventChannel;
    [SerializeField]
    private IntEventChannelSO playerHealthAdjustEventChannel;
    [SerializeField]
    private IntEventChannelSO playerEnergyAdjustEventChannel;
    [SerializeField]
    private BoolEventChannelSO playerHasElectrcityAdjustEventChannel;
    [SerializeField]
    private BoolEventChannelSO playerHasWaterAdjustEventChannel;

    [Header("Channels To Read Stats")]
    [SerializeField]
    private IntEventChannelSO playerMentalWellbeingReadEventChannel;
    [SerializeField]
    private IntEventChannelSO playerHungerReadEventChannel;
    [SerializeField]
    private IntEventChannelSO playerHydrationReadEventChannel;
    [SerializeField]
    private IntEventChannelSO playerBathroomReadtEventChannel;
    [SerializeField]
    private IntEventChannelSO playerHealthReadEventChannel;
    [SerializeField]
    private IntEventChannelSO playerEnergyReadEventChannel;
    [SerializeField]
    private BoolEventChannelSO playerHasElectrcityReadEventChannel;
    [SerializeField]
    private BoolEventChannelSO playerHasWaterReadEventChannel;

    [Header("Other Need Channels")]
    [SerializeField]
    private VoidEventChannelSO powerTurnedOffEC;
    [SerializeField]
    private VoidEventChannelSO powerTurnedOnEC;
    [SerializeField]
    private VoidEventChannelSO waterTurnedOffEC;
    [SerializeField]
    private VoidEventChannelSO waterTurnedOnEC;
    [SerializeField]
    private IntEventChannelSO onMentalWellbeingChangeEC;
    [SerializeField]
    private IntEventChannelSO onHungerChangeEC;
    [SerializeField]
    private IntEventChannelSO onHydrationChangeEC;
    [SerializeField]
    private IntEventChannelSO onBathroomChangeEC;
    [SerializeField]
    private IntEventChannelSO onHealthChangeEC;
    [SerializeField]
    private IntEventChannelSO onEnergyChangeEC;
  
    [Header("Other Channels")]
    [SerializeField]
    private VoidEventChannelSO interactionEndedEC;
    [SerializeField]
    private BaseInteractionEventChannelSO getSelectedInteractionEC;
    [SerializeField]
    private BoolEventChannelSO onPlayerGoesAwayEC;
    [SerializeField]
    private Transform outOfSightTransform;
    
    [SerializeField]
    private float navDistanceThreshold = 0.75f;
    [SerializeField]
    private bool enablePlayerDebugLogs = false;

    private int currentMentalWellbeingNeed = 0; //Range 0-100
    private int currentHungerNeed = 0;          //Range 0-100
    private int currentHydrationNeed = 0;       //Range 0-100
    private int currentBathroomNeed = 0;        //Range 0-100
    private int currentHealthNeed = 0;          //Range 0-100
    private int currentEnergyNeed = 0;          //Range 0-100
    private bool hasElectricty = true;
    private bool hasWater = true;

    private InteractionManager interactionManager;
    private NavMeshAgent navAgent;

    private UI_Handler UI;
    private Vector3 originalPosition;

    private void OnEnable()
    {
        //Set Set Channels
        if (playerMentalWellbeingAdjustEventChannel != null)
            playerMentalWellbeingAdjustEventChannel.OnEventRaised += OnPlayerMentalWellbeingChange;
        if (playerHungerAdjustEventChannel != null)
            playerHungerAdjustEventChannel.OnEventRaised += OnPlayerHungerChange;
        if (playerHydrationAdjustEventChannel != null)
            playerHydrationAdjustEventChannel.OnEventRaised += OnPlayerHydrationChange;
        if (playerBathroomAdjustEventChannel != null)
            playerBathroomAdjustEventChannel.OnEventRaised += OnPlayerBathroomChange;
        if (playerHealthAdjustEventChannel != null)
            playerHealthAdjustEventChannel.OnEventRaised += OnPlayerHealthChange;
        if (playerEnergyAdjustEventChannel != null)
            playerEnergyAdjustEventChannel.OnEventRaised += OnPlayerEnergyChange;
        if (playerHasElectrcityAdjustEventChannel != null)
            playerHasElectrcityAdjustEventChannel.OnEventRaised += OnPlayerHasElectricityChange;
        if (playerHasWaterAdjustEventChannel != null)
            playerHasWaterAdjustEventChannel.OnEventRaised += OnPlayerHasWaterChange;

        //Set Read Channel
        if (playerMentalWellbeingReadEventChannel != null)
            playerMentalWellbeingReadEventChannel.OnEventWReturnRaised += GetMentalWellbeing;
        if (playerHungerReadEventChannel != null)
            playerHungerReadEventChannel.OnEventWReturnRaised += GetHunger;
        if (playerHydrationReadEventChannel != null)
            playerHydrationReadEventChannel.OnEventWReturnRaised += GetHydration;
        if (playerBathroomReadtEventChannel != null)
            playerBathroomReadtEventChannel.OnEventWReturnRaised += GetBathroom;
        if (playerHealthReadEventChannel != null)
            playerHealthReadEventChannel.OnEventWReturnRaised += GetHealth;
        if (playerEnergyReadEventChannel != null)
            playerEnergyReadEventChannel.OnEventWReturnRaised += GetEnergy;
        if (playerHasElectrcityReadEventChannel != null)
            playerHasElectrcityReadEventChannel.OnEventWReturnRaised += GetHasElectrcity;
        if (playerHasWaterReadEventChannel != null)
            playerHasWaterReadEventChannel.OnEventWReturnRaised += GetHasWater;

        //Other Channels
        if (playerAtDestinationEC != null)
            playerDesinationRecieveEC.OnEventRaised += OnDestinationRecieved;
        if (getSelectedInteractionEC != null)
            getSelectedInteractionEC.OnEventRaised += OnCurrentInteractionRecieved;
        if (onPlayerGoesAwayEC != null)
            onPlayerGoesAwayEC.OnEventRaised += OnPlayerGoesAway;


        //Components
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent == null)
            Debug.LogWarning($"{name} has no NavMeshAgent!");
    }


    //All public needs: Can and probably should be refactored away due to
    //EC adjust methods exsiting and able to do the job
    public int playerMentallWellbeing
    {
        get { return currentMentalWellbeingNeed; }
        set
        {
            currentMentalWellbeingNeed = value;
            if (UI != null)
            {
                UI.playerMentalWellbeign.text = "Mental wellbeing: " + currentMentalWellbeingNeed;
            }
        }
    }
    public int playerHunger
    {
        get { return currentHungerNeed; }
        set
        {
            currentHungerNeed = value;
            if (UI != null)
            {
                UI.playerHunger.text = "Hunger: " + currentHungerNeed;
            }
        }
    }

    public int playerHydration
    {
        get { return currentHydrationNeed; }
        set
        {
            currentHydrationNeed = value;
            if (UI != null)
            {
                UI.playerHydration.text = "Hydration: " + currentHydrationNeed;
            }
        }
    }
    public int playerBathroom
    {
        get { return currentBathroomNeed; }
        set
        {
            currentBathroomNeed = value;
            if (UI != null)
            {
                UI.playerBathroom.text = "Bathroom: " + currentBathroomNeed;
            }
        }
    }
    public int playerHealth
    {
        get { return currentHealthNeed; }
        set
        {
            currentHealthNeed = value;
            if (UI != null)
            {
                UI.playerHealth.text = "Health: " + currentHealthNeed;
            }
        }
    }
    public int playerEnergy
    {
        get { return currentEnergyNeed; }
        set
        {
            currentEnergyNeed = value;
            if (UI != null)
            {
                UI.playerEnergy.text = "Energy: " + currentEnergyNeed;
            }
        }
    }
    public bool playerHasElectrcity
    {
        get { return hasElectricty; }
        set
        {
            hasElectricty = value;
            if (UI != null)
            {
                UI.playerElectricity.text = "Has power: " + hasElectricty;
            }
        }
    }
    public bool playerHasWater
    {
        get { return hasWater; }
        set
        {
            hasWater = value;
            if (UI != null)
            {
                UI.playerWater.text = "Has water: " + hasWater;
            }
        }
    }

    private void Start()
    {
        UI = FindObjectOfType<UI_Handler>();
        interactionManager = FindObjectOfType<InteractionManager>();
    }

    #region OnChangeMethods
    private void OnPlayerMentalWellbeingChange(int offset)
    {
        playerMentallWellbeing += offset;
        //clamp between 0-100
       playerMentallWellbeing = Mathf.Clamp(playerMentallWellbeing, 0, 100);

        onMentalWellbeingChangeEC.RaiseEvent(playerMentallWellbeing);
        if (enablePlayerDebugLogs)
            Debug.Log("Player Mental Wellbeing changed" + offset);
    }
    private void OnPlayerHungerChange(int offset)
    {
        playerHunger += offset;
        //clamp between 0-100
        playerHunger = Mathf.Clamp(playerHunger, 0, 100);

        onHungerChangeEC.RaiseEvent(playerHunger);

        if (enablePlayerDebugLogs)
            Debug.Log("Player Hunger changed" + offset);
    }
    private void OnPlayerHydrationChange(int offset)
    {
        playerHydration += offset;
        //clamp between 0-100
        playerHydration = Mathf.Clamp(playerHydration, 0, 100);
        onHydrationChangeEC.RaiseEvent(playerHydration);

        if (enablePlayerDebugLogs)
            Debug.Log("Player Hydration changed" + offset);
    }
    private void OnPlayerBathroomChange(int offset)
    {
        playerBathroom += offset;
        //clamp between 0-100
        playerBathroom = Mathf.Clamp(playerBathroom, 0, 100);
        onBathroomChangeEC.RaiseEvent(playerBathroom);

        if (enablePlayerDebugLogs)
            Debug.Log("Player Bathroom changed" + offset);
    }
    private void OnPlayerHealthChange(int offset)
    {
        playerHealth += offset;
        //clamp between 0-100
        playerHealth = Mathf.Clamp(playerHealth, 0, 100);
        onHealthChangeEC.RaiseEvent(playerHealth);

        if (enablePlayerDebugLogs)
            Debug.Log("Player Health changed" + offset);

    }
    private void OnPlayerEnergyChange(int offset)
    {
        playerEnergy += offset;
        //clamp between 0-100
        playerEnergy = Mathf.Clamp(playerEnergy, 0, 100);
        onEnergyChangeEC.RaiseEvent(playerEnergy);

        if (enablePlayerDebugLogs)
            Debug.Log("Player Energy changed" + offset);

    }

    private void OnPlayerHasElectricityChange(bool hasPower)
    {
        playerHasElectrcity = hasPower;
        if (!playerHasElectrcity)
        {
            powerTurnedOffEC.RaiseEvent();
        }
        else
            powerTurnedOnEC.RaiseEvent();
        if (enablePlayerDebugLogs)
        {
            Debug.Log("Player HasElectricity changed" + hasPower);
        }
    }
    private void OnPlayerHasWaterChange(bool hasWater)
    {
        playerHasWater = hasWater;
        if (!playerHasWater)
        {
            waterTurnedOffEC.RaiseEvent();
        }
        else
        {
            waterTurnedOnEC.RaiseEvent();
        }
        if (enablePlayerDebugLogs)
            Debug.Log("Player HasWater changed" + hasWater);

    }
    #endregion
    #region GetMethods 
    private int GetMentalWellbeing()
    {
        return playerMentallWellbeing;
    }
    private int GetHunger()
    {
        return playerHunger;
    }
    private int GetHydration()
    {
        return playerHydration;
    }
    private int GetBathroom()
    {
        return playerBathroom;
    }
    private int GetHealth()
    {
        return playerHealth;
    }
    private int GetEnergy()
    {
        return playerEnergy;
    }
    private bool GetHasElectrcity()
    {
        return playerHasElectrcity;
    }
    private bool GetHasWater()
    {
        return playerHasWater;
    }
    #endregion
    //Variables to recieve Destination and alert when at destination
    private void OnDestinationRecieved(Vector3 newDestination)
    {
        //If primary (takes to interaction begin) or secondary (movement within interaction)
        if (interactionManager.InteractionUIParentGO.activeSelf)
        {
            OnDestinationRecievedSecondary(newDestination);
            return;
        }

        interactionEndedEC.OnEventRaised += OnInteractionEnd;

        interactionManager.InteractionUIParentGO.SetActive(true);
        interactionManager.InteractionCancelButtonGO.SetActive(true);
        interactionManager.InteractionName.text = CurrentInteraction.InteractionName;

        currentDestination = newDestination;
        //Use Navmesh to traverse to given destination.
        navAgent.destination = newDestination;
        StartCoroutine(IsAtDestination());

    }

    //Used to check if at destination, could and should probably be finetuned
    private IEnumerator IsAtDestination()
    {
        yield return new WaitUntil(() => Vector3.Distance(transform.position, currentDestination) < navDistanceThreshold);
        //When at destination

        interactionManager.InteractionCancelButtonGO.SetActive(false);
        playerAtDestinationEC.RaiseEvent();
        yield return null;

    }

    //Called when the interaction requires player to disappear, such as going to toilet or to sleep
    private void OnPlayerGoesAway(bool isGoingAway)
    {
        if (isGoingAway)
        {
            originalPosition = navAgent.destination;
            navAgent.Warp(outOfSightTransform.position);
        }
        else
        {
            navAgent.Warp(originalPosition);
        }
    }

    private void OnDestinationRecievedSecondary(Vector3 newDestination)
    {

        currentDestination = newDestination;
        //Use Navmesh to traverse to given destination.
        navAgent.destination = newDestination;
        StartCoroutine(IsAtDestinationSecondary());

    }

    //Used to check if at destination, could and should probably be finetuned
    private IEnumerator IsAtDestinationSecondary()
    {
        yield return new WaitUntil(() => Vector3.Distance(transform.position, currentDestination) < navDistanceThreshold);
        
        playerAtDestinationSecondaryEC.RaiseEvent();
        yield return null;

    }

    private void OnInteractionEnd()
    {
        interactionEndedEC.OnEventRaised -= OnInteractionEnd;
        currentDestination = Vector3.zero;
        if (IsSleeping)
            IsSleeping = false;
        Debug.Log("interaction ended");
    }

    public void InteractionCancel()
    {
        navAgent.SetDestination(transform.position);
        interactionManager.EndInteraction();
    }

    private void OnCurrentInteractionRecieved(BaseInteraction bi)
    {
        CurrentInteraction = bi;
        var sleepInteraction = CurrentInteraction as Sleep_Interaction;
        if (sleepInteraction != null)
        {
            IsSleeping = true;
            Debug.Log("PlayerSleeps");
        }
    }

    private void OnDisable()
    {
        //Set channels
        playerMentalWellbeingAdjustEventChannel.OnEventRaised -= OnPlayerMentalWellbeingChange;
        playerHungerAdjustEventChannel.OnEventRaised -= OnPlayerHungerChange;
        playerHydrationAdjustEventChannel.OnEventRaised -= OnPlayerHydrationChange;
        playerBathroomAdjustEventChannel.OnEventRaised -= OnPlayerBathroomChange;
        playerHealthAdjustEventChannel.OnEventRaised -= OnPlayerHealthChange;
        playerEnergyAdjustEventChannel.OnEventRaised -= OnPlayerEnergyChange;
        playerHasElectrcityAdjustEventChannel.OnEventRaised -= OnPlayerHasElectricityChange;
        playerHasWaterAdjustEventChannel.OnEventRaised -= OnPlayerHasWaterChange;

        //Read Channels
        playerMentalWellbeingReadEventChannel.OnEventWReturnRaised -= GetMentalWellbeing;
        playerHungerReadEventChannel.OnEventWReturnRaised -= GetHunger;
        playerHydrationReadEventChannel.OnEventWReturnRaised -= GetHydration;
        playerBathroomReadtEventChannel.OnEventWReturnRaised -= GetBathroom;
        playerHealthReadEventChannel.OnEventWReturnRaised -= GetHealth;
        playerEnergyReadEventChannel.OnEventWReturnRaised -= GetEnergy;
        playerHasElectrcityReadEventChannel.OnEventWReturnRaised -= GetHasElectrcity;
        playerHasWaterReadEventChannel.OnEventWReturnRaised -= GetHasWater;

        //Other Channels
        playerDesinationRecieveEC.OnEventRaised -= OnDestinationRecieved;
    }
}
