using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : EntityController
{
	
	[SerializeField]
	public float wanderSpeedModifier = 0.5f;
	[SerializeField]
	public float flashSaleSpeedMultiplier = 2.0f;
	[SerializeField]
	public int points = 1;
	[SerializeField]
	public float timeout = 10.0f;
	public Vector2 targetGoal = Vector2.zero;

	private bool goalReached = false;
	public bool leaving = false;
	private Vector2 wanderPosition = Vector2.zero;
	public bool isFlashSale = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (!goalReached && targetGoal != Vector2.zero) {//move towards goal (store/exit)
			if(CheckProximity(targetGoal)) {
				GoalReached();
			} else {
				MoveToGoal();
			}
		} else if(goalReached && !leaving) {//wander
			Wander();
		} else if(goalReached && leaving) {
			MoveToGoal();
		}
	}

	private bool CheckProximity(Vector2 target) {
		return Vector2.Distance(transform.position, target) <= 0.5f;
	}

	private void GoalReached() {
		
		GetWanderPosition();
		goalReached = true;
		StartCoroutine("Timeout");
	}

	private void MoveToGoal() {
		Vector2 direction = targetGoal - (Vector2)transform.position;
		direction.Normalize();
		Move(direction, (isFlashSale ? flashSaleSpeedMultiplier : 1.0f));
	}

	private void GetWanderPosition() {
		
		wanderPosition = new Vector2(targetGoal.x + Random.Range(-2.222f, 2.222f), targetGoal.y);
	}

	//timeout wander behavior - then find exit and leave
	IEnumerator Timeout() {
		yield return new WaitForSeconds(timeout);
		// Destroy(gameObject);
		FindExit();
	}

	private void Wander() {
		if(CheckProximity(wanderPosition)) {
			GetWanderPosition();
		} else {
			Vector2 target = (wanderPosition - (Vector2)transform.position);
			target.Normalize();
			Move(target, wanderSpeedModifier + (isFlashSale ? flashSaleSpeedMultiplier : 1.0f));
		}
	}

	private void FindExit() {

		NpcExit[] exits = Object.FindObjectsOfType<NpcExit>();
		int exitIndex = Random.Range(0, exits.Length);
		if(exits[exitIndex]) {
			targetGoal = exits[exitIndex].transform.position;
			leaving = true;
		}
	}

	public void AwardPoints() {
		Debug.Log("Points awarded");
	}
}
