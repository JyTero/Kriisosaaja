using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Phone : BaseInteraction
{
    [Header("Channels for text message events")]
    [SerializeField]
    private PhoneTextEvent MomPhoneEventChannel;
    [SerializeField]
    private PhoneTextEvent PowerOutageText;
    [SerializeField]
    private WeatherEventChannelSO onWeatherChangeEC;
    [SerializeField]
    private QualityEventChannelSO onAirQualityChangeEC;

    private TuneToWeatherRadioInteraction tuneToWeatherChannel;
    private TuneToNationalBroadcastingRadioInteraction tuneToNationalBroadcast;

    public HandleSnakePhone snakePhone;
    public GameObject snakeGameGO;
    public bool snakeOn = false;
    public List<Button> mainButtons = new List<Button>();
    //public List<Button> settingsButtons = new List<Button>();
    //public List<Button> weatherButtons = new List<Button>();
    public List<Sprite> backgroundImages = new List<Sprite>();
    public List<TextMeshProUGUI> bodyTextList = new List<TextMeshProUGUI>();

    //public Transform phoneScreenPos;
    //public Transform TargetPosUp;
    //public Transform TargetPosDown; 
    //public float liftSpeed = 1.0f; // 1500 is good on my pc (veetu)

    //public bool Up = false;


    public float Energy = 10;
    public PlayerResourceData resourceData;

    public Image phoneScreen;
    public Image phoneBlackScreen;
    public Image RedBar;
    public List<Image> BatteryStateImages;
    //public TextMeshProUGUI phoneHeaderNotiffText;
    public bool canLift = true;

    public Camera MainCamera;
    public Camera SnakeCamera;


    public bool AirplaneMode = false;
    public bool bluetooth = true;

    public AudioSource notificationSound;
    void Awake()
    {
        //Put phone to down pos at the beginning
        //phoneScreenPos.position = TargetPosDown.position;
        resourceData = FindObjectOfType<PlayerResourceData>();
    }

    private void OnEnable()
    {
        onWeatherChangeEC.OnEventRaised += RaiseWeatherAlert;
        onAirQualityChangeEC.OnEventRaised += RaiseBroadcastAlert;

    }

    private void OnDisable()
    {
        onWeatherChangeEC.OnEventRaised -= RaiseWeatherAlert;
        onAirQualityChangeEC.OnEventRaised -= RaiseBroadcastAlert;
    }
    protected override void Start()
    {
        base.Awake();
        tuneToWeatherChannel = GetComponent<TuneToWeatherRadioInteraction>();
        tuneToNationalBroadcast = GetComponent<TuneToNationalBroadcastingRadioInteraction>();
        //Get the scene index and send the apropriate goverment text.
        
        int sceneI = SceneManager.GetActiveScene().buildIndex;
        if (sceneI == 3 && resourceData.scenario3Start == false)
        {
            resourceData.scenario3Start = true;
            NotiffGov(PowerOutageText.TheText1);
        }
    }

    void Update()
    {
        //if (canLift)
        //{
        //    //Press P to bring phone up
        //    if (Input.GetKeyDown(KeyCode.P))
        //    {
        //        LiftPhone();
        //    }
        //    //Moves the phone up when bool up true (P pressed)
        //    if (Up == true)
        //    {
        //        var step = liftSpeed * Time.deltaTime;
        //        phoneScreenPos.position = Vector3.MoveTowards(phoneScreenPos.position, TargetPosUp.position, step);
        //        if (Vector3.Distance(phoneScreenPos.position, TargetPosUp.position) < 0.001f)
        //        {
        //            phoneScreenPos.position = TargetPosUp.position;
        //        }
        //    }
        //    //Brings the phone down when P pressed again
        //    else if (Up == false)
        //    {
        //        var step = liftSpeed * Time.deltaTime;
        //        phoneScreenPos.position = Vector3.MoveTowards(phoneScreenPos.position, TargetPosDown.position, step);
        //        if (Vector3.Distance(phoneScreenPos.position, TargetPosDown.position) < 0.001f)
        //        {
        //            phoneScreenPos.position = TargetPosDown.position;
        //        }
        //    }
        //    //Default down
        //    else phoneScreenPos.position = TargetPosDown.position;
        //    //blackscreen follows phone screen.
        //    phoneBlackScreen.transform.position = phoneScreen.transform.position;
        //}
    }
    
    //void LiftPhone()
    //{
    //    if (Up == false)
    //    {
    //        Up = true;
    //    }
    //    else Up = false;
    //}

    //Manages the images for phone battery bar, when energy lowers the more power bars the phone loses, last energy makes last bar red, after which the phone goes black.
    void ManageBatteryBar()
    {
        switch(Energy)
        {
            case 0:
                phoneBlackScreen.gameObject.SetActive(true);
                phoneScreen.gameObject.SetActive(false);
                break;

            case 1:
                RedBar.gameObject.SetActive(true);
                BatteryStateImages[0].gameObject.SetActive(false);
                break; 
                
            case 3:
                BatteryStateImages[1].gameObject.SetActive(false);
                break;

            case 5:
                BatteryStateImages[2].gameObject.SetActive(false);
                break;

            case 7:
                BatteryStateImages[3].gameObject.SetActive(false);
                break;

            case 10:
                //Everythings active
                break;
        }
    }

    //Text someone app
    public void MessageApp()
    {
        if(AirplaneMode == false)
        {
            mainButtons.ForEach(button => { button.gameObject.SetActive(false); });
            phoneScreen.sprite = backgroundImages[2];

            Debug.Log("Message");
            Energy--;
            ManageBatteryBar();
        }
    }

    //Weather app to check world stats
    public TextMeshProUGUI NotiffText;
    public TextMeshProUGUI WeatherText;
    
    public void WeatherApp()
    {
        if (AirplaneMode == false)
        {
            mainButtons.ForEach(button => { button.gameObject.SetActive(false); });

            phoneScreen.sprite = backgroundImages[1];

            WeatherText.gameObject.SetActive(true);
            WeatherText.text = "Current weather is " + interactionManager.GetWorldWeather().ToString() + " according to lates weather reports.\n" + "The airquality is " + interactionManager.GetWorldAirQ().ToString() + " according to Finnish Meteorological Institute.";

            
            Energy--;
            ManageBatteryBar();
        }
    }
    private void RaiseWeatherAlert(GlobalValues.Weather weather)
    {
        //notificationSound.Play();
        //phoneHeaderNotiffText.text = "Weather update!";
        StartCoroutine(HeaderCountdown());
    }
    private void RaiseBroadcastAlert(GlobalValues.Quality airQ)
    {
        //notificationSound.Play();
        //phoneHeaderNotiffText.text = "Air quality has changed!";
        StartCoroutine(HeaderCountdown());
    }
    IEnumerator HeaderCountdown()
    {
        float counter = 15;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1f);
            counter--;
        }
        //phoneHeaderNotiffText.text = "";
    }

    //Get gov texts here
    public void NotiffGov(string text)
    {
        if (AirplaneMode == false)
        {
            mainButtons.ForEach(button => { button.gameObject.SetActive(false); });
            NotiffText.gameObject.SetActive(true);
            phoneScreen.sprite = backgroundImages[2];
            //notificationSound.Play();
            NotiffText.text += text + "\n";
        }
    }



    public GameObject OneOneTwoObject;

    public void OneOneTwoApp()
    {
        mainButtons.ForEach(button => { button.gameObject.SetActive(false); });
        phoneScreen.sprite = backgroundImages[1];
        OneOneTwoObject.gameObject.SetActive(true);
    }

    //Game app
    public void SnakeGameApp()
    { 
        MainCamera.gameObject.SetActive(false);
        SnakeCamera.gameObject.SetActive(true);
        //LiftPhone();
        snakeGameGO.SetActive(true);
        //StartCoroutine(SnakeLiftCountdown(0.5f));
        Energy -= 2;
    }
    //Needed so there won't be 2 phones up at the same time
    //IEnumerator SnakeLiftCountdown(float seconds)
    //{
    //    float counter = seconds;
    //    while (counter > 0)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //        counter--;
    //    }
    //    canLift = false;
    //    //snakePhone.LiftPhone();
    //}

    //Brings up homescreen buttons and backgorund while disabling otherbuttons
    public void HomeButton()
    {
        phoneScreen.sprite = backgroundImages[0];
        //settingsButtons.ForEach(button => { button.gameObject.SetActive(false); });
        bodyTextList.ForEach(text => { text.gameObject.SetActive(false); });
        OneOneTwoObject.gameObject.SetActive(false);
        mainButtons.ForEach(button => { button.gameObject.SetActive(true); });
    }

    //Manage phone settings like airplane mode, bluetooth, gps

    //public Button Settings;
    //public void BringUpSettingsApp()
    //{
    //    phoneScreen.sprite = backgroundImages[1];
    //    mainButtons.ForEach(button => { button.gameObject.SetActive(false);} );
    //    settingsButtons.ForEach(button => { button.gameObject.SetActive(true); });
    //}

//    public Image BluetoothButton;
//    public void BluetoothToggle()
//    {
//        if (bluetooth == false)
//        {
//            bluetooth = true;
//            BluetoothButton.color = Color.blue;
//            Debug.Log("Bluetoth");
//        }
//        else
//        {
//            BluetoothButton.color = Color.gray;
//            bluetooth = false;
//            Debug.Log("Bluetoth else");
//        }
//    }

//    public Image AirplaneModeButton;
//    public void AirplaneToggle()
//    {
//        if (AirplaneMode == false)
//        {
//            AirplaneMode = true;
//            AirplaneModeButton.color = Color.blue;
//            Debug.Log("Airpln");
//        }
//        else
//        {
//            AirplaneModeButton.color = Color.gray;
//            AirplaneMode = false;
//            Debug.Log("Airpln else");
//        }
//    }
}