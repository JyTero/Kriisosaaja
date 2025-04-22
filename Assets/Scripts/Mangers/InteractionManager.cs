using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using static GlobalValues;

public class InteractionManager : MonoBehaviour
{
    [Header("Channels To Adjust Player Stats")]
    [SerializeField]
    private FloatEventChannelSO playerMoneyAdjustEventChannel;
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

    [Header("Channels To Adjust World Stats")]
    [SerializeField]
    private IntEventChannelSO worldTemperatureAdjustEventChannel;
    [SerializeField]
    private WeatherEventChannelSO worldWeatherAdjustEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldHealthQualityAdjustEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldInfoQualityAdjustEventChannel;
    [SerializeField]
    private BoolEventChannelSO worldIsAllowedOutAdjustEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldAirQualityAdjustEventChannel;

    [Header("Channels To Read Player Stats")]
    [SerializeField]
    private FloatEventChannelSO playerMoneyReadEventChannel;
    [SerializeField]
    private IntEventChannelSO playerMentalWellbeingReadEventChannel;
    [SerializeField]
    private IntEventChannelSO playerHungerReadEventChannel;
    [SerializeField]
    private IntEventChannelSO playerHydrationReadEventChannel;
    [SerializeField]
    private IntEventChannelSO playerBathroomReadEventChannel;
    [SerializeField]
    private IntEventChannelSO playerHealthReadEventChannel;
    [SerializeField]
    private IntEventChannelSO playerEnergyReadEventChannel;
    [SerializeField]
    private BoolEventChannelSO playerHasElectrcityReadEventChannel;
    [SerializeField]
    private BoolEventChannelSO playerHasWaterReadEventChannel;

    [Header("Channels To Read World Stats")]
    [SerializeField]
    private IntEventChannelSO worldTemperatureReadEventChannel;
    [SerializeField]
    private WeatherEventChannelSO worldWeatherReadEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldAirQualityReadEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldHealthReadEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldInfoQualityReadEventChannel;
    [SerializeField]
    private BoolEventChannelSO worldIsAllowedOutReadEventChannel;

    [Header("Other Channels")]
    [SerializeField]
    private IntEventChannelSO getTimeEC;
    [SerializeField]
    private Vector3EventChannelSO playerDestinationSetEC;
    [SerializeField]
    private VoidEventChannelSO playerEndInteractionEC;
    [SerializeField]
    private VoidEventChannelSO cancelLongInteractionEC;
    [SerializeField]
    private BaseInteractionEventChannelSO selectedInteractionEC;


    [Header("Interactions UI")]
    public GameObject InteractionUIParentGO;
    public GameObject InteractionCancelButtonGO;
    public GameObject LongInteractionStopButtonGO;
    public TMP_Text InteractionName;

    [Header("The Rest")]
    public TMP_Dropdown interactionsDropDown;
    public PlayerResourceData PlayerResourceData;
    public TimeManager TimeManager;
    [SerializeField]
    private TextMeshProUGUI BasicResources;

    [HideInInspector]
    public Fridge SceneFridge;
    [HideInInspector]
    public KitchenCounter SceneKitchenCounter;
    [HideInInspector]
    public bool ScenarioInitialised = false;

    private BaseInteraction[] itemInteractions;
    private ItemManager itemManager;

    private void Awake()
    {
        InteractionUIParentGO.SetActive(false);
        InteractionCancelButtonGO.SetActive(false);
        itemManager = FindObjectOfType<ItemManager>();
        PlayerResourceData = FindObjectOfType<PlayerResourceData>();
    }
    private void Start()
    {
        SceneFridge = FindObjectOfType<Fridge>();
        SceneKitchenCounter = FindObjectOfType<KitchenCounter>();
        TimeManager = FindObjectOfType<TimeManager>();

        UpdateResourcesToList();
    }

    public int GetManagerTime()
    {
        return getTimeEC.OnEventWReturnRaised();

    }
    //Called on interactable item click,
    //interactions list comes form baseItem of the clicked item
    public void GetAndDisplayInteractions(List<BaseInteraction> interactions)
    {
        var interactionMenu = new List<TMP_Dropdown.OptionData>();
        //Add first item to guide player to pick interaction, doesn't do anything
        interactionMenu.Add(new TMP_Dropdown.OptionData("Pick interaction"));
        //Add interactions from 
        for (int i = 0; i < interactions.Count; i++)
        {
            interactionMenu.Add(new TMP_Dropdown.OptionData(interactions[i].InteractionName));
        }
        interactionsDropDown.options = interactionMenu;
        itemInteractions = interactions.ToArray();

        interactionsDropDown.value = 0;
        interactionsDropDown.gameObject.SetActive(true);

        //Enable listener to get run selected interaction.
        interactionsDropDown.onValueChanged.AddListener(OnInteractionSelected);
    }

    void OnInteractionSelected(int index)
    {
        interactionsDropDown.gameObject.SetActive(false);

        interactionsDropDown.onValueChanged.RemoveListener(OnInteractionSelected);
        //-1 to index due to the added Pick Interaction
        selectedInteractionEC.OnEventRaised(itemInteractions[index - 1]);
        itemInteractions[index - 1].OnInteractionBegin();
    }

    public void UpdateResourcesToList()
    {
        BasicResources.text = PlayerResourceData.MoneyAmount.ToString() + "€\n" +
            "Meat: " + PlayerResourceData.MeatFoodAmount.ToString() + "\n" +
            "Vegetables: " + PlayerResourceData.VegetableFoodAmount.ToString() + "\n" +
            "Pasta: " + PlayerResourceData.PastaFoodAmount.ToString() + "\n" +
            "Canned Foods: " + PlayerResourceData.CannedFoodAmount.ToString() + "\n" +
            "Chips: " + PlayerResourceData.ChipsFoodAmount.ToString() + "\n" +
            "Milk: " + PlayerResourceData.MilkDrinkAmount.ToString() + "\n" +
            "Water: " + PlayerResourceData.WaterDrinkAmount.ToString() + "\n" +
            "Juice: " + PlayerResourceData.JuiceDrinkAmount.ToString();
    }

