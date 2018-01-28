using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    static AudioManager instance = null;

    public AudioClip[] coughs;

    public AudioSource[] sources;
    AudioSource soundtrack;

    public GameObject audio;

    void Awake ()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            print("Duplicate music player self-destructing");
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        sources = GetComponents<AudioSource>();
        soundtrack = sources[0];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(string word)
    {
        /*switch (word)
        {
            case "FUCK":
                soundEffects.PlayOneShot(fuck[Random.Range(0, fuck.Length)]);
                break;
            default:
                soundEffects.PlayOneShot(grunts[Random.Range(0, grunts.Length)]);
                break;
        }*/
    }
}
