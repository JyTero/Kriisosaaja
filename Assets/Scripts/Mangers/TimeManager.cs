using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


public class TimeManager : MonoBehaviour
{
    public int NightBegin = 19;
    public int NightEnd = 7;
    [HideInInspector]
    public int CurrentTime = -1;

    [HideInInspector]
    public int CurrentHour = 0;
    [HideInInspector]
    public int ScenarioEndTime;
    [HideInInspector]
    public bool PlayerAwayFastForward = false;
    [HideInInspector]
    public bool gameRuns = true;
    [HideInInspector]
    public bool IsNight = false;

    [SerializeField]
    private int tickPerInGameHour = 12;
    [SerializeField]
    private VoidEventChannelSO timeBeatEC;
    [SerializeField]
    private IntEventChannelSO timeReadEC;
    [SerializeField]
    private VoidEventChannelSO scenarioTimeWon;
    [SerializeField]
    private BoolEventChannelSO timeOfDayChangeEC;
    [SerializeField]
    private TextMeshProUGUI displayeTicks;
    [SerializeField]
    private TextMeshProUGUI displayTime;

    private int currentMinute = 00;
    private int currentHour;


    private void OnEnable()
    {
        if (timeReadEC != null)
        {
            timeReadEC.OnEventWReturnRaised += GetTime;
        }
        if (GlobalValues.SceneTime != -1)
        {
            CurrentTime = GlobalValues.SceneTime;
            Debug.Log("Saved Time Loaded");
        }
    }
    private void OnDisable()
    {
        timeReadEC.OnEventWReturnRaised -= GetTime;
        GlobalValues.SceneTime = CurrentTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        //If first time in scenario scene
        if (CurrentTime == -1)
        {
            //Makes the game start at 8am
            CurrentTime = 96;
        }

        currentHour = (CurrentTime / tickPerInGameHour) % 24;
        displayTime.text = currentHour.ToString() + ":00";
        StartCoroutine(MoveTime());
    }

    private IEnumerator MoveTime()
    {
        //Give systems time to run their start before ticking time on, gummy solution.
        yield return new WaitForSeconds(0.25f);
        while (gameRuns)
        {
            CurrentTime++;
            timeBeatEC.RaiseEvent();
            displayeTicks.text = CurrentTime.ToString();

            DisplayTime();

            // Check if it's night
            if (currentHour >= 19 || currentHour < 7)
            {
                if (!IsNight)
                    TimeOfDayChange();

            }
            else
            {
                if (IsNight)
                    TimeOfDayChange();
            }

            //Scenario end
            if (CurrentTime >= ScenarioEndTime)
            {
                scenarioTimeWon.RaiseEvent();
                gameRuns = false;
                break;
            }

            yield return new WaitForSeconds(1);
        }
    }

    private void TimeOfDayChange()
    {
        IsNight = !IsNight;
        //Low key debug, simply swithces the first, currently only, ligth in scene
        var light = FindObjectOfType<Light>();
        light.enabled = !light.enabled;
    }

    public void SetTimeSpeed(int speed)
    {
        Time.timeScale = speed;
    }

    private int GetTime()
    {
        return CurrentTime;
    }

    private void DisplayTime()
    {
        //Time Displaying
        currentHour = (CurrentTime / tickPerInGameHour) % 24;
        if (CurrentTime % tickPerInGameHour % 2 == 1)
        {
        }
        else
            currentMinute = 10 * (CurrentTime % tickPerInGameHour / 2);
        if (currentMinute == 0)
        {
            displayTime.text = currentHour.ToString() + ":00";
        }
        else
        {
            displayTime.text = currentHour.ToString() + ":" + currentMinute.ToString();
        }
    }
}
