﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

	public float speed;

	//cough tracking
	public int powerOfLastCough;
	public GameObject mostInfectedBy;

	protected Rigidbody2D _rigidBody;
	protected Animator _animator;

    protected EscalatorRide _escalatorRide = null;
    protected bool _ridingEscalator = false;
    public bool RidingEscalator { get { return _ridingEscalator; } }
    protected Escalator _escalatorInRange = null;
    private bool _inEscalatorRange = false;
    public bool InEscalatorRange { get { return _inEscalatorRange; } }

    public Action ElevatorRideComplete;

    virtual protected void Awake() {

		_rigidBody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
	}

    private float t = 0.0f;

    public virtual void Update()
    {
        if (_ridingEscalator)
        {
            float posX = Mathf.Lerp(_escalatorRide.StartinPosition.x, _escalatorRide.TargetPosition.x, t);
            float posY = Mathf.Lerp(_escalatorRide.StartinPosition.y, _escalatorRide.TargetPosition.y, t);

            transform.position = new Vector3(posX, posY + 0.7f, transform.position.z);

            t += (_escalatorRide.RideSpeed / 10f) * Time.deltaTime;

            if (t > 1.0f)
            {
                GetOffEscalator();
            }
        }
    }

    public PointParticle pointParticle;
    public Sprite[] pointSprites;

    public void SpawnPoints(int points)
    {
        PointParticle pp = Instantiate(pointParticle, transform.position, Quaternion.identity, transform);
        switch(points)
        {
            case 1:
                pp.spriteRenderer.sprite = pointSprites[0];
                break;
            case 2:
                pp.spriteRenderer.sprite = pointSprites[1];
                break;
            case 3:
                pp.spriteRenderer.sprite = pointSprites[2];
                break;
            case 4:
                break;
        }
    }

	virtual public void Move(Vector2 direction, float speedModifier = 1.0f)
    {
        if (!_ridingEscalator)
        {
		    if(!_animator.GetBool("walking")) {
			    _animator.SetBool("walking", true);
		    }

		    if(direction.x > 0) transform.localScale = new Vector2(-1, 1);
		    else if(direction.x < 0) transform.localScale = new Vector2(1, 1);

            Vector2 flatDirection = new Vector2(direction.x, 0f);

            _rigidBody.AddForce(flatDirection * speed * speedModifier * Time.deltaTime * 60);
        }
	}

	virtual public void StopMove() {

		_animator.SetBool("walking", false);
	}

	virtual public void Sneeze() {
 
        _animator.SetTrigger("cough");
        //_animator.SetTrigger("Sneeze");
		// return GameObject.Instantiate(projectile, shootPoint.position, transform.rotation);
	}

    virtual public void Cough()
    {

        _animator.SetTrigger("cough");
        // return GameObject.Instantiate(projectile, shootPoint.position, transform.rotation);
    }

    virtual public void GetOffEscalator()
    {
        _ridingEscalator = false;
        t = 0.0f;
        _escalatorRide = null;
        //_escalatorInRange.EscalatorInUse = false;
        _escalatorInRange.CharactersOnEscalator.Remove(this);

        ElevatorRideComplete();
        ElevatorRideComplete = null;
    }

    virtual public void GoUpEscalator(Action rideCompleteCallback)
    {
        if (transform.position.y < _escalatorInRange.transform.position.y && !_escalatorInRange.IsShutdown)
        {
            ElevatorRideComplete = rideCompleteCallback;
            StopMove();
            //_escalatorInRange.EscalatorInUse = true;
            _escalatorInRange.CharactersOnEscalator.Add(this);
            _escalatorRide = new EscalatorRide(_escalatorInRange.TargetBottom.position, _escalatorInRange.TargetTop.position, _escalatorInRange.EscDirectionVertical == Escalator.EscalatorDirectionVertical.Up ? _escalatorInRange.ProperDirectionEscalatorSpeed : _escalatorInRange.WrongDirectionEscalatorSpeed);
            _ridingEscalator = true;
        }
    }

    virtual public void GoDownEscalator(Action rideCompleteCallback)
    {
        if (transform.position.y > _escalatorInRange.transform.position.y && !_escalatorInRange.IsShutdown)
        {
            ElevatorRideComplete = rideCompleteCallback;
            StopMove();
            //_escalatorInRange.EscalatorInUse = true;
            _escalatorInRange.CharactersOnEscalator.Add(this);
            _escalatorRide = new EscalatorRide(_escalatorInRange.TargetTop.position, _escalatorInRange.TargetBottom.position, _escalatorInRange.EscDirectionVertical == Escalator.EscalatorDirectionVertical.Down ? _escalatorInRange.ProperDirectionEscalatorSpeed : _escalatorInRange.WrongDirectionEscalatorSpeed);
            _ridingEscalator = true;
        }
    }

    virtual protected void Die() {
		_animator.SetTrigger("Die");
	}

	virtual public void OnDieAnimEnd() {
		Destroy(this.gameObject);
	}

    public virtual void EnableEscalatoring(Escalator escalator)
    {
        _escalatorInRange = escalator;
        _inEscalatorRange = true;
    }

    public virtual void DisableEscalatoring()
    {
        _escalatorInRange = null;
        _inEscalatorRange = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        Escalator escalator = coll.transform.GetComponent<Escalator>();
        if (escalator)
        {
            EnableEscalatoring(escalator);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.transform.GetComponent<Escalator>())
        {
            DisableEscalatoring();
        }
    }
}
