using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.F1)) {
            Application.LoadLevel(Application.loadedLevel);
        }

        if (Input.GetKeyDown(KeyCode.F2)) {
            Application.LoadLevel(0);
        }
	}
}
