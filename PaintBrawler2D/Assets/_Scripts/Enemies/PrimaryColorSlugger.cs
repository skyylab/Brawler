using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrimaryColorSlugger : EnemyScript {
    
    [SerializeField]
    private GameObject Spawner;
    [SerializeField]
    private float _waitTimer = 1f;
    [SerializeField]
    private float _chaseOffScreenTimer = 1f;
    [SerializeField]
    private float _attackTime = 4f;

    private float _speedConstant = 2f;

    void Start()
    {
        InitializeClass();
        _particleGenerator.GetComponent<ParticleSystem>().emissionRate = 0;
        Spawner = GameObject.Find("EnemySpawner");
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
            Vector3 TargetAhead = _aquiredTargets[0].transform.position + new Vector3(10f, 0f, 0f);
            
            _lastPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, TargetAhead, Time.deltaTime * _moveSpeedActual);

            if (transform.position == TargetAhead) {
                _currentState = EnemyState.attacking;
            }
        }
    }

    public override void Attacking() {

        _waitTimer -= Time.deltaTime;
        _attackTime -= Time.deltaTime;
        _chaseOffScreenTimer -= Time.deltaTime;

        if (_waitTimer < 0) { 
            _currentlyAttacking = true;
            _lastPosition = transform.position;
            transform.position += Vector3.left * _moveSpeedActual * _speedConstant * Time.deltaTime;
            Attack();
        }

        if (_attackTime < 0)
        {
            if (!_objectSprites[0].GetComponent<Renderer>().isVisible)
            {
                _speedConstant *= -1;
                _attackTime = 4f;
                _chaseOffScreenTimer = 1f;
            }
        }

        if (_chaseOffScreenTimer > 0)
        {
            Vector3 TargetAhead = new Vector3(transform.position.x, _aquiredTargets[0].transform.position.y, 0f);
            transform.position = Vector3.MoveTowards(transform.position, TargetAhead, Time.deltaTime * _moveSpeedActual);
        }
    }

    public override void Attack()
    {
        _attackLanded = false;
        _animator.Play("Berserk");
        _objectSprites[1].GetComponent<BoxCollider>().enabled = true;
        _objectSprites[2].GetComponent<BoxCollider>().enabled = true;
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
}
