using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : EntityController
{
    public Sneeze sneezePrefab;
    public Cough coughPrefab;
    public GameObject escalatorNotification;

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
        _animator.SetTrigger("cough");
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
