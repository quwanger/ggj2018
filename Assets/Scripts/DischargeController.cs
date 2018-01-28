using System.Collections;
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

        Debug.DrawRay(transform.position, new Vector2(direction*rayDistance, 0), Color.green, 1.0f);

        // List<RaycastHit2D> filteredHits = new List<RaycastHit2D>();

        for (int i = 0; i < hits.Length; i++)
        {
            GameObject obj = hits[i].collider.gameObject;

            if(type == "cough")
            {
                if (obj.layer == LayerMask.NameToLayer("NPC"))
                {
                    NpcController npcScript = obj.GetComponent<NpcController>();
                    npcScript.hitByCough(dischargeLevel, owner);
                    // filteredHits.Add(hits[i]);
                }
            } else
            {
                if (obj.layer == LayerMask.NameToLayer("NPC") || 
                    obj.layer == LayerMask.NameToLayer("Player"))
                {
                    EntityController entity = obj.GetComponent<EntityController>();
                    entity.GetComponent<Rigidbody2D>().AddForce(transform.parent.localScale * -1000.0f);
                    // filteredHits.Add(hits[i]);
                }
            }
           
        }
    }
}
