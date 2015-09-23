using UnityEngine;
using System.Collections;

public class PowHitLife : MonoBehaviour {

    private float _life = 0.1f;
	
	// Update is called once per frame
	void Update () {
        _life -= Time.deltaTime;
        if (_life < 0) {
            Destroy(gameObject);
        }
	}
}
