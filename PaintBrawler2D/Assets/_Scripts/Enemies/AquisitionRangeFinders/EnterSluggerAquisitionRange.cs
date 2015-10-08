using UnityEngine;
using System.Collections;

public class EnterSluggerAquisitionRange : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private EnemyScript _parentScript;

    void Start() {
        _parentScript = _parent.GetComponent<EnemyScript>();
    }

	void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            if (_parentScript._currentlyAttacking == false) { 
                _parentScript.AddToTargetList(other.gameObject);
                _parentScript.SawPlayer();
            }
        }
    }
}
