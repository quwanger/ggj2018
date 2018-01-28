using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cough : DischargeController {
    private AudioManager audioManager;

	// Use this for initialization
	void Start () {
        Debug.Log("Cough");

        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update () {
        
    }

    public void initiateCough()
    {
        ParticleSystem coughPS = this.GetComponent<ParticleSystem>();

        audioManager.PlaySound("player coughs");

        float rayDistance = 0f;

        if (dischargePower <= 1)
        {
            ParticleSystem.MainModule myModule = coughPS.main;
            myModule.startSpeedMultiplier = 13.0f;
            myModule.maxParticles = 10;
            rayDistance = 2f;
            dischargeLevel = 1;
        }
        else if (dischargePower > 1 && dischargePower <= 2)
        {
            ParticleSystem.MainModule myModule = coughPS.main;
            myModule.startSpeedMultiplier = 20.0f;
            myModule.maxParticles = 30;
            rayDistance = 3f;
            dischargeLevel = 2;
        }
        else
        {
            ParticleSystem.MainModule myModule = coughPS.main;
            myModule.startSpeedMultiplier = 35.0f;
            myModule.maxParticles = 80;
            rayDistance = 4f;
            dischargeLevel = 3;
        }

        coughPS.Play();

        shootRay(rayDistance, "cough");
    }
}
