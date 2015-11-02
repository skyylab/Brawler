using UnityEngine;
using System.Collections;

public class PersistentCharacterSelectDummy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    if (GameObject.Find("PersistentCharSelect")) {
            Destroy(gameObject);
        }
	}
}
