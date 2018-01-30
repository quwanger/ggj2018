using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTime : MonoBehaviour {

    public GameObject currSpriteBG;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // if(currSpriteBG.name)


        string myCurrStore = currSpriteBG.GetComponent<SpriteRenderer>().sprite.name;

        if(myCurrStore.Equals("store_info"))
        {
            float timer = GameManager.Instance.gameTime;

            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            //this.GetComponent<Text>().text = niceTime;
            this.GetComponent<TextMesh>().text = niceTime;
            
        }
        else
        {
            this.GetComponent<TextMesh>().text = "";
        }

	}
}
