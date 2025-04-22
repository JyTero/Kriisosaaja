using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{

    private void Start()
    {
        
    }

    public void Init(bool isOffset)
    {
        this.GetComponent<SpriteRenderer>().color = isOffset? Color.white : Color.gray;
    }
}
