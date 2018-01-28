using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DischargeController : MonoBehaviour {
    public float dischargePower;
    public PlayerController owner;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void shootRay(float rayDistance)
    {
        float direction = transform.localScale.x * -1;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(direction, 0), rayDistance);

        Debug.DrawRay(transform.position, new Vector2(direction, 0), Color.green, 2.0f);

        // List<RaycastHit2D> filteredHits = new List<RaycastHit2D>();

        for (int i = 0; i < hits.Length; i++)
        {
            GameObject npc = hits[i].collider.gameObject;
            if (npc.layer == LayerMask.NameToLayer("NPC"))
            {
                // filteredHits.Add(hits[i]);
                Debug.Log(hits[i].collider.name);
            }
        }
    }
}
