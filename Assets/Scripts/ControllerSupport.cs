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
    int myPlayerID;

    void Start () {
        //get player tag
        string myTag = this.tag;
        myPlayerID = Convert.ToInt32(myTag.Substring(myTag.Length - 1, 1));

        //get player rigidbody and controller
        rb = GetComponent<Rigidbody2D>();
        currPlayerController = GetComponent<PlayerController>();
    }
	
	void Update () {
        //move the player by getting the normalized vector created by the joystick
        Vector3 mydir = new Vector2(x, y).normalized;
        movePlayer();

        //get keypresses.
        keyPressedTimer();
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
    }

    private bool rightTriggerInUse = false;
    private bool leftTriggerInUse = false;


    void keyPressedTimer() {
        if (!currPlayerController.isRegenerating)
        {
            if (Input.GetAxis(string.Concat("TriggersR_", myPlayerID)) != 0)
            {
                if (rightTriggerInUse == false)
                {
                    currPlayerController.timePressed = Time.time;
                    currPlayerController.isCharging = true;

                    Debug.Log("RT Pressed");

                    rightTriggerInUse = true;
                }
            }
        }
        if (Input.GetAxis(string.Concat("TriggersR_", myPlayerID)) == 0)
        {
            if (rightTriggerInUse)
            {
                rightTriggerInUse = false;
                currPlayerController.isCharging = false;

                Debug.Log("RT Released");

                currPlayerController.isRegenerating = true;

                currPlayerController.Cough();
            }
        }
        

        if (!currPlayerController.isRegenerating)
        {
            if (Input.GetAxis(string.Concat("TriggersL_", myPlayerID)) != 0)
            {
                if (leftTriggerInUse == false)
                {
                    currPlayerController.timePressed = Time.time;
                    currPlayerController.isCharging = true;

                    Debug.Log("LT Pressed");

                    leftTriggerInUse = true;
                }
            }
        }
        if (Input.GetAxis(string.Concat("TriggersL_", myPlayerID)) == 0)
        {
            if (leftTriggerInUse)
            {
                leftTriggerInUse = false;
                currPlayerController.isCharging = false;

                Debug.Log("LT Released");

                currPlayerController.isRegenerating = true;

                currPlayerController.Sneeze();
            }
        }
    }
}
