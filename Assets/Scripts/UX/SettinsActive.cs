using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettinsActive : MonoBehaviour
{
    public GameObject SettingsCanvas;

    public void Activity()
    {
        if(SettingsCanvas.activeInHierarchy == true)
        {
            SettingsCanvas.SetActive(false);
        }
        else if (SettingsCanvas.activeInHierarchy == false)
        {
            SettingsCanvas.SetActive(true);
        }
    }
}
