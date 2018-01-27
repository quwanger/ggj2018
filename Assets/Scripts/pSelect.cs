using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class pSelect : MonoBehaviour {


    public static int[] playersJoined = new int[] { 0, 0, 0, 0 };
    //public static int[] playerIndex = new int[] { 3, 2, 1, 4 };

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        

        if (Input.GetButtonDown(string.Concat("A_", 1)))
        {
            Debug.Log("Player1 Joined!");
            playersJoined[0] = 1;
        }
        if (Input.GetButtonDown(string.Concat("A_", 2)))
        {
            Debug.Log("Player2 Joined!");
            playersJoined[1] = 1;
        }
        if (Input.GetButtonDown(string.Concat("A_", 3)))
        {
            Debug.Log("Player3 Joined!");
            playersJoined[2] = 1;
        }
        if (Input.GetButtonDown(string.Concat("A_", 4)))
        {
            Debug.Log("Player4 Joined!");
            playersJoined[3] = 1;
        }
        GameManager.numOfPlayers = 0;
        for (int i = 0; i < playersJoined.Length; i++)
        {
   
            if (playersJoined[i] > 0)
            {
                GameManager.numOfPlayers++;
            }
        }

        Debug.Log("We currently have " + GameManager.numOfPlayers + " players");

        if (Input.GetButtonDown(string.Concat("Start_", 1)) || Input.GetButtonDown(string.Concat("Start_", 2)) 
            || Input.GetButtonDown(string.Concat("Start_", 3)) || Input.GetButtonDown(string.Concat("Start_", 4)))
        {

            SceneManager.LoadScene(3);
        }
    }
}
