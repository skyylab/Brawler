using UnityEngine;
using System.Collections;

public class EnterEnemyCircleRange : MonoBehaviour {

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
        _timerWaitBeforeTryAgain -= Time.deltaTime;

        _target = _parentScript._aquiredTargets[0];

        if (_inAttackRange && _timerWaitBeforeTryAgain < 0)
        {
            if (_target.GetComponent<HeroScript>().AddAttacker(_parent))
            {
                _parentScript._currentState = EnemyScript.EnemyState.attacking;
            }
            else
            {
                Debug.Log("HEYHEY");
                _parentScript._currentState = EnemyScript.EnemyState.circling;
                _parentScript.RandomCirclePoint = _parentScript.GetRandomPointCircle(Random.Range(0f, 360f), 10f); ;
                _timerWaitBeforeTryAgain = _timerReset;
            }
        }

        if (_avoidEnemy != null && _parentScript._currentState != EnemyScript.EnemyState.attacking) {
            _parentScript.Avoid(_avoidEnemy);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_parentScript._currentlyAttacking == false){
                _parentScript.ReachedCirclingDistance();
            }
            
            _inAttackRange = true;
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
