using UnityEngine;
using System.Collections;

public class RangedUnitAttackSight : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private EnemyScript _parentScript;

    void Start()
    {
        _parentScript = _parent.GetComponent<EnemyScript>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _parentScript.AddToAttackList(other.gameObject);
            _parentScript.StopSpeed();
            _parentScript._currentState = EnemyScript.EnemyState.attacking;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _parentScript.RemoveFromAttackList(other.gameObject);
            _parentScript.StartSpeed();

            if (_parentScript.AttackListSize() <= 0)
            {
                _parentScript._currentState = EnemyScript.EnemyState.chasing;
            }
        }
    }
}
