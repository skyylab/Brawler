using UnityEngine;
using System.Collections;

public class EnterRangedEnemyCircleRange : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private EnemyScript _parentScript;

    private GameObject _target;

    void Start()
    {
        _parentScript = _parent.GetComponent<EnemyScript>();
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _target = other.gameObject;
            if (_parentScript._currentState != EnemyScript.EnemyState.chasing)
            {
                _parentScript.SetFleeTarget(_target);
                _parent.GetComponent<PrimaryColorRangedEnemy>().ResetFleeTimer();
                _parentScript._currentState = EnemyScript.EnemyState.fleeing;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_parentScript._currentlyAttacking == false) {
                _parentScript.MovedOutCirclingDistance();
            }
            _target.GetComponent<HeroScript>().RemoveAttacker(_parent);
            
        }
    }
}
