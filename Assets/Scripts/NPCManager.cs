using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour {

    public List<SpawnerController> NpcSpawners = new List<SpawnerController>();
    public List<NpcController> AllNpcs = new List<NpcController>();

    public void SpawnNPCsForSale()
    {

    }

    public void SendNPCsToSale(int saleStrength, Transform store)
    {
        //sale strength will be out of 10 (10 meaning players have a 50% chance of changing direction to the sale
        foreach(NpcController npc in AllNpcs)
        {
            npc.SendToSale(store);
            int chanceOfGoingToSale = Random.Range(1, 14);
            if(chanceOfGoingToSale <= saleStrength)
            {
                //send them to the sale!
                npc.SendToSale(store);
            }
        }
    }
}
