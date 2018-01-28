using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sneeze : DischargeController {

    void Start() {
        Debug.Log("Sneezed");
    }

    public void initiateSneeze()
    {
        ParticleSystem sneezePS = this.GetComponent<ParticleSystem>();

        float rayDistance = 0f;

        if (dischargePower <= 1)
        {
            ParticleSystem.MainModule myModule = sneezePS.main;
            myModule.startSpeedMultiplier = 1f;
            myModule.maxParticles = 10;
            rayDistance = 2f;
        }
        else if (dischargePower > 1 && dischargePower <= 2)
        {
            ParticleSystem.MainModule myModule = sneezePS.main;
            myModule.startSpeedMultiplier = 2f;
            myModule.maxParticles = 25;
            rayDistance = 3f;
        }
        else
        {
            ParticleSystem.MainModule myModule = sneezePS.main;
            myModule.startSpeedMultiplier = 4.5f;
            myModule.maxParticles = 80;
            rayDistance = 4f;
        }

        sneezePS.Play();
        shootRay(rayDistance);
    }

    void Update() {

    }
}

