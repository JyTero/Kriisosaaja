using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//NOT REPRESENTATIVE OF ACTUAL PRACTICES IN USE
public class DemoUI : MonoBehaviour
{
    Player player;
    WorldManager worldManager;


    [SerializeField]
    public TextMeshProUGUI worldTemperature;

    public TextMeshProUGUI worldWeather;

    public TextMeshProUGUI worldHealth;
    public TextMeshProUGUI worldAirQ;
    public TextMeshProUGUI worldInfo;
    public TextMeshProUGUI worldCurfew;


    public TextMeshProUGUI playerMoney;
    public TextMeshProUGUI playerMentalWellbeing;

    public TextMeshProUGUI playerHunger;

    public TextMeshProUGUI playerHydration;

    public TextMeshProUGUI playerBathroom;

    public TextMeshProUGUI playerHealth;

    public TextMeshProUGUI playerEnergy;

    public TextMeshProUGUI playerEletricity;

    public TextMeshProUGUI playerWater;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
