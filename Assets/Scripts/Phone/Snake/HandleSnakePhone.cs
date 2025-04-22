using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleSnakePhone : MonoBehaviour
{

    public Transform snakeScreenPos;
    public Transform TargetPosUp;
    public Transform TargetPosDown;

    public bool Up = false;

    public float liftSpeed = 1500f; // 1500 is good

    //public void setScreenPos()
    //{
    //    snakeScreenPos.position = TargetPosDown.position;
    //}
    void Update()
    {
        ////Moves the phone up when bool up true (P pressed)
        //if (Up == true)
        //{
        //    var step = liftSpeed * Time.deltaTime;
        //    snakeScreenPos.position = Vector3.MoveTowards(snakeScreenPos.position, TargetPosUp.position, step);
        //    if (Vector3.Distance(snakeScreenPos.position, TargetPosUp.position) < 0.001f)
        //    {
        //        snakeScreenPos.position = TargetPosUp.position;
        //    }
        //}

        ////Brings the phone down when P pressed again
        //else if (Up == false)
        //{
        //    var step = liftSpeed * Time.deltaTime;
        //    snakeScreenPos.position = Vector3.MoveTowards(snakeScreenPos.position, TargetPosDown.position, step);
        //    if (Vector3.Distance(snakeScreenPos.position, TargetPosDown.position) < 0.001f)
        //    {
        //        snakeScreenPos.position = TargetPosDown.position;
        //    }
        //}
        ////Default down
        //else snakeScreenPos.position = TargetPosDown.position;
    }

    //public void LiftPhone()
    //{
    //    if (Up == false)
    //    {
    //        Up = true;
    //    }
    //    else Up = false;
    //}
}
