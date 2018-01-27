using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : EntityController
{
    public Sneeze sneezePrefab;
    public Cough coughPrefab;


    // Use this for initialization
    void Start () {
	}

    public override void Sneeze(float timePressed = 2.0f)
    {
        base.Sneeze(timePressed);

        Sneeze s = Instantiate(sneezePrefab, transform.position, transform.rotation);
        s.owner = this;

        s.power = (int) timePressed;
    }

    public override void Cough(float timePressed = 2.0f)
    {
        base.Cough(timePressed);
        Debug.Log(coughPrefab);
        Cough c = Instantiate(coughPrefab, transform.position, transform.rotation);

        c.power = (int)timePressed;
    }

    // Update is called once per frame
    void Update () {
       
    }
}
