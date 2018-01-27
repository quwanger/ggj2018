using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : EntityController
{
	public float timeout;
	public Vector2? targetGoal;

	// Use this for initialization
	void Start () {
		targetGoal = null;
		StartCoroutine("Timeout");	
	}
	
	// Update is called once per frame
	void Update () {
		
		Debug.Log(targetGoal);
		if (targetGoal != null) {
			if(Vector2.Distance(transform.position, (Vector2)targetGoal) <= 0.5f) {
				targetGoal = null;
			} else {
				Vector2 direction = (Vector2)transform.position - (Vector2)targetGoal;
				direction.Normalize();
				Debug.Log(direction);
				Move(direction);
			}
		}
	}

	IEnumerator Timeout() {
		yield return new WaitForSeconds(timeout);
		Destroy(gameObject);
	}
}
