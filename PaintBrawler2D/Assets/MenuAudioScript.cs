using UnityEngine;
using System.Collections;

public class MenuAudioScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update() {
        if (Application.loadedLevel > 1)
        {
            Destroy(gameObject);
        }
    }
	
}
