using UnityEngine;
using System.Collections;

public class BasicEnemy : EnemyScript {
    
    int RandomNumber = 0;
    float RandomInterval = 0f;

    void Start()
    {
        InitializeClass();
    }

    void Update()
    {
        ManageRayCast();
        ManageEnemyState();
        ManageSpriteOrientation();

        if (_hitPoints < 0)
        {
            // Die
            Destroy(gameObject);
        }
    }

    public override void Idle() {
    }
    public override void SawPlayer() {
        _currentState = EnemyState.chasing;
    }
    public override void Chasing() {
        if (_aquiredTargets.Count > 0)
        {
            _lastPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeed);
        }
    }
    public override void Circling() {

        RandomInterval -= Time.deltaTime;
        if (RandomInterval > 0) {
            transform.position += Vector3.up * RandomNumber * Time.deltaTime * _moveSpeed;
        }
        else {
            RandomNumber = Random.Range(-1, 3);
            RandomInterval = Random.Range(3f, 5f);
            if (RandomNumber == 2) {
                _currentState = EnemyState.attacking;
            }
        }
    }
    public override void Attacking() {
        _currentlyAttacking = true;
        _lastPosition = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeed);
        ManageAttack();
    }
    public override void Fleeing() {
    }

    public void ManageAttack() {

        _coolDown -= Time.deltaTime;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            _objectSprites[1].GetComponent<BoxCollider2D>().enabled = false;
            _objectSprites[2].GetComponent<BoxCollider2D>().enabled = false;
        }

        if (_inAttackRange && _coolDown < 0)
        {
            Attack();
        }
    }

    public override void ManageMovement()
    {
        if (_aquiredTargets.Count > 0)
        {
            _lastPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual);
        }

        float moveDirection = transform.position.x - _lastPosition.x;
        if (moveDirection > 0)
        {
            _objectWholeSprite.transform.localEulerAngles = new Vector2(0f, 0f);
        }
        else if (moveDirection < 0)
        {
            _objectWholeSprite.transform.localEulerAngles = new Vector2(0f, 180f);
        }
    }

    public override void Attack()
    {
        _attackLanded = false;
        _animator.CrossFade("Attack", 0.01f);
        _objectSprites[1].GetComponent<BoxCollider2D>().enabled = true;
        _objectSprites[2].GetComponent<BoxCollider2D>().enabled = true;
        _coolDown = _coolDownSet;
    }

    protected void ManageEnemyState()
    {
        switch (_currentState)
        {
            case EnemyState.initializing:
                _currentState = EnemyState.idle;
                break;
            case EnemyState.idle:
                Idle();
                break;
            case EnemyState.sawPlayer:
                SawPlayer();
                break;
            case EnemyState.chasing:
                Chasing();
                break;
            case EnemyState.circling:
                Circling();
                break;
            case EnemyState.attacking:
                Attacking();
                break;
            case EnemyState.fleeing:
                Fleeing();
                break;
            default:
                break;
        }
    }
}
