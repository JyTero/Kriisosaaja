using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTester : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannelSO voidEventChannel;
    [SerializeField]
    private IntEventChannelSO intEventChannel;

    private void OnEnable()
    {
        //WorldValues.OnIntChange += OnIntChange;
        voidEventChannel.OnEventRaised += OnVoidChange;
        intEventChannel.OnEventRaised += OnIntChange;
    }

    private void OnDisable()
    {
        //WorldValues.OnIntChange -= OnIntChange;
        voidEventChannel.OnEventRaised -= OnVoidChange;
        intEventChannel.OnEventRaised -= OnIntChange;
        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnIntChange(int newValue)
    {
        //if (newValue % 2 == 0)
        //{ 
        //    GetComponent<MeshRenderer>().material.color = Color.red; 
        //}

        //else if (newValue % 2 == 1)
        //{
        //    GetComponent<MeshRenderer>().material.color = Color.blue; 
        //}

       // Debug.Log("Event value !: " + WorldValues.worldTemp);
       Debug.Log("INTSO EVENT CHANNEL SLAY");
    }

    private void OnVoidChange()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();

        if(mr.material.color != Color.red)
        {
            mr.material.color = Color.red;
        }
        else
            mr.material.color = Color.blue;

       // Debug.Log("SO EVENT CHANNEL SLAY");
    }
}
