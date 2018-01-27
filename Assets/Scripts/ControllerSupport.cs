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

    float timePressed = 0f;

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
        if(x == 0) {
            currPlayerController.StopMove();
        }
    }

    private bool rightTriggerInUse = false;
    private bool leftTriggerInUse = false;


    void keyPressedTimer() {
        Debug.Log(Input.GetAxis(string.Concat("TriggersR_", myPlayerID)));
        
        if (Input.GetAxis(string.Concat("TriggersR_", myPlayerID)) != 0)
        {
            if (rightTriggerInUse == false)
            {
                timePressed = Time.time;
        
                // Start player charge bar
                Debug.Log("Pressed TriggersR_" + myPlayerID);

                rightTriggerInUse = true;
            }
        }
        if (Input.GetAxis(string.Concat("TriggersR_", myPlayerID)) == 0)
        {
            if (rightTriggerInUse) {
                rightTriggerInUse = false;
                timePressed = Time.time - timePressed;
                Debug.Log("Released TriggersR_"+ myPlayerID);

                currPlayerController.Cough(timePressed);
            }
        }

        if (Input.GetAxis(string.Concat("TriggersL_", myPlayerID)) != 0)
        {
            if (leftTriggerInUse == false)
            {
                timePressed = Time.time;

                // Stop player charge bar
                Debug.Log("Pressed TriggersL_"+ myPlayerID);

                leftTriggerInUse = true;
            }
        }
        if (Input.GetAxis(string.Concat("TriggersL_", myPlayerID)) == 0)
        {
            if (leftTriggerInUse)
            {
                leftTriggerInUse = false;
                timePressed = Time.time - timePressed;
                currPlayerController.Sneeze(timePressed);
                Debug.Log("Released TriggersL_"+ myPlayerID);
            }
        }
    }
}
