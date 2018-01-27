﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerSupport : MonoBehaviour
{

    //global vars
    protected float x;
    protected float y;
    protected float moveSpeed = 6;

    private PlayerController currPlayerController; 

    protected Rigidbody2D rb;
    int myPlayerID;

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
    }

    void keyPressedTimer()
    {
        //if we want to switch it to the triggers, button name is TriggersR_ or TriggersL_
        if (Input.GetButtonDown(string.Concat("A_", myPlayerID)))
        {
            timePressed = Time.time;
        }

        if (Input.GetButtonUp(string.Concat("A_", myPlayerID)))
        {
            timePressed = Time.time - timePressed;
            Debug.Log("Player " + myPlayerID + " Pressed A for : " + timePressed + " Seconds");
        }
    }
}
