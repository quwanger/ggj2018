using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip[] NPCCoughs;
    public AudioClip[] playerCoughs;
    public AudioClip[] playerSneezes;


    public AudioSource[] sources;

    AudioSource sFX;
    AudioSource soundtrack;

	// Use this for initialization
	void Start () {
        sources = GetComponents<AudioSource>();
        sFX = sources[0];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(string word)
    {
        

        switch (word)
        {
            case "player coughs":
                sFX.PlayOneShot(playerCoughs[Random.Range(0, playerCoughs.Length)]);
                break;
            case "npc coughs":
                sFX.PlayOneShot(NPCCoughs[Random.Range(0, NPCCoughs.Length)]);
                break;
            case "sneezes":
                sFX.PlayOneShot(playerSneezes[Random.Range(0, playerSneezes.Length)]);
                break;
        }
    }
}
