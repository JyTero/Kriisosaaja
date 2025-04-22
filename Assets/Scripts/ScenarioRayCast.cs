using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class ScenarioRayCast : MonoBehaviour
{
    [SerializeField]
    private int interactableLayerInt;
    private InteractionManager interactionManager;

    private Player player;
    private bool debug = false;

    // Start is called before the first frame update
    void Start()
    {
        interactionManager = FindObjectOfType<InteractionManager>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (player.currentDestination == Vector3.zero)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    //Ray is made on mouse pos
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        BaseItem item = hit.transform.gameObject.GetComponent<BaseItem>();

                        //If valid interactable object
                        if (hit.transform.gameObject.layer == interactableLayerInt)
                        {
                            //Disable old lingering listeners
                            //(made when clicking object and then straight another object before selecting interaction)
                            interactionManager.CancelListener();

                            if (debug)
                                Debug.Log("Hit something: " + item.ItemName + "(" + item.gameObject.name + ")");
                            interactionManager.GetAndDisplayInteractions(item.Interactions);
                        }
                        else
                        {
                            if (debug)
                                Debug.Log("Hit: " + hit.transform.gameObject.name);
                            interactionManager.interactionsDropDown.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        if (debug)
                            Debug.Log("NoHit");
                    }
                }
                else
                {
                    if (debug)
                        Debug.Log("OverUI");
                    PointerEventData pointerEventData;
                    GraphicRaycaster raycaster = FindObjectOfType<GraphicRaycaster>();
                    pointerEventData = new PointerEventData(EventSystem.current);
                    pointerEventData.position = Input.mousePosition;

                    List<RaycastResult> results = new List<RaycastResult>();
                    raycaster.Raycast(pointerEventData, results);
                    foreach (RaycastResult result in results)
                    {

                        if (debug)
                            Debug.Log("UI: " + result.gameObject.name);
                    }
                }

            }
            else
            {
                if (debug)
                    Debug.LogWarning("Cancel current interaction before attempting to choose another one!");
            }
        }
    }
}
