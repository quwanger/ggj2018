using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    public NPCManager NPCManager;
    public MapManager MapManager;

    public List<GameObject> spawnPoints = new List<GameObject>();

    public PlayerController[] playerPrefabs;

    public static int numPlayers = 0;
    public static float gameTime = 210;
    public static List<string> playerSprites = new List<string>();

    public Text countdownText;
    public float countdownFrom;

    public bool gameStarted;
    bool isTimerStarted = false;

    public Dictionary<int, int> PlayerScores = new Dictionary<int, int>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start () {
        gameStarted = false;

        List<PlayerController> players = new List<PlayerController>();

        for(int i = 0; i < numPlayers; i++) {
            //Debug.Log(playerPrefabs[i].name);

            //Debug.Log(playerSprites[i]);

            if (playerSprites[i] == "red")
            {
                //Debug.Log("red");

                PlayerController playerName = playerPrefabs[0];

                if (playerName.name == "Player")
                {
                    players.Add(Instantiate(playerName, spawnPoints[0].transform.position, spawnPoints[0].transform.rotation));
                    PlayerScores.Add(playerName.playerId, 0);
                }
            }

            if (playerSprites[i] == "green")
            {
                //Debug.Log("green");

                PlayerController playerName = playerPrefabs[1];

                if (playerName.name == "Player2")
                {
                    players.Add(Instantiate(playerName, spawnPoints[1].transform.position, spawnPoints[1].transform.rotation));
                    PlayerScores.Add(playerName.playerId, 0);
                }
            }

            if (playerSprites[i] == "blue")
            {

                PlayerController playerName = playerPrefabs[2];

                if (playerName.name == "Player3")
                {
                    players.Add(Instantiate(playerName, spawnPoints[2].transform.position, spawnPoints[2].transform.rotation));
                    PlayerScores.Add(playerName.playerId, 0);
                }
            }

            if (playerSprites[i] == "yellow")
            {

                PlayerController playerName = playerPrefabs[3];

                if (playerName.name == "Player4")
                {
                    players.Add(Instantiate(playerName, spawnPoints[3].transform.position, spawnPoints[3].transform.rotation));
                    PlayerScores.Add(playerName.playerId, 0);
                }
            }

        }
    }

    void Update()
    {
        float time = countdownFrom - Time.timeSinceLevelLoad;

        if (time <= 3.0f && time > 2.0f)
        {
            countdownText.text = "3";
        } else if (time <= 2.0f && time > 1.0f)
        {
            countdownText.text = "2";
        } else if (time <= 1.0f && time > 0.0f)
        {
            countdownText.text = "1";
        } else if (time <= 0.0f)
        {
            // countdownText.text = "GO";
            gameStarted = true;
            if(!isTimerStarted)
            {
                StartCoroutine(GetWorldTime());
                isTimerStarted = true;
            }
        }
    }

    float currCountdownValue = GameManager.gameTime;
    public IEnumerator GetWorldTime()
    {
        //currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            GameManager.gameTime = currCountdownValue;
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
    }

}
