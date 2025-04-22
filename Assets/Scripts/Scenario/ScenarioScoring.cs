using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ScenarioScoring : MonoBehaviour
{
    public int ScenarioScore = 0;
    [HideInInspector]
    public List<string> scoringStrings = new();
    [HideInInspector]
    public List<int> scoringScores = new();

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
    [SerializeField]
    private VoidEventChannelSO onPortableStoveCookingEC;
    [SerializeField]
    private TMP_Text gameEndDescriptionText;
    [SerializeField]
    private int veryLowNeedLimit = 0;

    [SerializeField]
    private int properWindowPreparationScore = 0;
    [SerializeField]
    private int inproperWindowPreparationScore = 0;
    [SerializeField]
    [Tooltip("Time lethal air is allowed to exist without score penalty.")]
    private int lethalAirGracePeriod = 0;


    [SerializeField]
    [TextArea(1, 3)]
    private string veryLowNeedIntro = "";
    [SerializeField]
    private string lowNeedDescription = "";
    [SerializeField]
    private string acceptableNeedDescription = "";

    [SerializeField]
    [TextArea(1, 3)]
    private string cumulativeScoreDescriptionIntro = "";
    [SerializeField]
    private BoolEventChannelSO onWindowSealEC;
    [SerializeField]
    private QualityEventChannelSO onAirQChangeEC;
    [SerializeField]
    [TextArea(1, 3)]
    private string properWindowPrepDescription = "";
    [SerializeField]
    [TextArea(1, 3)]
    private string inproperWindowPrepDescription = "";
    [SerializeField]
    [TextArea(1, 3)]
    private string lackOfWindowPrepDescription = "";

    [SerializeField]
    private VoidEventChannelSO timeBeatEC;
    [SerializeField]
    private BoolEventChannelSO onUnpoweredBathroomUse;
    [SerializeField]
    private BoolEventChannelSO onDrytoiletPrep;

    [SerializeField]
    [TextArea(1, 3)]
    private string firstUnpoweredBathroomUseDescription = "";
    [SerializeField]
    [TextArea(1, 3)]
    private string laterUnpoweredBathroomUseDescription = "";
    [SerializeField]
    [TextArea(1, 3)]
    private string firstFullUnpoweredBathroomUseDescription = "";
    [SerializeField]
    [TextArea(1, 3)]
    private string laterFullUnpoweredBathroomUseDescription = "";
    [SerializeField]
    private int unpoweredBathroomUseScorePenalty = 0;
    [SerializeField]
    private int fullUnpoweredBahtroomScorePenalty = 0;

    [SerializeField]
    [TextArea(1, 3)]
    private string properDryToiletPrepDescription;
    [SerializeField]
    [TextArea(1, 3)]
    private string inproperDryToiletPrepDescription;
    [SerializeField]
    private int properDryToiletPrepScore = 0;
    [SerializeField]
    private int inproperDryToiletPrepScore = 0;
    [SerializeField]
    private string firstPortableStoveMealDescription = "";
    [SerializeField]
    private string generalPortableStoveMealDescription = "";
    [SerializeField]
    private int portableStoveMealScore = 10;
    [SerializeField]
    private VoidEventChannelSO onDangerousFoodEatenEC;
    [SerializeField]
    [TextArea(1, 3)]
    private string dangerousFoodEatenDescription;
    [SerializeField]
    private int dangerousFoodEatenScore;


    private bool firstUnpoweredBathroomUse = true;
    private bool firstUnpoweredBathroomFull = true;
    private List<int> lowestNeedValues = new();
    private int lowestMentalWellbeing = 100;
    private int lowestHunger = 100;
    private int lowestHydration = 100;
    private int lowestBathroom = 100;
    private int lowestHealth = 100;
    private int lowestEnergy = 100;
    private Dictionary<int, int> lowNeedTracking = new();

    private bool veryLowNeedsBool = false;
    private int lethalAirInRoomTime = 0;
    private bool lethalAirInRoom = false;

    private InteractionManager interactionManager;
    private ItemManager itemManager;
    private bool lethalAirGraceEnded = false;
    private bool firstPortableStoveMeal = true;


    private void OnEnable()
    {
        onMentalWellbeingChangeEC.OnEventRaised += OnMentalWellbeingChange;
        onHungerChangeEC.OnEventRaised += OnHungerChange;
        onHydrationChangeEC.OnEventRaised += OnHydrationChange;
        onBathroomChangeEC.OnEventRaised += OnBathroomChange;
        onHealthChangeEC.OnEventRaised += OnHealthChange;
        onEnergyChangeEC.OnEventRaised += OnEnergyChange;

        timeBeatEC.OnEventRaised += TimeBeat;
        onWindowSealEC.OnEventRaised += OnWindowSeal;
        onAirQChangeEC.OnEventRaised += OnAirQChange;
        onUnpoweredBathroomUse.OnEventRaised += OnUnpoweredBathroomUse;
        onDrytoiletPrep.OnEventRaised += OnDryToiletPrep;
        onPortableStoveCookingEC.OnEventRaised += OnPortableStoveCookingScoring;
        onDangerousFoodEatenEC.OnEventRaised += OnDangerousFoodEatenScoring;
    }

    private void Start()
    {
        interactionManager = FindAnyObjectByType<InteractionManager>();
        itemManager = FindObjectOfType<ItemManager>();

        scoringStrings = new();
        scoringStrings.Add(cumulativeScoreDescriptionIntro);
        scoringScores = new();
        scoringScores.Add(-1);

    }
    #region onNeedChangeMethods
    private void OnMentalWellbeingChange(int currentValue)
    {
        //save lowest rating for the course of game
        if (currentValue < lowestMentalWellbeing)
            lowestMentalWellbeing = currentValue;
    }
    private void OnHungerChange(int currentValue)
    {
        //save lowest rating for the course of game
        if (currentValue < lowestHunger)
            lowestHunger = currentValue;
    }
    private void OnHydrationChange(int currentValue)
    {
        //save lowest rating for the course of game
        if (currentValue < lowestHydration)
            lowestHydration = currentValue;
    }

    private void OnBathroomChange(int currentValue)
    {
        //save lowest rating for the course of game
        if (currentValue < lowestBathroom)
            lowestBathroom = currentValue;
    }

    private void OnHealthChange(int currentValue)
    {
        //save lowest rating for the course of game
        if (currentValue < lowestHealth)
            lowestHealth = currentValue;
    }

    private void OnEnergyChange(int currentValue)
    {
        //save lowest rating for the course of game
        if (currentValue < lowestEnergy)
            lowestEnergy = currentValue;
    }
    #endregion

    //Window and Air Quality Scoring
    private void OnAirQChange(GlobalValues.Quality airQuality)
    {
        if (airQuality == GlobalValues.Quality.Dangerous)
        {
            if (!itemManager.WindowIsSealed)
                lethalAirInRoom = true;
        }
        else
        {
            lethalAirInRoom = false;
            lethalAirGraceEnded = false;
        }
    }

    private void OnWindowSeal(bool isSealed)
    {
        if (isSealed)
        {
            //if window seal was done with lethal air, give score 
            if (interactionManager.GetWorldAirQ() == GlobalValues.Quality.Dangerous)
            {
                //accumulativeScore += properPreparationScore;
                lethalAirInRoom = false;
                lethalAirGraceEnded = false;

                scoringStrings.Add(properWindowPrepDescription);
                scoringScores.Add(properWindowPreparationScore);
            }
            //else prep was done with non lethal air, penalty
            else
            {
                scoringStrings.Add(inproperWindowPrepDescription);
                scoringScores.Add(inproperWindowPreparationScore);
            }
        }
        else
        {
            if (interactionManager.GetWorldAirQ() == GlobalValues.Quality.Dangerous)
            {
                lethalAirInRoom = true;
            }
            else
            {
            }
        }
    }

    private void OnDryToiletPrep(bool isPreped)
    {
        //If done when outtages
        if (interactionManager.GetPlayerHasElectricity() == false ||
            interactionManager.GetPlayerHasWater() == false)
        {
            scoringStrings.Add(properDryToiletPrepDescription);
            scoringScores.Add(properDryToiletPrepScore);
        }
        else
        {
            scoringStrings.Add(inproperDryToiletPrepDescription);
            scoringScores.Add(inproperDryToiletPrepScore);
        }
    }
    //Give score penalty for using "normal" bathroom when no power or water.
    private void OnUnpoweredBathroomUse(bool unpoweredCapasityFull)
    {
        scoringScores.Add(unpoweredBathroomUseScorePenalty);
        if (firstUnpoweredBathroomUse)
        {
            scoringStrings.Add(firstUnpoweredBathroomUseDescription);
            firstUnpoweredBathroomUse = false;
        }
        else
            scoringStrings.Add(laterUnpoweredBathroomUseDescription);

        if (unpoweredCapasityFull)
        {
            if (firstUnpoweredBathroomFull)
            {
                scoringStrings.Add(firstFullUnpoweredBathroomUseDescription);
                firstUnpoweredBathroomFull = false;
            }
            else
            {
                scoringStrings.Add(laterFullUnpoweredBathroomUseDescription);
            }
            scoringScores.Add(fullUnpoweredBahtroomScorePenalty);
        }
    }

    private void OnPortableStoveCookingScoring()
    {
        //Give points for portable cooking only if power is cut
        if (!interactionManager.GetPlayerHasElectricity())
        {
            if (firstPortableStoveMeal)
            {
                firstPortableStoveMeal = false;
                scoringStrings.Add(firstPortableStoveMealDescription);
            }
            else
                scoringStrings.Add(generalPortableStoveMealDescription);
            scoringScores.Add(portableStoveMealScore);
        }
    }

    private void OnDangerousFoodEatenScoring()
    {
        scoringStrings.Add(dangerousFoodEatenDescription);
        scoringScores.Add(dangerousFoodEatenScore);
    }

    private void TimeBeat()
    {
        if (lethalAirInRoom)
        {
            lethalAirInRoomTime++;

            //Bad air has been in room long enough to give penalty
            if (lethalAirInRoomTime >= lethalAirGracePeriod)
            {
                if (!lethalAirGraceEnded)
                {
                    Debug.Log("Lethal dose of Lethal air");

                    scoringStrings.Add(lackOfWindowPrepDescription);
                    scoringScores.Add(inproperWindowPreparationScore);
                    lethalAirGraceEnded = true;
                }
            }
        }
    }

    public string DisplayEndScore()
    {
        gameEndDescriptionText.gameObject.SetActive(true);

        AddNeedsToList();
        //Lowest need scoring
        AddLowestNeedScoresToOutput();

        int finalScore = 0;
        string finalText = "";
        //Iterate through string list, keep index to track score list at the same time
        int i = 0;
        foreach (string s in scoringStrings)
        {
            //If -1, the entry is "header" and doesn't have scoring 
            if (scoringScores[i] == -1)
            {
                finalText += s + "\n";
            }
            else
            {
                finalText += $"{s} ({scoringScores[i]}pts)\n";
                finalScore += scoringScores[i];
            }

            i++;
        }

        finalText += "\n" + "Final Score: " + finalScore.ToString();

        ScenarioScore = finalScore;

        return finalText;
    }

    private void AddLowestNeedScoresToOutput()
    {
        int lowestNeedValuesIndex = 0;

        //check if any of the needs is very low, add proper "header" to text

        foreach (int needValue in lowestNeedValues)
        {
            if (needValue <= veryLowNeedLimit)
            {
                //If very low needs,add prelude and then a line mentioning each low need.
                if (!veryLowNeedsBool)
                {

                    scoringStrings.Add(veryLowNeedIntro);
                    scoringScores.Add(-1);

                    veryLowNeedsBool = true;
                    break;
                }
            }
        }

        foreach (int needValue in lowestNeedValues)
        {
            string lowNeedString = "";
            //If need is seen to be very low, differetn text is displayed (TODO: adjust scoring for very low needs)
            if (needValue <= veryLowNeedLimit)
            {
                switch (lowestNeedValuesIndex)
                {
                    case 0:
                        lowNeedString = $"The mental wellbeing {lowNeedDescription}";
                        break;
                    case 1:
                        lowNeedString = $"The hunger {lowNeedDescription} ";
                        break;
                    case 2:
                        lowNeedString = $"The hydration {lowNeedDescription}";
                        break;
                    case 3:
                        lowNeedString = $"The bathroom {lowNeedDescription}";
                        break;
                    case 4:
                        lowNeedString = $"The health {lowNeedDescription}";
                        break;
                    case 5:
                        lowNeedString = $"The energy {lowNeedDescription}";
                        break;

                    default:
                        Debug.LogWarning("Invalid lowestNeedValuesIndex!");
                        break;
                }

                scoringStrings.Add(lowNeedString);
                scoringScores.Add(needValue);
            }
            else
            {
                switch (lowestNeedValuesIndex)
                {
                    case 0:
                        lowNeedString = $"The mental wellbeing {acceptableNeedDescription}";
                        break;
                    case 1:
                        lowNeedString = $"The hunger {acceptableNeedDescription}";
                        break;
                    case 2:
                        lowNeedString = $"The hydration {acceptableNeedDescription}";
                        break;
                    case 3:
                        lowNeedString = $"The bathroom {acceptableNeedDescription}";
                        break;
                    case 4:
                        lowNeedString = $"The health {acceptableNeedDescription}";
                        break;
                    case 5:
                        lowNeedString = $"The energy {acceptableNeedDescription}";
                        break;

                    default:
                        Debug.LogWarning("Invalid lowestNeedValuesIndex!");
                        break;
                }

                scoringStrings.Add(lowNeedString);
                scoringScores.Add(needValue);
            }

            lowestNeedValuesIndex++;
        }
    }

    private void AddNeedsToList()
    {
        lowestNeedValues.Add(lowestMentalWellbeing);
        lowestNeedValues.Add(lowestHunger);
        lowestNeedValues.Add(lowestHydration);
        lowestNeedValues.Add(lowestBathroom);
        lowestNeedValues.Add(lowestHealth);
        lowestNeedValues.Add(lowestEnergy);
    }

    private void OnDisable()
    {
        onMentalWellbeingChangeEC.OnEventRaised -= OnMentalWellbeingChange;
        onHungerChangeEC.OnEventRaised -= OnHungerChange;
        onHydrationChangeEC.OnEventRaised -= OnHydrationChange;
        onBathroomChangeEC.OnEventRaised -= OnBathroomChange;
        onHealthChangeEC.OnEventRaised -= OnHealthChange;
        onEnergyChangeEC.OnEventRaised -= OnEnergyChange;

        timeBeatEC.OnEventRaised -= TimeBeat;
        onWindowSealEC.OnEventRaised -= OnWindowSeal;
        onAirQChangeEC.OnEventRaised -= OnAirQChange;
        onUnpoweredBathroomUse.OnEventRaised -= OnUnpoweredBathroomUse;
        onDrytoiletPrep.OnEventRaised -= OnDryToiletPrep;
        onPortableStoveCookingEC.OnEventRaised -= OnPortableStoveCookingScoring;
        onDangerousFoodEatenEC.OnEventRaised -= OnDangerousFoodEatenScoring;
    }
}
