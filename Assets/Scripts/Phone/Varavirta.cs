using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Varavirta : MonoBehaviour
{
    //reference to the resource holder
    public PlayerResourceData ResourceData;

    private void Start()
    {
        //finds the resources
        ResourceData = FindAnyObjectByType<PlayerResourceData>();
    }

    //Why is phone chanrge stored in ResourceData???
    public void RechargePhone(UnityEngine.UI.Button button)
    {
        //so long as phone isn't maxed out continue
        for(float i = ResourceData.PhoneEnergy; i < 10; i++)
        {
            //Again if phone has space for energy, and varavirta has energy left to give, continue
            if (ResourceData.PhoneEnergy < 10 && ResourceData.VaraVirtaEnergy! >= 0)
            {
                ResourceData.VaraVirtaEnergy--;
                ResourceData.PhoneEnergy++;
            }
            Debug.Log("PhoneCharge: " + ResourceData.PhoneEnergy);
        }
        
        button.transform.parent.gameObject.SetActive(false);
    }

}
