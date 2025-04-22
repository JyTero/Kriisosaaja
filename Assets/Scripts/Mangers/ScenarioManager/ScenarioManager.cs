using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GlobalValues;

public class ScenarioManager : MonoBehaviour
{
    [HideInInspector]
    public int scenarioSituationIndex;
    //Tracks time for situation duration
    [HideInInspector]
    public int SituationTimer = 0;

    [Header("Situation")]
    [SerializeField]
    private ScenarioSO scenario;

    [Header("Channels To Adjust World Stats")]
    [SerializeField]
    private VoidEventChannelSO worldVoidEventChannel;
    [SerializeField]
    private IntEventChannelSO worldTemperatureAdjustEventChannel;
    [SerializeField]
    private WeatherEventChannelSO worldWeatherAdjustEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldHealthAdjustEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldInfoQualityAdjustEventChannel;
    [SerializeField]
    private BoolEventChannelSO worldIsAllowedOutAdjustEventChannel;
    [SerializeField]
    private QualityEventChannelSO worldAirQualityAdjustEventChannel;

    [Header("Channels To Adjust Player Stats")]
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
    private BoolEventChannelSO playerHasElectricityAdjustEventChannel;
    [SerializeField]
    private BoolEventChannelSO playerHasWaterAdjustEventChannel;

    [Header("Other Channels")]
    [SerializeField]
    private VoidEventChannelSO scenarioWonEC;
    [SerializeField]
    private StringEventChannelSO scenarioLostStringEC;
    [SerializeField]
    private VoidEventChannelSO timeBeatEC;
    [SerializeField]
    private IntEventChannelSO getTime;

    [Header("Non channels")]
    [SerializeField]
    private Canvas scenarioCanvas;
    [SerializeField]
    private TMP_Text gameLostText;
    [SerializeField]
    [TextArea(1, 4)]
    private string gameLostTextString;
    [SerializeField]
    private Canvas gameEndScreenCanvas;
    [SerializeField]
    private TMP_Text gameEndDescriptionText;
    [SerializeField]
    private bool debug = false;

    private int timeUntillNextSituation;
    private GameObject gameWonScreenParentGO;
    private GameObject gameLostScreenParentGO;
    private InteractionManager interactionManager;
    private PlayerResourceData playerResourceData;
    private ScenarioSceneSaveManager sceneSaveManager;
    private ScenarioScoring scenarioScoring;

    private GameObject radioGO;
    private GameObject PortableStoveGO;

    private void Awake()
    {

        playerResourceData = FindObjectOfType<PlayerResourceData>();

        sceneSaveManager = FindObjectOfType<ScenarioSceneSaveManager>();
        interactionManager = FindObjectOfType<InteractionManager>();
        scenarioScoring = FindObjectOfType<ScenarioScoring>();

        gameWonScreenParentGO = gameEndScreenCanvas.transform.GetChild(0).gameObject;
        gameLostScreenParentGO = gameEndScreenCanvas.transform.GetChild(1).gameObject;

        FindObjectOfType<TimeManager>().ScenarioEndTime = scenario.ScenarioEndTime;

        scenarioWonEC.OnEventRaised += ScenarioWon;
        scenarioLostStringEC.OnEventRaised += ScenarioLost;
        timeBeatEC.OnEventRaised += TimeBeat;


    }


    private void Start()
    {
        EnableBoolItems();
        if (scenario != null)
        {
            //If this is the first time entering scenario scene
            if (GlobalValues.SceneTime == -1)
            {
                StartCoroutine(BeginScenarioSituationCoro());

            }
            else
            {
                LoadScenario();
            }
        }
        else
            Debug.LogError("No Scenario loaded in scenario manager");
    }

    //If items have been bought, they're enabled
    private void EnableBoolItems()
    {
        radioGO = FindObjectOfType<Radio>().gameObject;
        PortableStoveGO = FindObjectOfType<PortableStove>().gameObject;


        if (playerResourceData.GotRadio)
        {
            radioGO.SetActive(true);
        }
        else
        {
            radioGO.SetActive(false);
        }

        if (playerResourceData.GotPortableStove)
        {
            PortableStoveGO.SetActive(true);
        }
        else
        {
            PortableStoveGO.SetActive(false);
        }

    }

    IEnumerator BeginScenarioSituationCoro()
    {
        yield return new WaitForSeconds(0.25f);

        //Save current scenario to GlobalValues for scenarioShop access
        CurrentScenario = scenario;

        //Displaye scenario Description
        interactionManager.ShowNoticationText(scenario.scenarioDescription, 0);
        RunSituation();

        //Let interactions know scenario has been set up
        interactionManager.ScenarioInitialised = true;
    }

