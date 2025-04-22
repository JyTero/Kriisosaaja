using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


//Snake game for phone app
public class SnakeMovement : MonoBehaviour
{
    //needed for movement direction
    private Vector3 gridMoveDirection = Vector3.up;

    //timer till movement
    private float gridMoveTimer;

    //when timer hits this point movement happens
    [System.NonSerialized]
    public float gridMoveTimerMax = 0.2f;
   
    //Body segment prefab
    public GameObject bodyPrefab;
    //list of all snakes current body segments
    private List<GameObject> segments = new List<GameObject>();
    private SnakeFood snakefood;
    private SnakeGameManager gameManager;
    private void Start()
    {
        segments.Add(FindFirstObjectByType<SnakeMovement>().gameObject);
        snakefood = FindFirstObjectByType<SnakeFood>();
        gameManager = FindFirstObjectByType<SnakeGameManager>();
        SetSpeed();
    }

    private void Update()
    {
        gridMoveTimer += Time.deltaTime;
        Debug.Log(gridMoveTimerMax.ToString() + " dif: " + gameManager.difficulty);
        //turn snake head depending on input
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (gridMoveDirection != Vector3.down)
            {
                gridMoveDirection = Vector3.up;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (gridMoveDirection != Vector3.up)
            {
                gridMoveDirection = Vector3.down;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (gridMoveDirection != Vector3.right)
            {
                gridMoveDirection = Vector3.left;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (gridMoveDirection != Vector3.left)
            {
                gridMoveDirection = Vector3.right;
            }
        }

        if(gridMoveTimer > gridMoveTimerMax)
        {
            for(int i = segments.Count - 1; i > 0; i--)
            {
                segments[i].transform.position = segments[i - 1].transform.position;
                segments[i].transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);
            }
            transform.position = new Vector3(Mathf.Round(transform.position.x) + gridMoveDirection.x,
                Mathf.Round(transform.position.y) + gridMoveDirection.y, 0.0f);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);
            gridMoveTimer = 0f;
        }
    }
    
    private float GetAngleFromVector(Vector3 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public void SetSpeed()
    {
        if (gameManager.difficulty == 0)
        {
            gridMoveTimerMax = 0.25f;
        }
        else if (gameManager.difficulty == 1)
        {
            gridMoveTimerMax = 0.2f;
        }
        else if (gameManager.difficulty == 2)
        {
            gridMoveTimerMax = 0.1f;
        }
    }

    private void Grow()
    {
        GameObject newSegment = Instantiate(bodyPrefab);
        
        if(segments.Count > 1)
        {
            newSegment.transform.position = new Vector3(segments[segments.Count - 1].transform.position.x, segments[segments.Count - 1].transform.position.y);
        }
        else if (segments.Count == 1)
        {
            newSegment.transform.position = new Vector3(segments[segments.Count - 1].transform.position.x - 1, segments[segments.Count - 1].transform.position.y - 1);
        }
        segments.Add(newSegment);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "SnakePellet")
        {
            Destroy(collision.gameObject);
            Grow();
            gameManager.score++;
            gameManager.SpawnFood();
        }

        else if (collision.tag == "Wall")
        {
            Debug.Log(collision.gameObject.name);
            EndState();
            FindObjectOfType<SnakeGameManager>().GameOver();
        }
    }
   
    private void EndState()
    {
        
        for(int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }
        var snakepelletDestroyable = GameObject.Find("SnakePellet(Clone)");
        segments.Clear();
        segments.Add(this.gameObject);
        gridMoveTimerMax = 1000000000f;
        Destroy(snakepelletDestroyable.gameObject);
        Destroy(this.gameObject);
    }
}
