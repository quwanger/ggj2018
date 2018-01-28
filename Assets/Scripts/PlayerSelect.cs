using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour {
    private int players;
    private bool[] alreadyEntered = new bool[4];


    // Use this for initialization
    void Start () {
        for (int i = 0; i < alreadyEntered.Length; i++)
        {
            alreadyEntered[i] = false;
        }
	}

    // Update is called once per frame
    void Update() {
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetButtonDown("A_" + i) && alreadyEntered[i] == false)
            {
                players++;

                Debug.Log("Number of players entered: " + players);

                alreadyEntered[i] = true;
            }
        }

        if (Input.GetButtonDown("Start_1") || Input.GetButtonDown("Start_2") || Input.GetButtonDown("Start_3") || Input.GetButtonDown("Start_4"))
        {
            GameManager.numPlayers = players;
            SceneManager.LoadScene(2);
        }

    }
}
