using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.F1)) {
            Application.LoadLevel(Application.loadedLevel);
        }
	}
}
