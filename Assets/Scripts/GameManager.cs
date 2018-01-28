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
    public static float gameTime = 120;

    public Text countdownText;
    public float countdownFrom;

    public bool gameStarted;

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
            players.Add(Instantiate(playerPrefabs[i], spawnPoints[i].transform.position, spawnPoints[i].transform.rotation));
        }
    }


    bool isTimerStarted = false;
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
            Debug.Log("Countdown: " + currCountdownValue);
            GameManager.gameTime = currCountdownValue;
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;

        }
    }

}
