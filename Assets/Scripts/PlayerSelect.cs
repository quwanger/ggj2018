using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour {
    private int players;
    private bool[] alreadyEntered = new bool[4];
        
    public SpriteRenderer[] playerSprites;

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
            

            if (Input.GetButtonDown("A_" + i) && alreadyEntered[i-1] == false)
            {
                players++;

                if (i == 1)
                {
                    GameManager.playerSprites.Add("red");
                } else if (i == 2)
                {
                    GameManager.playerSprites.Add("green");
                }
                else if (i == 3)
                {
                    GameManager.playerSprites.Add("blue");
                }
                else if (i == 4)
                {
                    GameManager.playerSprites.Add("yellow");
                }

                GameManager.numPlayers = i;

                playerSprites[i-1].enabled = true;

                alreadyEntered[i-1] = true;
            }
        }

        if (Input.GetButtonDown("Start_1") || Input.GetButtonDown("Start_2") || Input.GetButtonDown("Start_3") || Input.GetButtonDown("Start_4"))
        {
            GameManager.numPlayers = players;
            for (int i = 0; i < FindObjectsOfType<AudioSource>().Length; i++)
            {
                FindObjectsOfType<AudioSource>()[i].Stop();
            }

            SceneManager.LoadScene(3);
        }

    }
}
