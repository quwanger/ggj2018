using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DischargeController : MonoBehaviour {
    public int power = 1;
    public PlayerController owner;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int CalculatePower(float timePressed) {
        int tempPower = (int)Mathf.Floor(timePressed * 2);

        if (tempPower > 3)
        {
            power = 3;
        }
        else if (tempPower < 1)
        {
            power = 1;
        } else
        {
            power = tempPower;
        }

        Debug.Log("Cough power: " + power);

        return power;

    }
}
