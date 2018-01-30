using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour {

    public List<SpawnerController> NpcSpawners = new List<SpawnerController>();
    public List<NpcController> AllNpcs = new List<NpcController>();
    [Space(5)]
    [Header("Spawner Properties")]
    [SerializeField]
    public bool randomNpcSpeed;
    [SerializeField]
    public float minNpcSpeed;
    [SerializeField]
    public float maxNpcSpeed;

    [Space(5)]
    [Header("Spawner Properties")]
    [SerializeField]
    public float rateLowerLimit = 3;
    [SerializeField]
    public float rateUpperLimit = 5;
    [SerializeField]
    public float rateOffset = 0; //for flash sales - add this to lower and upper rate to increase or decrease spawn rate 
    [SerializeField]
    public float startingRateOffset = 0;
    [SerializeField]
    public int burstLowerLimit = 1;
    [SerializeField]
    public int burstUpperLimit = 10;

    public void SpawnNPCsForSale()
    {

    }

    public void SendNPCsToSale(int saleStrength, Transform store)
    {
        //sale strength will be out of 10 (10 meaning players have a 50% chance of changing direction to the sale
        foreach(NpcController npc in AllNpcs)
        {
            npc.SendToSale(store);
            int chanceOfGoingToSale = Random.Range(1, 21);
            if(chanceOfGoingToSale <= saleStrength)
            {
                //send them to the sale!
                npc.SendToSale(store);
            }
        }
    }

    public void KillNPCsAtStore(Transform store)
    {
        //Debug.Log("Killing NPCs");

        List<NpcController> npcsToKill = new List<NpcController>();

        foreach (NpcController npc in AllNpcs)
        {
            if(Vector2.Distance(new Vector2(store.position.x, store.position.y), new Vector2(npc.transform.position.x, npc.transform.position.y)) < 2.4f
                && npc._npcState != NpcController.NPCState.RidingEscalator
                && store.position.y > npc.transform.position.y)
            {
                npcsToKill.Add(npc);
            }
        }

        foreach (NpcController npc in npcsToKill)
        {
            //Debug.Log("Killing NPC");
            npc.ExitNPC(true);
        }
    }
}
