using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControls : MonoBehaviour {

    [SerializeField]
    private List<GameObject> Players = new List<GameObject>();

    [SerializeField]
    private bool _cameraLock = false;

    [SerializeField]
    private int EnemyCountOnScreen;

    public void RemovePlayers(GameObject Player) {
        Players.Remove(Player);
    }

    public void AddPlayers (GameObject Player) {
        Players.Add(Player);
    }

	// Use this for initialization
	void Start () {
        GameObject[] PlayerList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject x in PlayerList)
        {
            Players.Add(x);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!_cameraLock)
        {
            Vector3 NewCamPosition = FindCenterPoint();
            NewCamPosition.z = -12f;
            transform.position = Vector3.Slerp(transform.position, NewCamPosition, Time.deltaTime);
        }
        else
        {
            Vector3 NewCamPosition = FindCenterPoint();
            NewCamPosition.z = -12f;
            NewCamPosition.x = transform.position.x;
            transform.position = Vector3.Slerp(transform.position, NewCamPosition, Time.deltaTime);
        }

        if (EnemyCountOnScreen == 0)
        {
            _cameraLock = false;
        }
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "CameraPoint")
        {
            _cameraLock = true;
            other.gameObject.GetComponent<EnemyHolder>().ActivateEnemies();
            EnemyCountOnScreen = GameObject.FindGameObjectsWithTag("Enemy").Length;
            other.gameObject.SetActive(false);
        }
    }

    public void EnemyKilled()
    {
        EnemyCountOnScreen--;
    }
}
