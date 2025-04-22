using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioSceneChangeviaDoor : MonoBehaviour
{
    private ScenarioSceneSaveManager scenarioSaveManager;
    private void Start()
    {
        scenarioSaveManager = FindObjectOfType<ScenarioSceneSaveManager>();
    }

    //0 is shop, 1 is this scene, 3 and 4 are something to be decided
    public void LoadScene0()
    {
        SaveScenario();
        SceneManager.LoadScene(0);
    }
    public void LoadScene1()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadScene3()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadScene4()
    {
        SceneManager.LoadScene(4);
    }

    //Save variety of scenario data on scene exit, for the duration of session
    public void SaveScenario()
    {
        scenarioSaveManager.SaveScenarioSceneSurfaceSlotItems();
        scenarioSaveManager.SavePlayerValues();
        scenarioSaveManager.SaveWorldValues();
        scenarioSaveManager.SaveScenarioData();
        scenarioSaveManager.SaveScenarioObjectsState();
        scenarioSaveManager.SaveScoringLists();
    }
}
