using UnityEngine;
using System.Collections;

public class AnimateOnce : MonoBehaviour {

    [SerializeField]
    private Sprite[] _spriteArray;
    [SerializeField]
    private float _iteratorSpeed = 0.05f;
    private float _iteratorSpeedReset;

    private int _iterator = 0;

	// Use this for initialization
	void Start () {
        _iteratorSpeedReset = _iteratorSpeed;
	}
	
	// Update is called once per frame
	void Update () {

        _iteratorSpeed -= Time.deltaTime;

	    if (_iteratorSpeed < 0) {
            GetComponent<SpriteRenderer>().sprite = _spriteArray[_iterator];
            _iteratorSpeed = _iteratorSpeedReset;

            if (_iterator < _spriteArray.Length - 1) {
                _iterator++;
            }
        }
	}
}
