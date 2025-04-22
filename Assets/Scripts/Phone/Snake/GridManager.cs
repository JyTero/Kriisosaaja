using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    //size of grid
    [SerializeField] private int width, height;
    public GameObject TilesHolder;

    public void generateGrid()
    {
        //instantiate grid and name tiles, make offset tiles different color
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(Resources.Load("Tile") as GameObject, TilesHolder.transform.position + new Vector3(x + 1f, y + 0.6f, 0), Quaternion.identity);

                spawnedTile.transform.localScale = Vector3.one;
                spawnedTile.GetComponent<Renderer>().enabled = true;

                spawnedTile.transform.SetParent(TilesHolder.transform);

                //spawnedTile.transform.RotateAround(TilesHolder.transform.position, Vector3.right, 9f);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.GetComponent<Tile>().Init(isOffset);
            }
        }
        TilesHolder.transform.position += new Vector3(-0.55f, -0.3f, 0.25f);
    }
}
