using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    //PlayerResourceData resourceData = new PlayerResourceData();
    //string saveFilePath;

    //void Awake()
    //{
    //    resourceData = FindObjectOfType<PlayerResourceData>();
    //    saveFilePath = Application.persistentDataPath + "/PlayerData.json";
    //}

    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.S))
    //    {
    //        Debug.Log("Saved Game");
    //        SaveGame();
    //    }
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        Debug.Log("Load Game");
    //        LoadGame();
    //    }

    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        Debug.Log("Delete Save");
    //        DeleteSave();
    //    }
    //}
    //public void SaveGame()
    //{
    //    //Commented out to clear errors for merge push, needs to be adjusted
    //    // resourceData.SceneIndex = SceneManager.GetActiveScene().buildIndex;
    //    Debug.Log(SceneManager.GetActiveScene().buildIndex);
    //    string savePlayerData = JsonUtility.ToJson(resourceData);
    //    File.WriteAllText(saveFilePath, savePlayerData);
    //    Debug.Log("Save file created at: " + saveFilePath);
    //    Debug.Log(savePlayerData);
    //}

    //public void LoadGame()
    //{
    //    if (File.Exists(saveFilePath))
    //    {
    //        string loadPlayerData = File.ReadAllText(saveFilePath);
    //        JsonUtility.FromJsonOverwrite(loadPlayerData, resourceData);
    //        Debug.Log(resourceData);
    //    }
    //    else Debug.Log("There is no save files to load!");


    //    //reloads saved scene
    //    // SceneManager.LoadScene(resourceData.SceneIndex);
    //}

    //public void DeleteSave()
    //{
    //    if (File.Exists(saveFilePath))
    //    {
    //        Debug.Log("Deleted file at: " + saveFilePath);
    //        File.Delete(saveFilePath);
    //    }
    //    else Debug.Log("There is nothing to delete!");
    //}
}
