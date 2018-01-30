using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    static AudioManager instance = null;

    public AudioClip[] NPCCoughs;
    public AudioClip[] playerCoughs;
    public AudioClip[] playerSneezes;
    public AudioClip[] everything;
    public AudioClip[] blackFriday;
    public AudioClip[] clearance;
    public AudioClip[] endOfSeason;
    public AudioClip[] fireSale;
    public AudioClip[] liquidation;
    public AudioClip[] obama;
    public AudioClip drilling;
    public AudioClip swoosh;
    public AudioClip cashRegister;


    public AudioSource[] sources;

    AudioSource sFX;
    AudioSource soundtrack;
    AudioSource intercom;

    public GameObject audio;

    void Awake ()
    {
       /* if (instance != null)
        {
            Destroy(gameObject);
            print("Duplicate music player self-destructing");
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }*/
    }

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
                Debug.Log(playerCoughs.Length);
                sFX.PlayOneShot(playerCoughs[Random.Range(0, playerCoughs.Length)]);
                break;
            case "npc coughs":
                sFX.PlayOneShot(NPCCoughs[Random.Range(0, NPCCoughs.Length)]);
                break;
            case "sneezes":
                sFX.PlayOneShot(playerSneezes[Random.Range(0, playerSneezes.Length)]);
                break;
            case "everything":
                sFX.PlayOneShot(everything[Random.Range(0, everything.Length)]);
                break;
            case "black friday":
                sFX.PlayOneShot(blackFriday[Random.Range(0, blackFriday.Length)]);
                break;
            case "clearance":
                sFX.PlayOneShot(clearance[Random.Range(0, clearance.Length)]);
                break;
            case "end of season":
                sFX.PlayOneShot(endOfSeason[Random.Range(0, endOfSeason.Length)]);
                break;
            case "fire sale":
                sFX.PlayOneShot(fireSale[Random.Range(0, fireSale.Length)]);
                break;
            case "liquidation":
                sFX.PlayOneShot(liquidation[Random.Range(0, liquidation.Length)]);
                break;
            case "obama":
                sFX.PlayOneShot(obama[Random.Range(0, obama.Length)]);
                break;
            case "drilling":
                sFX.PlayOneShot(drilling);
                break;
            case "swoosh":
                sFX.PlayOneShot(swoosh);
                break;
            case "cash Register":
                sFX.PlayOneShot(cashRegister);
                break;
        }
    }
}
