using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SecondaryColorEnemy : EnemyScript {
    
    [SerializeField]
    private GameObject Spawner;

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

    protected override void InitializeClass() {
        int RandomNumber = Random.Range(0, 3);
        // Setting Player Color
        _secondaryColor = _secondaryColorArray[RandomNumber];

        switch (RandomNumber)
        {
            case 0:
                _currentColor = "Green";
                break;
            case 1:
                _currentColor = "Purple";
                break;
            case 2:
                _currentColor = "Orange";
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
        base.AccumulateColor(Damage, PrimaryColor);
        if (_pastMixColor == "")
        {
            _pastMixColor = PrimaryColor;
            _particleGenerator.GetComponent<ParticleSystem>().emissionRate = 5;
            _particleGenerator.GetComponent<ParticleSystem>().startColor = returnPrimaryColor(PrimaryColor);
        }
        else if (_pastMixColor != PrimaryColor)
        {
            Color MixedColor = MixColor(_pastMixColor, PrimaryColor);
            _particleGenerator.GetComponent<ParticleSystem>().startColor = MixedColor;
            _particleGenerator.GetComponent<ParticleSystem>().emissionRate = 0;
            GameObject ColorExplosion = Instantiate(_colorExplosion, transform.position, transform.rotation) as GameObject;
            ColorExplosion.GetComponent<ExplosionBirthTimer>().InitializeColor(MixedColor, Damage);
            ColorExplosion.transform.parent = gameObject.transform;
            _pastMixColor = "";
        }
    }

    public override void Idle() {
    }

    public override void SawPlayer() {
        _currentState = EnemyState.chasing;
    }

    public override void Chasing() {
        Debug.Log(gameObject.name + " is Chasing");

        if (_aquiredTargets.Count > 0)
        {
            _lastPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual);
        }
    }

    public override void Circling() {

        Debug.Log(gameObject.name + " is Circling ");
        transform.position = Vector2.MoveTowards(transform.position, RandomCirclePoint + (Vector2)_aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual);
    }

    public override void Attacking() {

        Debug.Log(gameObject.name + " is Attacking");

        _currentlyAttacking = true;
        _lastPosition = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, _aquiredTargets[0].transform.position, Time.deltaTime * _moveSpeedActual * 1.5f);
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