    public void LongInteractionBegin()
    {
        LongInteractionStopButtonGO.SetActive(true);
    }
    public void CancelListener()
    {
        interactionsDropDown.onValueChanged.RemoveAllListeners();
    }

    //Sends destination for player to be set as the current destination
    public void SetDestination(Vector3 newDestination)
    {
        playerDestinationSetEC.OnEventRaised(newDestination);
        LongInteractionStopButtonGO.SetActive(false);


    }
    public void EndInteraction()
    {

        InteractionUIParentGO.SetActive(false);
        playerEndInteractionEC.RaiseEvent();
    }

    public void CancelLongInteraction()
    {
        cancelLongInteractionEC.RaiseEvent();
    }

    //If duration is 0, use default display time
    public void ShowNoticationText(string notification, int duration)
    {
        itemManager.ShowNoticationText(notification, duration);
    }
    #region ChannelMethods
    //Player Adjust
    public void AdjustPlayerMoney(float offset)
    {
        playerMoneyAdjustEventChannel.OnEventRaised(offset);
    }
    public void AdjustPlayerMentalWellbeing(int offest)
    {
        playerMentalWellbeingAdjustEventChannel.OnEventRaised(offest);
    }
    public void AdjustPlayerHunger(int offset)
    {
        playerHungerAdjustEventChannel.OnEventRaised(offset);
    }
    public void AdjustPlayerHydration(int offest)
    {
        playerHydrationAdjustEventChannel.OnEventRaised(offest);
    }
    public void AdjustPlayerBathroom(int offest)
    {
        playerBathroomAdjustEventChannel.OnEventRaised(offest);
    }
    public void AdjustPlayerHealth(int offest)
    {
        playerHealthAdjustEventChannel.OnEventRaised(offest);
    }
    public void AdjustPlayerEnergy(int offest)
    {
        playerEnergyAdjustEventChannel.OnEventRaised(offest);
    }
    public void AdjustPlayerHasElectricity(bool hasElectricity)
    {
        playerHasElectrcityAdjustEventChannel.OnEventRaised(hasElectricity);
    }
    public void AdjustPlayerHasWater(bool hasWater)
    {
        playerHasWaterAdjustEventChannel.OnEventRaised(hasWater);
    }
    //World Adjust
    public void AdjustWorldTemperature(int offest)
    {
        worldTemperatureAdjustEventChannel.OnEventRaised(offest);
    }
    public void AdjustWorldWeather(Weather newWeather)
    {
        worldWeatherAdjustEventChannel.OnEventRaised(newWeather);
    }
    public void AdjustWorldHealthQ(Quality newHealthQ)
    {
        worldHealthQualityAdjustEventChannel.OnEventRaised(newHealthQ);
    }
    public void AdjustWorldAirQ(Quality newAirQ)
    {
        worldAirQualityAdjustEventChannel.OnEventRaised(newAirQ);
    }
    public void AdjustWorldInfoQ(Quality newInfoQ)
    {
        worldInfoQualityAdjustEventChannel.OnEventRaised(newInfoQ);
    }
    public void AdjustWorldIsAllowedOut(bool isAllowedOut)
    {
        worldIsAllowedOutAdjustEventChannel.OnEventRaised(isAllowedOut);
    }
    //Player Get
    public float GetPlayerMoney()
    {
        return playerMoneyReadEventChannel.OnEventWReturnRaised();
    }
    public int GetPlayerMentalWellbeing()
    {
        return playerMentalWellbeingReadEventChannel.OnEventWReturnRaised();
    }
    public int GetPlayerHunger()
    {
        return playerHungerReadEventChannel.OnEventWReturnRaised();
    }
    public int GetPlayerHydration()
    {
        return playerHydrationReadEventChannel.OnEventWReturnRaised();
    }
    public int GetPlayerBathroom()
    {
        return playerBathroomReadEventChannel.OnEventWReturnRaised();
    }
    public int GetPlayerHealth()
    {
        return playerHealthReadEventChannel.OnEventWReturnRaised();
    }
    public int GetPlayerEnergy()
    {
        return playerEnergyReadEventChannel.OnEventWReturnRaised();
    }
    public bool GetPlayerHasElectricity()
    {
        return playerHasElectrcityReadEventChannel.OnEventWReturnRaised();
    }
    public bool GetPlayerHasWater()
    {
        return playerHasWaterReadEventChannel.OnEventWReturnRaised();
    }

    //World Get
    public int GetWorldTemperature()
    {
        return worldTemperatureReadEventChannel.OnEventWReturnRaised();
    }
    public Weather GetWorldWeather()
    {
        return worldWeatherReadEventChannel.OnEventWReturnRaised();
    }
    public Quality GetWorldAirQ()
    {
        return worldAirQualityReadEventChannel.OnEventWReturnRaised();
    }
    public Quality GetWorldHealthQ()
    {
        return worldHealthReadEventChannel.OnEventWReturnRaised();
    }
    public Quality GetWorldInfoQ()
    {
        return worldInfoQualityReadEventChannel.OnEventWReturnRaised();
    }
    public bool GetWorldIsAllowedOut()
    {
        return worldIsAllowedOutReadEventChannel.OnEventWReturnRaised();
    }
    #endregion
}


