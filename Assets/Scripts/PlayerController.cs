using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : EntityController
{
    public Sneeze sneezePrefab;
    public int sneezePower = 1;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKey(KeyCode.A)) {
			Move(Vector2.left);
		}
		if(Input.GetKey(KeyCode.D)) {
			Move(Vector2.right);
		}
		if(Input.GetKey(KeyCode.W)) {
			//up escalator
			// if(transform.position.y < -3f) Move(Vector2.up);
		}
		if(Input.GetKey(KeyCode.S)) {
			//down escalator
			// if(transform.position.y > -4f) Move(Vector2.down);
		}
		if(Input.GetKeyDown(KeyCode.Space)) {
            Sneeze s = Instantiate(sneezePrefab, transform.position, transform.rotation);
            s.owner = this;

            // s.timestamp = ms since epoch, not sure c# library
        }
	}
}
