using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnerController : MonoBehaviour {

	public NpcController npcToSpawn;
	//spawn rate of npcs
	public int rateLowerLimit = 3;
	public int rateUpperLimit = 5;
	public int rateOffset = 0;

	public List<NpcController> spawnedNpcs;

	// Use this for initialization
	void Start () {
		spawnedNpcs = new List<NpcController>();
		StartCoroutine("SpawnNpc");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator SpawnNpc() {

		while(true) {
			yield return new WaitForSeconds(Random.Range(rateOffset + rateLowerLimit, rateOffset + rateUpperLimit));
			NpcController n = Instantiate(npcToSpawn, transform.position, Quaternion.identity);
			n.timeout = 10.0f;
			n.targetGoal = new Vector2(0.0f, transform.position.y);
			spawnedNpcs.Add(n);
		}
	}
}
