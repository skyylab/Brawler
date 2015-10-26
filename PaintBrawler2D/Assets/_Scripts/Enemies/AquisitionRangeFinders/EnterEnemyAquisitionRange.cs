using UnityEngine;
using System.Collections;

public class EnterEnemyAquisitionRange : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private EnemyScript _parentScript;

    void Awake() {
        _parentScript = _parent.GetComponent<EnemyScript>();
    }

	void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (_parentScript._currentlyAttacking == false) { 
                _parentScript.AddToTargetList(other.gameObject);
                _parentScript.SawPlayer();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _parentScript.RemoveFromTargetList(other.gameObject);
            if (_parentScript._aquiredTargets.Count > 0) { 
                _parentScript._currentState = EnemyScript.EnemyState.chasing;
            }
            else
            {
                _parentScript._currentState = EnemyScript.EnemyState.idle;
            }
        }
    }
}
