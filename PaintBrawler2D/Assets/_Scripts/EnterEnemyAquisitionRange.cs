using UnityEngine;
using System.Collections;

public class EnterEnemyAquisitionRange : MonoBehaviour {

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

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        _parentScript.RemoveFromTargetList(other.gameObject);
    //    }
    //}
}
