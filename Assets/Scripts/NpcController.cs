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
        MovingToEscalator = 4
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

	private bool goalReached = false;
	public bool leaving = false;
	private Vector2 wanderPosition = Vector2.zero;
	public bool isFlashSale = false;

    private int currentFloor;
	
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
        Vector2 direction = targetEscalator.x > transform.position.x ? new Vector2(1f, 0f) : new Vector2(-1f, 0);
        Move(direction, (isFlashSale ? flashSaleSpeedMultiplier : 1.0f));
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

	private void FindExit() {

        _npcState = NPCState.MovingToExit;
		NpcExit[] exits = Object.FindObjectsOfType<NpcExit>();
		int exitIndex = Random.Range(0, exits.Length);
		if(exits[exitIndex]) {
			targetGoal = exits[exitIndex].transform.position;
			leaving = true;
		}
	}

    private void BeginToWander()
    {
        _npcState = NPCState.Wandering;
    }

    private void FindStore()
    {
        _npcState = NPCState.MovingToGoal;
        targetGoal = GameManager.Instance.MapManager.GetRandomStorefrontPosition();

        if ((transform.position.y - targetGoal.y) > 1)
        {
            //go down
            _npcState = NPCState.MovingToEscalator;
            targetEscalator = FindDownEscalator();
        }
        else if ((transform.position.y - targetGoal.y) < -1)
        {
            //go up
            _npcState = NPCState.MovingToEscalator;
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
        else if(randomTarget < 40)
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
}
