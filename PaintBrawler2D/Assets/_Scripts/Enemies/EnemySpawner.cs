using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private List<GameObject> _allEnemies = new List <GameObject>();

    [SerializeField]
    private int _spawnAmount = 5;
    [SerializeField]
    private int _spawnAmountCounter;
    [SerializeField]
    private float _spawnIntervalTimer = 0.5f;
    private float _spawnIntervalTimerReset;

    [SerializeField]
    private int _difficulty = 0;
    [SerializeField]
    private GameObject[] _enemyList;

    private bool _startSpawning = false;

    public void RemoveObject(GameObject ObjectToRemove)
    {
        _allEnemies.Remove(ObjectToRemove);
    }

    // Use this for initialization
    void Start () {
        _spawnIntervalTimerReset = _spawnIntervalTimer;
        _spawnAmount = _allEnemies.Count;
        _spawnAmountCounter = _spawnAmount;
	}
	
	// Update is called once per frame
	void Update () {
        _spawnIntervalTimer -= Time.deltaTime;

        if (_allEnemies.Count <= 0)
        {
            _startSpawning = true;
        }

        if (_startSpawning)
        {
            if (_spawnIntervalTimer < 0 && _spawnAmount > 0 && _allEnemies.Count < _spawnAmountCounter)
            {
                GameObject NewEnemy = Instantiate(_enemyList[_difficulty], transform.position, Quaternion.identity) as GameObject;
                _allEnemies.Add(NewEnemy);
                _spawnAmount--;
                _spawnIntervalTimer = _spawnIntervalTimerReset;
            }
            else if (_allEnemies.Count >= _spawnAmountCounter)
            {
                _spawnAmountCounter++;
                ResetSpawner();
                _startSpawning = false;
            }
            
        }
        
	}

    void ResetSpawner()
    {
        _spawnAmount = _spawnAmountCounter;
    }
}
