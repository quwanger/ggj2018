using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnerController : MonoBehaviour {

	public NpcController npcToSpawn;
    [SerializeField]
    public int mallFloor;

    // Use this for initialization
    void Start () {
		StartCoroutine("SpawnNpc");
	}

	IEnumerator SpawnNpc() {

        yield return new WaitForSeconds(GameManager.Instance.NPCManager.startingRateOffset);

        while (true) {
			float offset = Mathf.Max(GameManager.Instance.NPCManager.rateOffset, 0);
			for(int i = 0; i < Random.Range(GameManager.Instance.NPCManager.burstLowerLimit, GameManager.Instance.NPCManager.burstUpperLimit); i++) {
				yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
				NpcController n = Instantiate(npcToSpawn, transform.position, Quaternion.identity);
                GameManager.Instance.NPCManager.AllNpcs.Add(n);
                n.Init(mallFloor);
			}
            yield return new WaitForSeconds(Random.Range(offset + GameManager.Instance.NPCManager.rateLowerLimit, offset + GameManager.Instance.NPCManager.rateUpperLimit));
        }
	}
}
