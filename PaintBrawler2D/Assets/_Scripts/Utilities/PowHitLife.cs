using UnityEngine;
using System.Collections;

public class PowHitLife : MonoBehaviour {

    private float _life;

    [SerializeField]
    private ParticleSystem _particleSystem;
	
    void Start() {
        _life = _particleSystem.startLifetime;
    }

	// Update is called once per frame
	void Update () {
        _life -= Time.deltaTime;
        if (_life < 0) {
            Destroy(gameObject);
        }
	}
}
