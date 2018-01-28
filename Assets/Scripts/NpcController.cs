using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : EntityController
{
    public enum NPCState
    {
        None = 0,
        Wandering = 1,
        MovingToGoal = 2,
        MovingToExit = 3,
        MovingToEscalator = 4,
        RidingEscalator = 5
    }

    public NPCState _npcState = NPCState.None;

	public float wanderSpeedModifier = 0.05f;
	public float flashSaleSpeedMultiplier = 2.0f;
	public int points = 1;
	private float timeout;
    public int timeoutMin = 3;
    public int timeoutMax = 8;
    public Vector2 targetGoal = Vector2.zero;
    private Vector2 targetEscalator;
    private bool goingUp = false;

	private bool goalReached = false;
	public bool leaving = false;
	private Vector2 wanderPosition = Vector2.zero;
	public bool isFlashSale = false;

    private int currentFloor;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
	
	// Update is called once per frame
	public override void Update () {

        base.Update();

        switch(_npcState)
        {
            case NPCState.MovingToGoal:
                if (CheckProximity(targetGoal))
                {
                    GoalReached();
                }
                else
                {
                    MoveToGoal();
                }
                break;
            case NPCState.Wandering:
                Wander();
                break;
            case NPCState.MovingToExit:
                MoveToExit();
                break;
            case NPCState.MovingToEscalator:
                MoveToEscalator();
                break;
            default:
                break;
        }
	}

    public void Init(int floor)
    {
        currentFloor = floor;
        DecideNextTarget(true);
        Physics2D.IgnoreLayerCollision(8, 10, true);
    }

	private bool CheckProximity(Vector2 target) {
        return Mathf.Abs(transform.position.x - target.x) <= 0.2f;
    }

	private void GoalReached() {
		GetWanderPosition();
		goalReached = true;
        ResetTimeout();
        StartCoroutine("Timeout");
	}

    private void ResetTimeout()
    {
        timeout = Random.Range(timeoutMin, timeoutMax);
    }

	private void MoveToGoal() {
		Vector2 direction = targetGoal.x > transform.position.x ? new Vector2(1f, 0f) : new Vector2(-1f, 0);
		Move(direction, (isFlashSale ? flashSaleSpeedMultiplier : 1.0f));
	}

    private void MoveToEscalator()
    {
        if (CheckProximity(targetEscalator))
        {
            _npcState = NPCState.RidingEscalator;
            //use escalator
            if (goingUp)
            {
                GoUpEscalator(CompleteEscalatorRide);
                currentFloor++;
            }
            else
            {
                GoDownEscalator(CompleteEscalatorRide);
                currentFloor--;
            }
        }
        else
        {
            Vector2 direction = targetEscalator.x > transform.position.x ? new Vector2(1f, 0f) : new Vector2(-1f, 0);
            Move(direction, (isFlashSale ? flashSaleSpeedMultiplier : 1.0f));
        }
    }

    private void CompleteEscalatorRide()
    {
        if (leaving)
        {
            _npcState = NPCState.MovingToExit;
        }
        else
        {
            _npcState = NPCState.MovingToGoal;
        }
        CheckIfTargetIsOnFloor();
    }

    private void MoveToExit()
    {
        if(CheckProximity(targetGoal))
        {
            //reached exit
            Destroy(this.gameObject);
        }
        else
        {
            MoveToGoal();
        }
    }

	private void GetWanderPosition() {
        _npcState = NPCState.Wandering;
		wanderPosition = new Vector2(targetGoal.x + Random.Range(-2f, 2f), 0f);
	}

	//timeout wander behavior - then find exit and leave
	IEnumerator Timeout() {
		yield return new WaitForSeconds(timeout);
        DecideNextTarget();
    }

	private void Wander() {
		if(CheckProximity(wanderPosition)) {
			GetWanderPosition();
		} else {
            Vector2 target = wanderPosition.x > transform.position.x ? new Vector2(1f, 0f) : new Vector2(-1f, 0);
            target.Normalize();
			Move(target, wanderSpeedModifier + (isFlashSale ? flashSaleSpeedMultiplier : 1.0f));
		}
	}

	private void FindExit()
    {
        _npcState = NPCState.MovingToExit;
        leaving = true;
        List<NpcExit> exits = GameManager.Instance.MapManager.GetActiveExits();
		targetGoal = exits[Random.Range(0, exits.Count)].transform.position;
        CheckIfTargetIsOnFloor();
    }

    private void BeginToWander()
    {
        _npcState = NPCState.Wandering;
    }

    private void FindStore()
    {
        _npcState = NPCState.MovingToGoal;
        targetGoal = GameManager.Instance.MapManager.GetRandomStorefrontPosition();
        CheckIfTargetIsOnFloor();
    }

    private void CheckIfTargetIsOnFloor()
    {
        if ((transform.position.y - targetGoal.y) > 1)
        {
            //go down
            _npcState = NPCState.MovingToEscalator;
            goingUp = false;
            targetEscalator = FindDownEscalator();
        }
        else if ((transform.position.y - targetGoal.y) < -1)
        {
            //go up
            _npcState = NPCState.MovingToEscalator;
            goingUp = true;
            targetEscalator = FindUpEscalator();
        }
        else
        {
            //it's on this floor
        }
    }

    private Vector2 FindDownEscalator()
    {
        return GameManager.Instance.MapManager.GetEscalatorFromFloorToFloor(currentFloor - 1, currentFloor, true);
    }

    private Vector2 FindUpEscalator()
    {
        return GameManager.Instance.MapManager.GetEscalatorFromFloorToFloor(currentFloor, currentFloor + 1, false);
    }

    private void DecideNextTarget(bool justSpawed = false)
    {
        int randomTarget = Random.Range(0, 100);

        if(randomTarget < 20 && !justSpawed)
        {
            FindExit();
        }
        else if(randomTarget < 40 && !justSpawed)
        {
            // randomly wander around
            BeginToWander();
        }
        else
        {
            // select new goal
            FindStore();
        }
    }

    public void AwardPoints() {
		Debug.Log("Points awarded");
	}

    public virtual void hitByCough(int power, GameObject coughOwner)
    {
        audioManager.PlaySound("npc coughs");

        Debug.Log("Power: " + power);
        Debug.Log("ExistingPower: " + powerOfLastCough);
        if (power < powerOfLastCough)
        {
            return;
        }
        else
        {
            powerOfLastCough = power;
            mostInfectedBy = coughOwner;

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            PlayerController ownerScript = coughOwner.GetComponent<PlayerController>();

            spriteRenderer.color = ownerScript.playerColor;
        }
    }
}
