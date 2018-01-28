using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour {

    public AudioClip MusicClip;
    public AudioClip MusicClip2;
    public AudioSource MusicSource;

	// Use this for initialization
	void Start () {
        MusicSource.clip = MusicClip;
        MusicSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MusicSource.clip = MusicClip2;
            MusicSource.Play();
        }
	}
}
