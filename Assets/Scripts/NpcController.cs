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
        RidingEscalator = 5,
        MovingToSale = 6
    }

    public GameObject deathNotification;

    public NPCState _npcState = NPCState.None;
    public NPCState _previousNpcState = NPCState.None;

    public float wanderSpeedModifier = 0.05f;
	public float flashSaleSpeedMultiplier = 1.3f;
	public int points = 1;
	private float timeout;
    public int timeoutMin = 3;
    public int timeoutMax = 8;
    public float saleLingerMultiplier;
    public Vector2 targetGoal = Vector2.zero;
    private Escalator targetEscalator;
    private Vector2 targetEscalatorPosition;
    private bool goingUp = false;

    private float npcLifespan;
    private bool npcIsDead = false;

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
            case NPCState.MovingToSale:
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

        //failsafe
        if(transform.position.y < -10)
        {
            ExitNPC();
        }
	}

    private void ChangeNPCState(NPCState state)
    {
        _previousNpcState = _npcState;
        _npcState = state;
    }

    public void Init(int floor)
    {
        npcLifespan = Random.Range(10, 120);
        StartCoroutine("StartKillingShopper");
        currentFloor = floor;
        DecideNextTarget(true);
        Physics2D.IgnoreLayerCollision(8, 10, true);
    }

    IEnumerator IgnoreWallsTimer()
    {
        yield return new WaitForSeconds(1f);
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
        if (CheckProximity(targetEscalatorPosition))
        {
            ChangeNPCState(NPCState.RidingEscalator);
            if (targetEscalator.IsShutdown)
            {
                targetEscalator = null;
                //if you get to the escalator, and its shutdown... give up and find a new target
                DecideNextTarget();
            }
            else
            {
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
        }
        else
        {
            Vector2 direction = targetEscalatorPosition.x > transform.position.x ? new Vector2(1f, 0f) : new Vector2(-1f, 0);
            Move(direction, (isFlashSale ? flashSaleSpeedMultiplier : 1.0f));
        }
    }

    private void CompleteEscalatorRide()
    {
        if (leaving)
        {
            ChangeNPCState(NPCState.MovingToExit);
        }
        else if (npcIsDead)
        {
            FindExit();
        }
        else if(_previousNpcState == NPCState.MovingToSale)
        {
            ChangeNPCState(_previousNpcState);
        }
        else
        {
            ChangeNPCState(NPCState.MovingToGoal);
        }

        CheckIfTargetIsOnFloor();
    }

    private void MoveToExit()
    {
        if(CheckProximity(targetGoal))
        {
            //reached exit
            ExitNPC();
        }
        else
        {
            MoveToGoal();
        }
    }

    public void ExitNPC(bool isKilled = false)
    {
        if(isKilled)
        {
            Instantiate(deathNotification, transform.position, Quaternion.identity);
        }

        if(mostInfectedBy != null)
        {
            int infectedBy = mostInfectedBy.GetComponent<PlayerController>().playerId;
            GameManager.Instance.PlayerScores[infectedBy] += powerOfLastCough;
            Debug.Log("<color=blue>Player " + mostInfectedBy.name + " has just received " + powerOfLastCough + " points. They now have " + GameManager.Instance.PlayerScores[infectedBy].ToString() + " points.</color>");
        }

        GameManager.Instance.NPCManager.AllNpcs.Remove(this);
        Destroy(this.gameObject);
    }

	private void GetWanderPosition() {
        ChangeNPCState(NPCState.Wandering);
        wanderPosition = new Vector2(targetGoal.x + Random.Range(-2f, 2f), 0f);
	}

	//timeout wander behavior - then find exit and leave
	IEnumerator Timeout() {
		yield return new WaitForSeconds(_previousNpcState == NPCState.MovingToSale ? timeout * saleLingerMultiplier : timeout);
        DecideNextTarget();
    }

    IEnumerator StartKillingShopper()
    {
        yield return new WaitForSeconds(npcLifespan);
        npcIsDead = true;
        SendToExit();
    }

    private void SendToExit()
    {
        FindExit();
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
        ChangeNPCState(NPCState.MovingToExit);
        leaving = true;
        List<NpcExit> exits = GameManager.Instance.MapManager.GetActiveExits();
		targetGoal = exits[Random.Range(0, exits.Count)].transform.position;
        CheckIfTargetIsOnFloor();
    }

    private void BeginToWander()
    {
        ChangeNPCState(NPCState.Wandering);
    }

    private void FindStore()
    {
        ChangeNPCState(NPCState.MovingToGoal);
        targetGoal = GameManager.Instance.MapManager.GetRandomStorefrontPosition();
        CheckIfTargetIsOnFloor();
    }

    public void SendToSale(Transform store)
    {
        if (_npcState != NPCState.RidingEscalator && !npcIsDead)
        {
            ChangeNPCState(NPCState.MovingToSale);
            targetGoal = store.position;
            CheckIfTargetIsOnFloor();
            isFlashSale = true;
        }
    }

    private void CheckIfTargetIsOnFloor()
    {
        if ((transform.position.y - targetGoal.y) > 1)
        {
            //go down
            ChangeNPCState(NPCState.MovingToEscalator);
            goingUp = false;
            targetEscalator = FindDownEscalator();
            if(!targetEscalator)
            {
                //if all escalators are under construction, just wander
                BeginToWander();
            }
            else
            {
                targetEscalatorPosition = new Vector2(targetEscalator.TargetTop.position.x, targetEscalator.TargetTop.position.y);
            }
        }
        else if ((transform.position.y - targetGoal.y) < -1)
        {
            //go up
            ChangeNPCState(NPCState.MovingToEscalator);
            goingUp = true;
            targetEscalator = FindUpEscalator();
            if (!targetEscalator)
            {
                //if all escalators are under construction, just wander
                BeginToWander();
            }
            else
            {
                targetEscalatorPosition = new Vector2(targetEscalator.TargetBottom.position.x, targetEscalator.TargetBottom.position.y);
            }
        }
        else
        {
            //it's on this floor
        }
    }

    private Escalator FindDownEscalator()
    {
        return GameManager.Instance.MapManager.GetEscalatorFromFloorToFloor(currentFloor - 1, currentFloor, true);
    }

    private Escalator FindUpEscalator()
    {
        return GameManager.Instance.MapManager.GetEscalatorFromFloorToFloor(currentFloor, currentFloor + 1, false);
    }

    private void DecideNextTarget(bool justSpawed = false)
    {
        int randomTarget = Random.Range(0, 100);
        isFlashSale = false;

        if (randomTarget < 20 && !justSpawed)
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
		//Debug.Log("Points awarded");
	}

    public virtual void hitByCough(int power, GameObject coughOwner)
    {
        audioManager.PlaySound("npc coughs");
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
