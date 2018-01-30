using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour {
    public static AudioScript instance = null;

    public AudioClip MusicClip;
    //public AudioClip MusicClip2;
    public AudioSource MusicSource;

    void Awake()
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
        //MusicSource.clip = MusicClip;
        //MusicSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //MusicSource.clip = MusicClip2;
            MusicSource.Play();
        }
	}
}
