using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu_Handler : MonoBehaviour
{
    [Header("Buttons")]
    public Button playBtn;
    public Button quitBtn;

    // Start is called before the first frame update
    void Start()
    {
        playBtn.onClick.AddListener(Play);
        quitBtn.onClick.AddListener(QuitGame);
    }

    void Play()
    {
        SceneManager.LoadScene(1);
        GlobalValues.ClearGlobalValues();
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