    private void LoadScenario()
    {
        sceneSaveManager.LoadScenarioData();
        sceneSaveManager.LoadPlayerValues();
        sceneSaveManager.LoadWorldValues();
        sceneSaveManager.LoadScenarioSceneSurfaceSlotItems();
        sceneSaveManager.LoadScenarioObjectsState();
        sceneSaveManager.LoadScoringLists();
        interactionManager.ScenarioInitialised = true;

        if (debug)
            Debug.Log("SitIndex: " + ScenarioSituationIndex + "\n SitTimer: " + SituationTimer);

        //Get previous situation for situation duration
        if (ScenarioSituationIndex - 1 <= 0)
            timeUntillNextSituation = scenario.scenarioSituations[0].SituationDuration;
        else if (ScenarioSituationIndex - 1 <= scenario.scenarioSituations.Count - 1)
            timeUntillNextSituation = scenario.scenarioSituations[scenarioSituationIndex - 1].SituationDuration;
    }

    private void TimeBeat()
    {
        SituationTimer++;
        if (SituationTimer > timeUntillNextSituation)
        {
            RunSituation();
        }
    }

    private void RunSituation()
    {
        SituationTimer = 0;
        //IF the situation to be run is the last one
        if (scenarioSituationIndex <= scenario.scenarioSituations.Count - 1)
        {
            if (scenario.scenarioSituations[scenarioSituationIndex].situationDescription != "")
                interactionManager.ShowNoticationText(scenario.scenarioSituations[scenarioSituationIndex].situationDescription, 0);

            //Run each SitEffect on Situation
            foreach (BaseSituationEffectSO situationEffectSO in scenario.scenarioSituations[scenarioSituationIndex].situationEffects)
            {
                RunSituationEffect(situationEffectSO);
            }

            timeUntillNextSituation = scenario.scenarioSituations[scenarioSituationIndex].SituationDuration;
            scenarioSituationIndex++;
        }
        else
        {
            if (debug)
                Debug.Log("End of Situations!");
            //All Situations are used, unsubscribe from timeBeat since that's the only task it does in this script
            timeBeatEC.OnEventRaised -= TimeBeat;
        }

    }

    private void RunSituationEffect(BaseSituationEffectSO situationEffectSO)
    {
        switch (situationEffectSO)
        {
            //WorldSituationEffects
            case WorldTemperatureSituationEffectSO tempEffectSO:
                if (debug)
                    Debug.Log("TemperatureEffect");
                AdjustTemperature(tempEffectSO.temperatureOffset);
                break;
            case WorldWeatherSituationEffectSO weatherEffectSO:
                if (debug) 
                    Debug.Log("WeatherEffect");
                AdjustWeather(weatherEffectSO.newWeather);
                break;
            case WorldAirQualitySituationEffectSO airQEffectSO:
                if (debug)
                    Debug.Log("AirQSituationEffect");
                    AdjustAirQuality(airQEffectSO.newAirQuality);
                break;
            case WorldHealthSituationEffectSO healthQEffectSO:
                if (debug)
                    Debug.Log("HealthQSituationEffect");
                AdjustWorldHealth(healthQEffectSO.newHealth);
                break;
            case WorldInfoQualitySituationEffectSO infoQEffectSO:
                if (debug)
                    Debug.Log("InfoQualitySituationEffect");
                AdjustInfoQuality(infoQEffectSO.newInfoQuality);
                break;
            case WorldIsAllowedOutSituationEffectSO isAllowedOutEffectSO:
                if (debug)
                    Debug.Log("IsAllowedOutSituationEffect");
                AdjustIsAllowedOut(isAllowedOutEffectSO.isAllowedOut);
                break;
            case PlayerMentalWellbeingSituationEffectSO playerWellbeingEffectSO:
                if (debug)
                    Debug.Log("PlayerMentalWellbeingEffect");
                AdjustPlayerMentalWellbeing(playerWellbeingEffectSO.mentalWellbeingOffset);
                break;
            case PlayerHungerSituationEffectSO playerHungerEffectSO:
                if (debug)
                    Debug.Log("PlayerHungerEffect");
                AdjustPlayerHunger(playerHungerEffectSO.hungerOffset);
                break;
            case PlayerHydrationSituationEffectSO playerHydrationEffectSO:
                if (debug)
                    Debug.Log("PlayerHydrationEffect");
                AdjustPlayerHydration(playerHydrationEffectSO.hydrationOffset);
                break;
            case PlayerBathroomSituationEffectSO playerBathroomEffectSO:
                if (debug)
                    Debug.Log("PlayerBathroomEffect");
                AdjustPlayerBathroom(playerBathroomEffectSO.bathroomOffset);
                break;
            case PlayerHealthSituationEffectSO playerHealthEffectSO:
                if (debug)
                    Debug.Log("PlayerHealthEffect");
                AdjustPlayerHealth(playerHealthEffectSO.healthOffset);
                break;
            case PlayerEnergySituationEffectSO playerEnergyEffectSO:
                if (debug)
                    Debug.Log("PlayerEnergyEffect");
                    AdjustPlayerEnergy(playerEnergyEffectSO.energyOffset);
                break;
            case PlayerHasPowerSituationEffectSO playerHasPowerEffectSO:
                if (debug)
                    Debug.Log("PlayerHasPowerEffect");
                AdjustPlayerHasPower(playerHasPowerEffectSO.hasPower);
                break;
            case PlayerHasWaterSituationEffectSO playerHasWaterEffectSO:
                if (debug)
                    Debug.Log("PlayerHasWaterEffect");
                AdjustPlayerHasWater(playerHasWaterEffectSO.hasWater);
                break;
            default:
                Debug.LogWarning($"Unknown effect type in situation: {scenario.scenarioSituations[0].name}, effect name {situationEffectSO.name}");
                break;
        }
    }

