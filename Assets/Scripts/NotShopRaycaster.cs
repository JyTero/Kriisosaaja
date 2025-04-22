using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioRaycaster : MonoBehaviour
{

    public ItemMenus ItemMenus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Ray is made on mouse pos
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.tag == "Interactable") 
                {
                    Debug.Log("Ray hit: " + hit.collider.name);


                    //if clicked interactable objects menu is hidden, make it active
                    if (hit.transform.GetChild(0).gameObject.activeSelf == false)
                    {
                        //clear and hide previous clicks menu
                        for (int i = 0; i < ItemMenus.menus.Count; i++)
                        {
                            ItemMenus.menus[i].transform.GetChild(0).gameObject.SetActive(false);
                        }
                        ItemMenus.menus.Clear();

                        hit.transform.GetChild(0).gameObject.SetActive(true);
                        //Add menu to list
                        ItemMenus.menus.Add(hit.collider.gameObject);
                    }

                    //if the menu is already active, hide it
                    else if (hit.transform.GetChild(0).gameObject.activeSelf == true)
                    {
                        //Debug.Log("else");
                        hit.transform.GetChild(0).gameObject.SetActive(false);
                        //clear and hide menu
                        for (int i = 0; i < ItemMenus.menus.Count; i++)
                        {
                            ItemMenus.menus[i].transform.GetChild(0).gameObject.SetActive(false);
                        }
                        ItemMenus.menus.Clear();
                    }

                }
            }
            else
            {
                //if ray missed this happens
                Debug.Log("Ry missed");
            }
        }
    }
}
