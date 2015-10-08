﻿using UnityEngine;
using System.Collections;

public class EnterRangedEnemyAttackRange : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private EnemyScript _parentScript;


    private GameObject _target;
    private float _timerReset;

    void Start()
    {
        _parentScript = _parent.GetComponent<EnemyScript>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _parentScript.AddToAttackList(other.gameObject);
            _parentScript.StopSpeed();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _parentScript.RemoveFromAttackList(other.gameObject);
            _parentScript.StartSpeed();
        }
    }
}
