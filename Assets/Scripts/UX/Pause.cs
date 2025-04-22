using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pause : MonoBehaviour
{
    public GameObject PauseMenu;
    public Button pauseButton;

   
    public void PauseButton()
    {
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void ContinueButton()
    {
        Time.timeScale = 1.0f;
        pauseButton.gameObject.SetActive(true);
        PauseMenu.SetActive(false);
        
    }

}