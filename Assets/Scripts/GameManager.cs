using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    public NPCManager NPCManager;
    public MapManager MapManager;

    public PlayerController[] playerPrefabs;

    public static int numPlayers = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start () {
        List<PlayerController> players = new List<PlayerController>();

        // to-do should be dynamic based on active players
        for(int i = 0; i < numPlayers; i++) {
           players.Add(Instantiate(playerPrefabs[i], transform.position, transform.rotation));
        }

    }

}
