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
        if (Input.GetKeyDown(KeyCode.Space)) {
            Sneeze s = Instantiate(sneezePrefab, transform.position, transform.rotation);
            s.owner = this;
            Debug.Log("Pressed Space");
            // s.timestamp = ms since epoch, not sure c# library
        }
    }
}
