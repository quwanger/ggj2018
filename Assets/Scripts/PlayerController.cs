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

    public override void Sneeze(float timePressed)
    {
        base.Sneeze(timePressed);

        Sneeze s = Instantiate(sneezePrefab, transform.position, transform.rotation);
        s.owner = this;
        s.CalculatePower(timePressed);
    }

    public override void Cough(float timePressed)
    {
        base.Cough(timePressed);

        Cough c = Instantiate(coughPrefab, transform.position, transform.rotation);
        c.owner = this;
        c.CalculatePower(timePressed);
    }

    // Update is called once per frame
    void Update () {
       
    }
}
