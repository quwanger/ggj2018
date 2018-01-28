using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : EntityController
{
    public Sneeze sneezeEffect;
    public Cough coughEffect;
    public int segments;
    public GameObject divider;
    public float timePressed;
    public GameObject chargeBarFG;
    public bool isCharging;
    public bool isRegenerating;
    private float regenerationTime = 0f;
    public GameObject escalatorNotification;
    float currPower;
    public RectTransform dividerParent;

    // Use this for initialization
    void Start () {
        // Initialize stamina bar
        for(int i = 0; i < segments; i++) {
            GameObject d = Instantiate(divider, transform.position, 
                transform.rotation, dividerParent);
        }

	}

    public override void Sneeze()
    {
        base.Sneeze();
        Sneeze s = sneezeEffect;
        ParticleSystem sneezePS = s.GetComponent<ParticleSystem>();
        
        
        s.owner = this;
     

        if(currPower <= 1)
        {
            ParticleSystem.MainModule myModule = sneezePS.main;
            myModule.startSpeedMultiplier = 1f;
            myModule.maxParticles = 10;
       

        }
        else if ( currPower > 1 && currPower <= 2)
        {
            ParticleSystem.MainModule myModule = sneezePS.main;
            myModule.startSpeedMultiplier = 2f;
            myModule.maxParticles = 25;

        }
        else 
        {
            ParticleSystem.MainModule myModule = sneezePS.main;
            myModule.startSpeedMultiplier = 4.5f;
            myModule.maxParticles = 80;

        }

        ParticleSystem.Particle[] m_Particles = new ParticleSystem.Particle[sneezePS.main.maxParticles];
        int numParticlesAlive = sneezePS.GetParticles(m_Particles);
        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++)
        {
            m_Particles[i].velocity = Vector3.Scale(m_Particles[i].velocity, new Vector3(1,1,0));

            //myvelocity.z = 0;

            //m_Particles[i].velocity = myvelocity;


        }

        // Apply the particle changes to the particle system
        //sneezePS.SetParticles(m_Particles, numParticlesAlive);

        sneezePS.Play();
    }

    public override void Cough()
    {
        base.Cough();
        Cough c = coughEffect;
        ParticleSystem coughPS = c.GetComponent<ParticleSystem>();
  

        c.owner = this;
        ParticleSystem.MainModule myModule = coughPS.main;

        if (currPower <= 1)
        {
            
            myModule.startSpeedMultiplier = 1f;
            myModule.maxParticles = 10;


        }
        else if (currPower > 1 && currPower <= 2)
        {
           
            myModule.startSpeedMultiplier = 20f;
            myModule.maxParticles = 25;

        }
        else
        {
            
            myModule.startSpeedMultiplier = 45f;
            myModule.maxParticles = 80;

        }

        ParticleSystem.Particle[] m_Particles = new ParticleSystem.Particle[coughPS.main.maxParticles];
        int numParticlesAlive = coughPS.GetParticles(m_Particles);
        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++)
        {
           var myvelocity = m_Particles[i].velocity;
                
           myvelocity.z = 0;
        }

        // Apply the particle changes to the particle system
        coughPS.SetParticles(m_Particles, numParticlesAlive);

        coughPS.Play();
        _animator.SetTrigger("cough");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (isRegenerating)
        {
            // The 0.01 is just to make it feel smoother
            if (chargeBarFG.GetComponent<Image>().fillAmount <= 0.01)
            {
                isRegenerating = false;
                regenerationTime = 0f;
            }

            float fillAmount = chargeBarFG.GetComponent<Image>().fillAmount;

            regenerationTime += Time.deltaTime * 0.1f;
            chargeBarFG.GetComponent<Image>().fillAmount = Mathf.Lerp(fillAmount, 0, regenerationTime);
        }

        if (isCharging)
        {
            // Current time - time when trigger was pressed
            int roundedTimePressed = (int)Mathf.Floor(Time.time - timePressed) * 2;
            //Debug.Log(roundedTimePressed);

            // Holding a discharge button for longer than you can charge
            if (roundedTimePressed > segments)
            {
                chargeBarFG.GetComponent<Image>().fillAmount = 1.0f;
            }

            // Holding a discharge button for shorter than a segment
            else if (roundedTimePressed < 1)
            {
                chargeBarFG.GetComponent<Image>().fillAmount = 1.0f / segments;
            }

            // Holding a discharge button for longer than a segment but less than max
            else
            {
                chargeBarFG.GetComponent<Image>().fillAmount = (1.0f / segments) * roundedTimePressed;
            }

            currPower = roundedTimePressed;
        }
    }
    public override void EnableEscalatoring(Escalator escalator)
    {
        escalatorNotification.SetActive(true);
        base.EnableEscalatoring(escalator);
    }

    public override void DisableEscalatoring()
    {
        escalatorNotification.SetActive(false);
        base.DisableEscalatoring();
    }
}
