using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrimaryColorRangedEnemy : EnemyScript {
    [SerializeField]
    private GameObject Spawner;

    [SerializeField]
    private GameObject _prefabBullet;
    [SerializeField]
    private GameObject _prefabMuzzleFlash;
    [SerializeField]
    private GameObject _attackRangeSight;

    private float _fleeTimer = 1.5f;

    public void ResetFleeTimer() { _fleeTimer = 1.5f;  }

    void Start()
    {
        InitializeClass();
        _particleGenerator.GetComponent<ParticleSystem>().emissionRate = 0;
        Spawner = GameObject.Find("EnemySpawner");
    }

    public override void Update()
    {
        base.Update();

        if (_invincibleTimer < 0) {
            _attackRangeSight.SetActive(true);
        }

        if (_stunDuration < 0) { 
            ManageRayCast();
            ManageEnemyState();
            ManageSpriteOrientation();
        }
        else {
            // PlayStunAnimation
        }
    }

    public override void Idle() {
    }

    public override void SawPlayer() {
        _currentState = EnemyState.chasing;
    }

    public override void Chasing() {
        base.Chasing();
        _attackRangeSight.SetActive(true);
        if (_aquiredTargets.Count > 0)
        {
            _lastPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual);
        }
    }

    public override void Attacking() {

        _currentlyAttacking = true;
        _lastPosition = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual * 1.5f);
        ManageAttack();
    }

    public override void Fleeing() {
        _fleeTimer -= Time.deltaTime;

        if (_fleeTimer > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, _fleeTarget.transform.position, Time.deltaTime * _moveSpeed * -1);
        }
        else
        {
            _currentState = EnemyState.chasing;
        }
    }

    public void ManageAttack() {

        _coolDown -= Time.deltaTime;

        if (_coolDown < 0)
        {
            Attack();
        }
    }

    public override void Attack()
    {
        _attackLanded = false;
        _animator.CrossFade("FireWeapon", 0.01f);
        GameObject EnemyBullet = Instantiate(_prefabBullet, transform.position, transform.rotation) as GameObject;

        int facingDirection = 0;

        if (_moveDirection > 0)
        {
            facingDirection = -1;
        }
        else
        {
            facingDirection = 1;
        }

        EnemyBullet.GetComponent<BulletEnemyScript>().Initialize(_damage, facingDirection);
        _coolDown = Random.Range(_coolDownSet, _coolDownSet + 1f);
    }

    public override void Fallen()
    {
        base.Fallen();
        _attackRangeSight.SetActive(false);
    }

}
