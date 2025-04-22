using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ItemMenus : MonoBehaviour
{

    // list of the menus that pops up when you click an item, mostly used to hide them when needed.
    public List<GameObject> menus = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void InteractKeititin(UnityEngine.UI.Button button)
    {
        if (button.transform.parent.parent.name == "Retkikeititin")
        {
            //retikikeitin funk
        }
        else return;
    }

    public void InteractVaraVirta(UnityEngine.UI.Button button)
    {
        if (button.transform.parent.parent.name == "Radio")
        {
            //varavirta funk
        }
        else return;
    }

    public void InteractPuhelin(UnityEngine.UI.Button button)
    {
        if (button.transform.parent.parent.name == "Radio")
        {
            //puhelin funk 
        }
        else return;
    }
}
