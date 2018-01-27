using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerSupport : MonoBehaviour {

    protected float x;
    protected float y;
    protected float moveSpeed = 2;

    protected Rigidbody rb;
    int myPlayerID;

    // Use this for initialization
    void Start () {
        string myTag = this.tag;
        myPlayerID = Convert.ToInt32(myTag.Substring(myTag.Length - 1, 1));
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 mydir = new Vector3(x, y, 0).normalized;
        movePlayer();

    }

    void movePlayer()
    {
        string myX = string.Concat("L_XAxis_", myPlayerID);
        string myY = string.Concat("L_YAxis_", myPlayerID);

        x = Input.GetAxis(myX);
        y = Input.GetAxis(myY);

        Vector3 dir = new Vector3(x, y, 0);
        dir = dir.normalized;

        rb.AddForce(dir * moveSpeed);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 30.0f);

    }
}
