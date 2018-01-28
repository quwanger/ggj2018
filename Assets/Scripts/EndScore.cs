using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScore : MonoBehaviour {

    public Text[] player;

    GameManager gameManager;

    // Use this for initialization
    void Start() {


        for (int i = 0; i < GameManager.Instance.PlayerScores.Count; i++)
        {
            player[i].text = "Score: " + GameManager.Instance.PlayerScores[i+1].ToString(); ;
        }

        


    }
	
	// Update is called once per frame
	void Update () {
	}
}
