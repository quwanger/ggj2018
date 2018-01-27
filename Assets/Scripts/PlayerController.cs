using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(this.tag.Equals("player1"))
        {
            if (Input.GetKey(KeyCode.A))
            {
                Move(Vector2.left);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Move(Vector2.right);
            }
            if (Input.GetKey(KeyCode.W))
            {
                //up escalator
                // if(transform.position.y < -3f) Move(Vector2.up);
            }
            if (Input.GetKey(KeyCode.S))
            {
                //down escalator
                // if(transform.position.y > -4f) Move(Vector2.down);
            }
            if (Input.GetKey(KeyCode.Space))
            {

            }
        }
        else if (this.tag.Equals("player2"))
        {
            if (Input.GetKey(KeyCode.H))
            {
                Move(Vector2.left);
            }
            if (Input.GetKey(KeyCode.K))
            {
                Move(Vector2.right);
            }
            if (Input.GetKey(KeyCode.U))
            {
                //up escalator
                // if(transform.position.y < -3f) Move(Vector2.up);
            }
            if (Input.GetKey(KeyCode.J))
            {
                //down escalator
                // if(transform.position.y > -4f) Move(Vector2.down);
            }
            if (Input.GetKey(KeyCode.M))
            {

            }
        }
        else if (this.tag.Equals("player3"))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Move(Vector2.left);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Move(Vector2.right);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                //up escalator
                // if(transform.position.y < -3f) Move(Vector2.up);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                //down escalator
                // if(transform.position.y > -4f) Move(Vector2.down);
            }
            if (Input.GetKey(KeyCode.Delete))
            {

            }
        }
        else if (this.tag.Equals("player4"))
        {
            if (Input.GetKey(KeyCode.Keypad4))
            {
                Move(Vector2.left);
            }
            if (Input.GetKey(KeyCode.Keypad6))
            {
                Move(Vector2.right);
            }
            if (Input.GetKey(KeyCode.Keypad8))
            {
                //up escalator
                // if(transform.position.y < -3f) Move(Vector2.up);
            }
            if (Input.GetKey(KeyCode.Keypad5))
            {
                //down escalator
                // if(transform.position.y > -4f) Move(Vector2.down);
            }
            if (Input.GetKey(KeyCode.Keypad2))
            {

            }
        }

    }
}
