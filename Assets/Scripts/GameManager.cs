using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public PlayerController[] playerPrefabs;

    // Use this for initialization
    void Start () {
        List<PlayerController> players = new List<PlayerController>();

        // to-do should be dynamic based on active players
        int numOfPlayers = playerPrefabs.Length;
        for(int i = 0; i < numOfPlayers; i++) {
           players.Add(Instantiate(playerPrefabs[i], transform.position, transform.rotation));
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
