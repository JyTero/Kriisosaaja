using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CheckWeatherWindowInteraction : BaseInteraction
{
    [SerializeField]
    private int checkLenght;
    [SerializeField] [TextArea(1, 4)]
    private string chekWeatherString;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    public override void OnInteractionBegin()
    {
        base.OnInteractionBegin();
        // Route to interaction spot, if applicaple 
        playerAtDestinationEC.OnEventRaised += AtDestination;

        interactionManager.SetDestination(thisItem.InteractionSpotTransform.position);

    }

    //Raised when player arrives to their current destination
    protected override void AtDestination()
    {
        base.AtDestination();
        //Stuff player does when arriving to the interaction spot

        DuringInteraction();

    }


    //The "Main" of the Interaction
    private void DuringInteraction()
    {
        GetShowWeather();
    }

    private void GetShowWeather()
    {
        GlobalValues.Weather curWeather = interactionManager.GetWorldWeather();

        var window = thisItem as WindowItem;

        interactionManager.ShowNoticationText(chekWeatherString + curWeather.ToString(), 0);

        EndInteraction();

    }

    protected override void EndInteraction()
    {
        base.EndInteraction();  
    }

}
