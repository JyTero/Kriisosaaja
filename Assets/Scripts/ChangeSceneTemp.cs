using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTemp : MonoBehaviour
{
    private SceneManager sceneManager;

    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
    public void ChangeSceneBack()
    {
        SceneManager.LoadScene(3);
    }
}
