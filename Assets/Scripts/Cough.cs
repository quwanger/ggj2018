using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cough : DischargeController {

	// Use this for initialization
	void Start () {
        Debug.Log("Cough");
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void initiateCough()
    {
        ParticleSystem coughPS = this.GetComponent<ParticleSystem>();

        float rayDistance = 0f;

        if (dischargePower <= 1)
        {
            ParticleSystem.MainModule myModule = coughPS.main;
            myModule.startSpeedMultiplier = 1f;
            myModule.maxParticles = 10;
            rayDistance = 2f;
        }
        else if (dischargePower > 1 && dischargePower <= 2)
        {
            ParticleSystem.MainModule myModule = coughPS.main;
            myModule.startSpeedMultiplier = 20f;
            myModule.maxParticles = 25;
            rayDistance = 3f;
        }
        else
        {
            ParticleSystem.MainModule myModule = coughPS.main;
            myModule.startSpeedMultiplier = 45f;
            myModule.maxParticles = 80;
            rayDistance = 4f;
        }

        coughPS.Play();
        shootRay(rayDistance);
    }
}
