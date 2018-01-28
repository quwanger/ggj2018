using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sneeze : DischargeController {
    private AudioManager audioManager;

    void Start() {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void initiateSneeze()
    {
        audioManager.PlaySound("sneezes");

        ParticleSystem sneezePS = this.GetComponent<ParticleSystem>();

        float rayDistance = 0f;

        if (dischargePower <= 1)
        {
            ParticleSystem.MainModule myModule = sneezePS.main;
            myModule.startSpeedMultiplier = 15f;
            myModule.maxParticles = 20;
            rayDistance = 2f;
        }
        else if (dischargePower > 1 && dischargePower <= 2)
        {
            ParticleSystem.MainModule myModule = sneezePS.main;
            myModule.startSpeedMultiplier = 25f;
            myModule.maxParticles = 40;
            rayDistance = 3f;
        }
        else
        {
            ParticleSystem.MainModule myModule = sneezePS.main;
            myModule.startSpeedMultiplier = 35f;
            myModule.maxParticles = 80;
            rayDistance = 4f;
        }

        sneezePS.Play();
        shootRay(rayDistance, "sneeze");
    }

    void Update() {

    }
}

