using UnityEngine;
using System.Collections;

public class EnterRangedEnemyCircleRange : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private EnemyScript _parentScript;

    private GameObject _target;
    private float _timerWaitBeforeTryAgain = 0f;
    private float _timerReset = 2f;
    private bool _inAttackRange = false;


    private GameObject _avoidEnemy;

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
            _avoidEnemy = other.gameObject;
            _target = other.gameObject;
        }

        if (other.tag == "Enemy") {
            _avoidEnemy = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_parentScript._currentlyAttacking == false){
                _parentScript.MovedOutCirclingDistance();
            }
            _target.GetComponent<HeroScript>().RemoveAttacker(_parent);
            _inAttackRange = false;
            _parentScript._currentState = EnemyScript.EnemyState.chasing;
        }

        if (other.tag == "Enemy")
        {
            _avoidEnemy = null;
        }
    }
}
