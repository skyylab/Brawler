using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {

    [SerializeField]
    private GameObject MainCamera;
	
	// Update is called once per frame
	void Update () {
        transform.position = MainCamera.transform.position;
	}
}
