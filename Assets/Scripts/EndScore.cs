using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScore : MonoBehaviour {

    GameManager gameManager;

	// Use this for initialization
	void Start () {
		//gameManager.GetComponent(player)
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(GameManager.Instance.PlayerScores[1]);
	}
}
