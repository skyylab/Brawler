using UnityEngine;
using System.Collections;

public class MegaHitLife : MonoBehaviour {

    private float _life = 0.1f;
	
	// Update is called once per frame
	void Update () {
        _life -= Time.deltaTime;
        if (_life < 0) {
            GetComponent<SpriteRenderer>().enabled = false;
            if (!GetComponent<AudioSource>().isPlaying) {
                Destroy(gameObject);
            }
        }
	}
}
