using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : EntityController
{
	public float timeout;
	public Vector2 targetGoal = Vector2.zero;

	// Use this for initialization
	void Start () {
		StartCoroutine("Timeout");	
	}
	
	// Update is called once per frame
	void Update () {
		
		Debug.Log(targetGoal);
		if (targetGoal != Vector2.zero) {
			if(Vector2.Distance(transform.position, targetGoal) <= 0.5f) {
				targetGoal = Vector2.zero;
			} else {
				Vector2 direction = targetGoal - (Vector2)transform.position;
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
