using UnityEngine;
using System.Collections;

public class BeeStinger : Finisher {
    
    [SerializeField]
    private AudioClip[] _hit;
    [SerializeField]
    private GameObject _hitParticle;

	// Use this for initialization
	void Start () {
        _parentScript = _parent.GetComponent<HeroScript>();
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag == "FinishingCollider") {
            GetComponent<AudioSource>().PlayOneShot(_hit[Random.Range(0, _hit.Length - 1)], 0.3f);
            Instantiate(_hitParticle, transform.position, transform.rotation);
        }
    }
}
