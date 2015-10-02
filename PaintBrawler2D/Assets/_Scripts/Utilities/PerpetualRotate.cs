using UnityEngine;
using System.Collections;

public class PerpetualRotate : MonoBehaviour {

    [SerializeField]
    private float speed = 0.2f;
	
	// Update is called once per frame
	void Update () {
        transform.localEulerAngles += new Vector3(0f, 0f, 1f) * speed;
	}
}
