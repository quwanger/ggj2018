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
        MovingToExit = 3
    }

    public NPCState _npcState = NPCState.None;

	public float wanderSpeedModifier = 0.05f;
	[SerializeField]
	public float flashSaleSpeedMultiplier = 2.0f;
	[SerializeField]
	public int points = 1;
	private float timeout;
    [SerializeField]
    public int timeoutMin = 3;
    [SerializeField]
    public int timeoutMax = 15;
    public Vector2 targetGoal = Vector2.zero;

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
            default:
                break;
        }
	}

    public void Init()
    {
        DecideNextTarget(true);
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
    }

    private void DecideNextTarget(bool justSpawed = false)
    {
        int randomTarget = Random.Range(0, 100);

        if(randomTarget < 10 && !justSpawed)
        {
            FindExit();
        }
        else if(randomTarget < 35)
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
