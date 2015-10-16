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

    private float _yAxisOffset = 0f;
    private float _zAxisOffset = 0f;

    public void RemovePlayers(GameObject Player) {
        Players.Remove(Player);
    }

    public void AddPlayers (GameObject Player) {
        Players.Add(Player);
    }

    public void SetCameraLock(bool Set) { _cameraLock = Set; }

	// Use this for initialization
	void Start () {
        GameObject[] PlayerList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject x in PlayerList)
        {
            Players.Add(x);
        }

        _yAxisOffset = transform.position.y;
        _zAxisOffset = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
        if (!_cameraLock)
        {
            Vector3 NewCamPosition = FindCenterPoint();
            NewCamPosition.y = _yAxisOffset;
            NewCamPosition.z += _zAxisOffset;
            transform.position = Vector3.Slerp(transform.position, NewCamPosition, Time.deltaTime * 2);
        }
        else
        {
            Vector3 NewCamPosition = FindCenterPoint();
            NewCamPosition.y = _yAxisOffset;
            NewCamPosition.z += _zAxisOffset;
            NewCamPosition.x = transform.position.x;
            transform.position = Vector3.Slerp(transform.position, NewCamPosition, Time.deltaTime * 2);
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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CameraPoint")
        {
            _cameraLock = true;
            other.gameObject.GetComponent<EnemyHolder>().ActivateEnemies();
            other.gameObject.SetActive(false);
        }
    }

    public void EnemyKilled()
    {
        EnemyCountOnScreen--;
    }
}