    public void ScenarioWon()
    {

        ScenarioOver("You completed the scenario!");
    }

    public void ScenarioLost(string reason)
    {

        string s = gameLostTextString + reason;
        ScenarioOver(s);

    }

    private void ScenarioOver(string scenarioLostReason)
    {
        FindObjectOfType<TimeManager>().gameRuns = false;
        //Disable raycasting
        FindObjectOfType<ScenarioRayCast>().gameObject.SetActive(false);
        //Hide scnarioCanvas
        scenarioCanvas.gameObject.SetActive(false);


        gameLostScreenParentGO.SetActive(true);

        gameEndDescriptionText.text = scenarioScoring.DisplayEndScore();
        gameLostText.text = scenarioLostReason;
    }


    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    #region adjust ECs
    //World Value Adjusters
    private void AdjustTemperature(int newTemp)
    {
        worldTemperatureAdjustEventChannel.OnEventRaised(newTemp);
    }
    private void AdjustWeather(Weather newWeather)
    {
        worldWeatherAdjustEventChannel.OnEventRaised(newWeather);
    }
    private void AdjustAirQuality(Quality newQuality)
    {
        worldAirQualityAdjustEventChannel.OnEventRaised(newQuality);
    }
    private void AdjustWorldHealth(Quality newHealth)
    {
        worldHealthAdjustEventChannel.OnEventRaised(newHealth);
    }
    private void AdjustInfoQuality(Quality newInfoQuality)
    {
        worldInfoQualityAdjustEventChannel.OnEventRaised(newInfoQuality);
    }
    private void AdjustIsAllowedOut(bool newIsAllowedOut)
    {
        worldIsAllowedOutAdjustEventChannel.OnEventRaised(newIsAllowedOut);
    }
    private void AdjustPlayerMentalWellbeing(int offset)
    {
        playerMentalWellbeingAdjustEventChannel.OnEventRaised(offset);
    }
    private void AdjustPlayerHunger(int offset)
    {
        playerHungerAdjustEventChannel.OnEventRaised(offset);
    }
    private void AdjustPlayerHydration(int offset)
    {
        playerHydrationAdjustEventChannel.OnEventRaised(offset);
    }
    private void AdjustPlayerBathroom(int offset)
    {
        playerBathroomAdjustEventChannel.OnEventRaised(offset);
    }
    private void AdjustPlayerHealth(int offset)
    {
        playerHealthAdjustEventChannel.OnEventRaised(offset);
    }
    private void AdjustPlayerEnergy(int offset)
    {
        playerEnergyAdjustEventChannel.OnEventRaised(offset);
    }
    private void AdjustPlayerHasPower(bool newHasPower)
    {
        playerHasElectricityAdjustEventChannel.OnEventRaised(newHasPower);
    }
    private void AdjustPlayerHasWater(bool newHasWater)
    {
        playerHasWaterAdjustEventChannel.OnEventRaised(newHasWater);
    }
    #endregion

    private void OnDisable()
    {
        scenarioWonEC.OnEventRaised -= ScenarioWon;
        scenarioLostStringEC.OnEventRaised -= ScenarioLost;
        timeBeatEC.OnEventRaised -= TimeBeat;
    }
}
