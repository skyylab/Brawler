using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrimaryColorEnemy : EnemyScript {

    private float _timerTagEnemySwitch = 10f;
    private float _timerTagEnemySwitchReset;

    private float _fleeTimer = 5f;

    void Start()
    {
        _timerTagEnemySwitchReset = _timerTagEnemySwitch;
        _fleeTimer = Random.Range(4f, 7f);
    }

    public override void Update()
    {
        base.Update();
        if (_stunDuration < 0)
        {
            ManageRayCast();
            ManageEnemyState();
            ManageSpriteOrientation();
        }
        else
        {
            // PlayStunAnimation
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") ||
            _animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage")) {
            _moveSpeedActual = 0f;
        }
        else {
            _moveSpeedActual = _moveSpeed;
        }
    }

    public override void Idle() {
    }

    public override void SawPlayer() {
        _currentState = EnemyState.chasing;
    }

    public override void Chasing() {
        base.Chasing();
        _attackRange.SetActive(true);
        if (_aquiredTargets.Count > 0)
        {
            _lastPosition = transform.position;
            Vector3 move = Vector3.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual);
            move.y = 0f;
            transform.position = move;
        }
    }

    public override void Circling() {
        if (Vector3.Distance(transform.position, _aquiredTargets[0].transform.position) < 6f && _avoidEnemy != null) {

            Vector3 move = Vector3.MoveTowards(transform.position, _avoidEnemy.transform.position, Time.deltaTime * -_moveSpeedActual);
            move.y = 0f;
            transform.position = move;
        }
        else {
            _currentState = EnemyState.chasing;
        }

        if (_aquiredTargets[0].GetComponent<HeroScript>().AddAttacker(gameObject)) {
            _currentState = EnemyState.attacking;
        }
    }

    public override void Attacking() {
        _currentlyAttacking = true;
        _lastPosition = transform.position;
        Vector3 move = Vector3.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual * 1.5f);
        move.y = 0f;
        transform.position = move;

        _timerTagEnemySwitch -= Time.deltaTime;

        if (_timerTagEnemySwitch < 0) {
            _currentlyAttacking = false;
            _timerTagEnemySwitch = _timerTagEnemySwitchReset;
            _aquiredTargets[0].GetComponent<HeroScript>().RemoveAttacker(gameObject);
            _currentState = EnemyState.fleeing;
        }

        ManageAttack();
    }

    public override void Fleeing() {
        _fleeTimer -= Time.deltaTime;

        if (_fleeTimer < 0) {
            _fleeTimer = Random.Range(4f, 7f);
            _currentState = EnemyState.circling;
        }
        transform.position = Vector3.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * -_moveSpeedActual);
    }

    public void ManageAttack() {

        _coolDown -= Time.deltaTime;

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
            transform.position = Vector3.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual);
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
        _coolDown = _coolDownSet;
    }

    public override void Fallen()
    {
        base.Fallen();
        _attackRange.SetActive(false);
    }

}
