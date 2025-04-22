using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuStartButtonScript : MonoBehaviour
{
    public void MainMenuStartBtn()
    {
        GlobalValues.ClearGlobalValues();
        FindObjectOfType<PlayerResourceData>().ResetPlayerResourceData();
        SceneManager.LoadScene(0);
    }
}
