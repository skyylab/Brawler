using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlinkingScript : MonoBehaviour {

    [SerializeField]
    private float _blinkInterval = 0.5f;
    private float _blinkIntervalReset;

	// Use this for initialization
	void Start () {
        _blinkIntervalReset = _blinkInterval;
	}
	
	// Update is called once per frame
	void Update () {
        _blinkInterval -= Time.deltaTime;

	    if (_blinkInterval < 0) {
            if (GetComponent<Text>().enabled == true) {
                GetComponent<Text>().enabled = false;
            }
            else {
                GetComponent<Text>().enabled = true;
            }
            _blinkInterval = _blinkIntervalReset;
        }
	}
}
