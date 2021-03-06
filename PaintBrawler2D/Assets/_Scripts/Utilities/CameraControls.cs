﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Giverspace;

public class CameraControls : MonoBehaviour {

    [SerializeField]
    private List<GameObject> Players = new List<GameObject>();

    [SerializeField]
    private bool _cameraLock = false;

    [SerializeField]
    private int EnemyCountOnScreen;

    [SerializeField]
    private GameObject _restartScreen;
    private float _restartCountdown = 2f;
    private bool _restartDisplayOn = false;

    private float _yAxisOffset = 0f;
    private float _zAxisOffset = 0f;

    private int playerCount = 3;

    private int _sectionCount = 1;

    public void RemovePlayers(GameObject Player) {
        playerCount--;
    }

    public void AddPlayers (GameObject Player) {
        playerCount++;
    }

    public int PlayerCount() {
        return playerCount;
    }

    public bool RestartDisplayed() {
        return _restartDisplayOn;
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

        if(PlayerCount() <= 0) {
            _restartCountdown -= Time.deltaTime;
            if (_restartCountdown < 0) {
                _restartScreen.SetActive(true);
            }

            if (_restartCountdown < -0.2f) {
                _restartDisplayOn = true;
            }
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
            Log.Metrics.Message("Entered Section " + _sectionCount.ToString());
            _sectionCount++;
        }
    }

    public void EnemyKilled()
    {
        EnemyCountOnScreen--;
    }
}
