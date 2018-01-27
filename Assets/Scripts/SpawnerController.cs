using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnerController : MonoBehaviour {

	public NpcController npcToSpawn;
	//spawn rate of npcs
	[SerializeField]
	public float rateLowerLimit = 3;
	[SerializeField]
	public float rateUpperLimit = 5;
	public float rateOffset = 0; //for flash sales - add this to lower and upper rate to increase or decrease spawn rate 

	[SerializeField]
	public int burstLowerLimit = 1;
	[SerializeField]
	public int burstUpperLimit = 10;

	public Transform tempGoal;

	// Use this for initialization
	void Start () {
		StartCoroutine("SpawnNpc");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator SpawnNpc() {

		while(true) {
			float offset = Mathf.Max(rateOffset, 0);
			yield return new WaitForSeconds(Random.Range(offset + rateLowerLimit, offset + rateUpperLimit));
			for(int i = 0; i < Random.Range(burstLowerLimit, burstUpperLimit); i++) {
				yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
				NpcController n = Instantiate(npcToSpawn, transform.position, Quaternion.identity);
				n.targetGoal = tempGoal.position;
			}
		}
	}
}
