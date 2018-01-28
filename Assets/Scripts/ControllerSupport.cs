using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSupport : MonoBehaviour
{

    //global vars
    protected float x;
    protected float y;

    private PlayerController currPlayerController;

    protected Rigidbody2D rb;
    public int myPlayerID;

    void Start()
    {
        //get player tag
        string myTag = this.tag;
        myPlayerID = Convert.ToInt32(myTag.Substring(myTag.Length - 1, 1));

        //get player rigidbody and controller
        rb = GetComponent<Rigidbody2D>();
        currPlayerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!currPlayerController.RidingEscalator)
        {
            //move the player by getting the normalized vector created by the joystick
            Vector3 mydir = new Vector2(x, y).normalized;
            movePlayer();
            //get keypresses.
            keyPressedTimer();
        }
    }

    void movePlayer()
    {
        //based on the player ID, access the correct controller joystick data
        string myX = string.Concat("L_XAxis_", myPlayerID);
        string myY = string.Concat("L_YAxis_", myPlayerID);

        //assign it to x and y; Ignore Y for now (no up movement)
        x = Input.GetAxis(myX);
        y = 0;//Input.GetAxis(myY);

        Vector3 dir = new Vector3(x, y);
        dir = dir.normalized;

        //this moves the player directly; do not use unless player controller is not available. 
        /*rb.AddForce(dir * moveSpeed);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 30.0f);*/

        currPlayerController.Move(dir);
        if (x == 0)
        {
            currPlayerController.StopMove();
        }
    }

    private bool leftTriggerInUse = false;


    void keyPressedTimer()
    {
        if (!currPlayerController.isRegenerating)
        {
            if (Input.GetButtonDown(string.Concat("A_", myPlayerID)))
            {
                currPlayerController.timePressed = Time.time;
                currPlayerController.isCharging = true;
            }

            if (Input.GetButtonUp(string.Concat("A_", myPlayerID)))
            {
                currPlayerController.isCharging = false;
                currPlayerController.isRegenerating = true;

                currPlayerController.Cough();
            }
        }

        if (!currPlayerController.isRegenerating)
        {
            if (Input.GetButtonDown(string.Concat("X_", myPlayerID)))
            {
                currPlayerController.timePressed = Time.time;
                currPlayerController.isCharging = true;
            }

            if (Input.GetButtonUp(string.Concat("X_", myPlayerID)))
            {
                currPlayerController.isCharging = false;
                currPlayerController.isRegenerating = true;

                currPlayerController.Sneeze();
            }
        }

        if (Input.GetAxis(string.Concat("L_YAxis_", myPlayerID)) != 0)
        {
            if (Input.GetAxis(string.Concat("L_YAxis_", myPlayerID)) < 0)
            {
                if (currPlayerController.InEscalatorRange)
                {
                    currPlayerController.GoDownEscalator(currPlayerController.CompleteEscalatorRide);
                }
            }
            else
            {
                if (currPlayerController.InEscalatorRange)
                {
                    currPlayerController.GoUpEscalator(currPlayerController.CompleteEscalatorRide);
                }
            }
        }
    }
}
