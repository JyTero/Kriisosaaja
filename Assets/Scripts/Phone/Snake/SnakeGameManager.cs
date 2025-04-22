using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SnakeGameManager : MonoBehaviour
{
    
    public GridManager gridManager;
    public GameObject grid;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public GameObject snakeHead;
    //List of the tiles making up the grid
    public List<GameObject> tiles = new List<GameObject>();
    public int score = 0;
    private SnakeFood snakeFood;
    public Button startButton;
    public TMP_Dropdown difficultyDropDown;
    [System.NonSerialized]
    public int difficulty;

    //public HandleSnakePhone snakePhoneHandler;
    public Phone phone;

    public Camera MainCamera;
    public Camera SnakeCamera;
    private void Start()
    {
        //Genereate grid
        gridManager.generateGrid();

        //add all tiles to the list
        tiles.AddRange(GameObject.FindGameObjectsWithTag("Tile"));
        snakeFood = FindFirstObjectByType<SnakeFood>();
        difficulty = difficultyDropDown.value;
        //snakePhoneHandler.setScreenPos();
    }
    public void Difficulty()
    {
        difficulty = difficultyDropDown.value;
    }

    //on gameover this happens
    public void GameOver()
    {
        //Set gameover text active
        gameOverText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        scoreText.text = score.ToString();
        Debug.Log("Game over!");
        startButton.gameObject.SetActive(true);
        difficultyDropDown.gameObject.SetActive(true);
    }

    public void SpawnSnake()
    {
        //instantiate snake at the spot
        
        //choose random spot on the grid
        float x = UnityEngine.Random.Range(0, 14);
        float y = UnityEngine.Random.Range(0, 9);

        foreach (GameObject tile in tiles)
        {
            if (tile.name == $"Tile {x} {y}")
            {
                //instantiate food at the spot
                Instantiate(snakeHead, tile.transform.position, Quaternion.identity);
                snakeHead.transform.SetParent(grid.transform);
                StartCoroutine(Countdown(10));
                break;
            }
            else Debug.Log("Error, snakehead tile or GO not found");
        }
    }

    public void StartGame()
    {
        gameOverText.text = "Game Over!" + "<br> Your score was: ";
        scoreText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        difficultyDropDown.gameObject.SetActive(false);
        score = 0;
        SpawnFood();
        SpawnSnake();       
    }

    //gameobject of the snake food object
    public GameObject snakePellet;
    public void SpawnFood()
    {
        //Transform snakeFoodSpot;
        //choose random spot on the grid
        float x = UnityEngine.Random.Range(0, 14);
        float y = UnityEngine.Random.Range(0, 9);

        foreach(GameObject tile in tiles)
        {
            if (tile.name == $"Tile {x} {y}")
            {
                //instantiate food at the spot
                Instantiate(snakePellet, tile.transform.position, Quaternion.identity);
                break;
            }
            else Debug.Log("Error, snakefood tile not found");
        }

        
    }

    public void EndGame()
    {

        MainCamera.gameObject.SetActive(false);
        SnakeCamera.gameObject.SetActive(true);
        //snakePhoneHandler.LiftPhone();
        //StartCoroutine(Countdown(0.5f));
    }

    IEnumerator Countdown(float seconds)
    {
        float counter = seconds;
        while (counter > 0)
        {
            yield return new WaitForSeconds(0.5f);
            counter--;
        }
        phone.canLift = true;
    }
}