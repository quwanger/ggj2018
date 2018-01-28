using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScore : MonoBehaviour {

    public Text player1;
    public Text player2;
    public Text player3;
    public Text player4;

    GameManager gameManager;

	// Use this for initialization
	void Start () {
        player1.text = "Score: " + GameManager.Instance.PlayerScores[0];
        player2.text = "Score: " + GameManager.Instance.PlayerScores[1];
        player3.text = "Score: " + GameManager.Instance.PlayerScores[2];
        player4.text = "Score: " + GameManager.Instance.PlayerScores[3];


    }
	
	// Update is called once per frame
	void Update () {
	}
}
