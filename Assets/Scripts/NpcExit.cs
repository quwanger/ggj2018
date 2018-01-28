using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcExit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
        NpcController npc = other.GetComponent<NpcController>();
		if(npc)
        {
            npc.ExitNPC();
		}
	}
}
