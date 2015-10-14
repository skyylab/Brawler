using UnityEngine;
using System.Collections;

public class AnimatedSquigglyLines : MonoBehaviour {

    [SerializeField]
    private Sprite[] _allSpriteLoop;
    [SerializeField]
    private float _changeInterval = 0.15f;
    private float _changeIntervalReset;
    private int _iterator = 0;

	// Use this for initialization
	void Start () {
        _changeIntervalReset = _changeInterval;
	}
	
	// Update is called once per frame
	void Update () {

        _changeInterval -= Time.deltaTime;

	    if (_changeInterval < 0) {
            GetComponent<SpriteRenderer>().sprite = _allSpriteLoop[_iterator];
            _changeInterval = _changeIntervalReset;
        }

        _iterator++;

        if (_iterator >= _allSpriteLoop.Length) {
            _iterator = 0;
        }
	}
}
