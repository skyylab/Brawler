﻿using UnityEngine;
using System.Collections;

public class BrawlerFistStunCollider : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private BrawlerClass _parentScript;

    [SerializeField]
    private GameObject _powPrefab;

    [SerializeField]
    private AudioClip[] _hitSFX;

    private AudioSource _audio;
    void Start() {
        _parentScript = _parent.GetComponent<BrawlerClass>();
        _audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy") {
            other.GetComponent<EnemyScript>().KnockOver();

            Instantiate(_powPrefab, transform.position + transform.right, transform.rotation);
            
            int RandomNumber = Random.Range(0, 3);
            _audio.pitch = Random.Range(0.8f, 1.2f);
            _audio.PlayOneShot(_hitSFX[RandomNumber], 0.3f);
        }
    }
}
