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

    private float _fleeTimer = 1.5f;

    public void ResetFleeTimer() { _fleeTimer = 1.5f;  }

    void Start()
    {
        InitializeClass();
        _particleGenerator.GetComponent<ParticleSystem>().emissionRate = 0;
        Spawner = GameObject.Find("EnemySpawner");
        RandomCirclePoint = Random.insideUnitCircle;
    }

    void Update()
    {
        ManageRayCast();
        ManageEnemyState();
        ManageSpriteOrientation();

        if (_hitPoints < 0)
        {
            // Die
            Spawner.GetComponent<EnemySpawner>().RemoveObject(gameObject);
            Destroy(gameObject);
        }
    }

    protected override void InitializeClass()
    {
        int RandomNumber = Random.Range(0, 3);
        // Setting Player Color
        _secondaryColor = _primaryColorArray[RandomNumber];

        switch (RandomNumber)
        {
            case 0:
                _currentColor = "Red";
                break;
            case 1:
                _currentColor = "Yellow";
                break;
            case 2:
                _currentColor = "Blue";
                break;
        }

        foreach (GameObject x in _objectSprites)
        {
            x.GetComponent<SpriteRenderer>().color = _secondaryColor;
        }

        // Setting range
        _aquisitionRange.GetComponent<CircleCollider2D>().radius = _aquisitionRangeValue;
        _attackRange.GetComponent<CircleCollider2D>().radius = _attackRangeValue;

        _healthSlider.GetComponent<Slider>().maxValue = _hitPoints;
        _healthSlider.GetComponent<Slider>().value = _hitPoints;

        _animator = _animatedObj.GetComponent<Animator>();
        _moveSpeedActual = _moveSpeed;
        _coolDownSet = _coolDown;
    }


    public override void AccumulateColor(int Damage, string PrimaryColor)
    {
        TakeDamage(Damage, returnPrimaryColor(PrimaryColor));
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
            transform.position = Vector2.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual);
        }
    }

    public override void Circling() {
        transform.position = Vector2.MoveTowards(transform.position, RandomCirclePoint + (Vector2)_aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual);
    }

    public override void Attacking() {

        _currentlyAttacking = true;
        _lastPosition = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual * 1.5f);
        ManageAttack();
    }

    public override void Fleeing() {
        _fleeTimer -= Time.deltaTime;

        if (_fleeTimer > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, _fleeTarget.transform.position, Time.deltaTime * _moveSpeed * -1);
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
            // Move close to the target
            case EnemyState.chasing:
                Chasing();
                break;
            // Separate - start circling around the target
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
