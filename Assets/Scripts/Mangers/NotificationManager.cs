using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public Image notifImage;
    
    [SerializeField]
    private List<TextMeshProUGUI> notificationTextSlots = new();


    public void NewNotification(string notification)
    {
        string notif = "* " + notification;
        if (notificationTextSlots[0].text == "")
        {
            notificationTextSlots[0].text = notif;
        }
        else 
        {
            notificationTextSlots[2].text = notificationTextSlots[1].text;
            notificationTextSlots[1].text = notificationTextSlots[0].text;
            notificationTextSlots[0].text = notif;
        }

      if(notifImage.gameObject.activeSelf == false)
        {
            notifImage.gameObject.SetActive(true);
        }

    }

}
