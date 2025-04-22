using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UI_Handler : MonoBehaviour
{
    [Header("General Buttons")]
    public Button inventoryBtn;
    public Button playerNeedsBtn;
    public Button notificationBtn;
    public Button phoneBtn;

    [Header("General Panels")]
    public GameObject generalPanel; // Contains inventoryPanel, playerNeedsPanel, notificationPanel, phonePanel
    public GameObject inventoryPanel;
    public GameObject playerNeedsPanel;
    public GameObject notificationPanel;
    public GameObject phonePanel;

    [Header("Shop Buttons")]
    public Button shopYesBtn;
    public Button shopNoBtn;
    public Button shopUndoBtn;
    public Button shopHomeBtn;

    [Header("Shop Panels")]
    public GameObject shopUIPanel;
    public GameObject shopPurchaseConfirmationPanel;

    [Header("Player Needs")]
    public TextMeshProUGUI playerMoney;
    public TextMeshProUGUI playerHunger;
    public TextMeshProUGUI playerHydration;
    public TextMeshProUGUI playerBathroom;
    public TextMeshProUGUI playerEnergy;
    // Will show later in images
    public TextMeshProUGUI playerMentalWellbeign;
    public TextMeshProUGUI playerHealth;
    // DEBUGGING
    public TextMeshProUGUI playerElectricity;
    public TextMeshProUGUI playerWater;

    [Header("Pause Menu")]
    public Button pauseBtn;
    public Button resumeBtn;
    public Button backToMenuBtn;
    public GameObject pausePanel;
    public bool isPaused;

    // Only for debuging
    [Header("DEBUGGING || World Values")]
    public TextMeshProUGUI worldTemperature;
    public TextMeshProUGUI worldWeather;
    public TextMeshProUGUI worldAirQ;
    public TextMeshProUGUI worldInfo;
    public TextMeshProUGUI worldCurfew;
    public TextMeshProUGUI worldHealth;

    // Managers
    ShopManager shopManager;
    Player player;
    WorldManager worldManager;
    private NotificationManager notifManager;

    // Start is called before the first frame update
    void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();
        notifManager = FindObjectOfType<NotificationManager>();

        // General Buttons
        inventoryBtn.onClick.AddListener(OpenInventory);
        playerNeedsBtn.onClick.AddListener(OpenPlayerNeeds);
        notificationBtn.onClick.AddListener(OpenNotifications);
        phoneBtn.onClick.AddListener(OpenPhone);

        //Shop Buttons
        shopYesBtn.onClick.AddListener(AcceptPurchase);
        shopNoBtn.onClick.AddListener(DeclinePurchase);
        shopUndoBtn.onClick.AddListener(UndoPurchase);
        shopHomeBtn.onClick.AddListener(ReturnToYourHome);

        //Pause Button
        pauseBtn.onClick.AddListener(PauseGame);
        resumeBtn.onClick.AddListener(ResumeGame);
        backToMenuBtn.onClick.AddListener(BackToMenu);
    }

    #region General
    void OpenInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        playerNeedsPanel.SetActive(false);
        notificationPanel.SetActive(false);
        phonePanel.SetActive(false);
    }

    void OpenPlayerNeeds()
    {
        playerNeedsPanel.SetActive(!playerNeedsPanel.activeSelf);
        inventoryPanel.SetActive(false);
        notificationPanel.SetActive(false);
        phonePanel.SetActive(false);
    }

    void OpenNotifications()
    {
        notificationPanel.SetActive(!notificationPanel.activeSelf);
        playerNeedsPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        phonePanel.SetActive(false);

        //Enable notifincation alert img
        if (notifManager.notifImage.gameObject.activeSelf == true)
            notifManager.notifImage.gameObject.SetActive(false);
    }

    void OpenPhone()
    {
        phonePanel.SetActive(!phonePanel.activeSelf);
        playerNeedsPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        notificationPanel.SetActive(false);
    }

    #endregion

    #region Shop
    void AcceptPurchase()
    {
        shopManager.YesButton();
    }

    void DeclinePurchase()
    {
        shopManager.NoButton();
    }

    void UndoPurchase()
    {
        shopManager.UndoPurchase();
    }

    void ReturnToYourHome()
    {
        //SceneManager.LoadScene("HomeSceneWrking");
        FindObjectOfType<ShopStockSaver>().SaveAndChangeScene();
    }
    #endregion

    #region Pause Game
    void PauseGame()
    {
        isPaused = true;

        if(isPaused == true)
        {
            pauseBtn.gameObject.SetActive(false);
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void ResumeGame()
    {
        isPaused = false;

        if (isPaused == false)
        {
            pauseBtn.gameObject .SetActive(true);
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

   public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion
}