using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : EntityController
{
    public int playerId;

    public Sneeze sneeze;
    public Cough cough;
    public int segments;
    public GameObject divider;
    public float timePressed;
    public GameObject chargeBarFG;
    public bool isCharging;
    public bool isRegenerating;
    private float regenerationTime = 0f;
    public GameObject escalatorNotification;
    public float currPower;
    public RectTransform dividerParent;
    public Color playerColor;

    public Transform chargeBarCanvas;
    public GameObject coughChargeBar;
    public GameObject achooChargeBar;

    // Use this for initialization
    void Start () {
        // Initialize stamina bar
        for(int i = 0; i < segments; i++) {
            GameObject d = Instantiate(divider, transform.position, 
                transform.rotation, dividerParent);
        }

        coughChargeBar.SetActive(true);
        achooChargeBar.SetActive(false);
	}

    override public void Move(Vector2 direction, float speedModifier = 1.0f)
    {
        base.Move(direction, speedModifier);
        float chargeCanvasSize = Mathf.Abs(chargeBarCanvas.transform.localScale.x);
        if(direction.x > 0) chargeBarCanvas.localScale = new Vector2(-chargeCanvasSize, chargeCanvasSize);
        else if(direction.x < 0) chargeBarCanvas.localScale = new Vector2(chargeCanvasSize, chargeCanvasSize);
	}

    public override void Sneeze()
    {
        base.Sneeze();
        Sneeze s = sneeze;
        //Debug.Log(this.gameObject);
        s.owner = this.gameObject;
        s.dischargePower = currPower;
        s.initiateSneeze();
        //adds recoil
        StopPlayer(currPower);
    }

    public void StopPlayer(float countdownFrom)
    {
        float time = countdownFrom - Time.timeSinceLevelLoad;
        this.GetComponent<Rigidbody2D>().velocity = -this.GetComponent<Rigidbody2D>().velocity * currPower;
    }



    public override void Cough()
    {
        base.Cough();
        Cough c = cough;
        c.owner = this.gameObject;
        c.dischargePower = currPower;
        c.initiateCough();
        //adds recoil
        StopPlayer(currPower/2);
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

    public void CompleteEscalatorRide()
    {

    }
}
