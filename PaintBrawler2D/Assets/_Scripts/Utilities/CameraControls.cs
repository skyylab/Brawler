using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControls : MonoBehaviour {

    [SerializeField]
    private List<GameObject> Players = new List<GameObject>();

    public void RemovePlayers(GameObject Player) {
        Players.Remove(Player);
    }

    public void AddPlayers (GameObject Player) {
        Players.Add(Player);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 NewCamPosition = FindCenterPoint();
        NewCamPosition.z = -12f;
        transform.position = Vector3.Slerp(transform.position, NewCamPosition, Time.deltaTime);
    }

    Vector3 FindCenterPoint() {
     if (Players.Count == 0)
         return Vector3.zero;
     if (Players.Count == 1)
         return Players[0].transform.position;
     Bounds bounds = new Bounds(Players[0].transform.position, Vector3.zero);
     for (var i = 1; i<Players.Count; i++)
         bounds.Encapsulate(Players[i].transform.position); 
     return bounds.center;
 }
}
