﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DischargeController : MonoBehaviour {
    public float dischargePower;
    public int dischargeLevel;
    public GameObject owner;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void shootRay(float rayDistance, string type)
    {
        float direction = transform.parent.localScale.x * -1;
        Debug.Log("Direction: " + direction);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(direction, 0), rayDistance);

        Debug.DrawRay(transform.position, new Vector2(direction, 0), Color.green, rayDistance);

        // List<RaycastHit2D> filteredHits = new List<RaycastHit2D>();

        for (int i = 0; i < hits.Length; i++)
        {
            GameObject npc = hits[i].collider.gameObject;

            if(type == "cough")
            {
                if (npc.layer == LayerMask.NameToLayer("NPC"))
                {
                    NpcController npcScript = npc.GetComponent<NpcController>();
                    npcScript.hitByCough(dischargeLevel, owner);
                    // filteredHits.Add(hits[i]);
                }
            } else
            {

            }
           
        }
    }
}
