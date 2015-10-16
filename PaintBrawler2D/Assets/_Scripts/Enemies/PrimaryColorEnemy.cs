using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrimaryColorEnemy : EnemyScript {
    
    [SerializeField]
    private GameObject Spawner;

    void Start()
    {
        InitializeClass();
        _particleGenerator.GetComponent<ParticleSystem>().emissionRate = 0;
        Spawner = GameObject.Find("EnemySpawner");
        RandomCirclePoint = Random.insideUnitCircle;
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
            transform.position = Vector3.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual);
        }
    }

    public override void Circling() {
        //transform.position = Vector3.MoveTowards(transform.position, RandomCirclePoint + (Vector2)_aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual);
        _currentState = EnemyState.attacking;
    }

    public override void Attacking() {
        _currentlyAttacking = true;
        _lastPosition = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual * 1.5f);
        ManageAttack();
    }

    public override void Fleeing() {
    }

    public void ManageAttack() {

        _coolDown -= Time.deltaTime;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            _objectSprites[1].GetComponent<BoxCollider>().enabled = false;
            _objectSprites[2].GetComponent<BoxCollider>().enabled = false;
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
        _objectSprites[1].GetComponent<BoxCollider>().enabled = true;
        _objectSprites[2].GetComponent<BoxCollider>().enabled = true;
        _coolDown = _coolDownSet;
    }

}
