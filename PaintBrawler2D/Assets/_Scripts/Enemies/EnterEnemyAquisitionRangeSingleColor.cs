using UnityEngine;
using System.Collections;

public class EnterEnemyAquisitionRangeSingleColor : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private EnemyScriptSingleColor _parentScript;

    void Start() {
        _parentScript = _parent.GetComponent<EnemyScriptSingleColor>();
    }

	void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            if (_parentScript._currentlyAttacking == false) { 
                _parentScript.AddToTargetList(other.gameObject);
                _parentScript.SawPlayer();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _parentScript.RemoveFromTargetList(other.gameObject);
            if (_parentScript._aquiredTargets.Count > 0) { 
                _parentScript._currentState = EnemyScriptSingleColor.EnemyState.chasing;
            }
            else
            {
                _parentScript._currentState = EnemyScriptSingleColor.EnemyState.idle;
            }
        }
    }
}
