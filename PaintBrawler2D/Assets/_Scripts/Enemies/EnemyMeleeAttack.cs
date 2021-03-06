﻿using UnityEngine;
using System.Collections;

public class EnemyMeleeAttack : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private EnemyScript _parentScript;

    void Start() {
        _parentScript = _parent.GetComponent<EnemyScript>();
    }

	void OnTriggerEnter (Collider other) {
        if (other.tag == "Player" && !_parentScript.GetAttackLanded())
        {
            other.gameObject.GetComponent<HeroScript>().TakeDamage(_parentScript.GetDamage(), _parent);
            _parentScript.SetAttackLanded(true);
        }
    }
}
