using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrimaryColorSlugger : EnemyScript {
    
    [SerializeField]
    private GameObject Spawner;

    private float _waitTimer = 1f;

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
            Vector3 TargetAhead = _aquiredTargets[0].transform.position + new Vector3(10f, 0f, 0f);
            
            _lastPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, TargetAhead, Time.deltaTime * _moveSpeedActual);

            if (transform.position == TargetAhead) {
                _currentState = EnemyState.attacking;
            }
        }
    }

    public override void Attacking() {

        _waitTimer -= Time.deltaTime;

        if (_waitTimer < 0) { 
            _currentlyAttacking = true;
            _lastPosition = transform.position;
            transform.position += Vector3.left * _moveSpeedActual * 2 * Time.deltaTime;
            ManageAttack();
        }
    }

    public void ManageAttack() {


            Attack();
    }

    public override void Attack()
    {
        _attackLanded = false;
        _animator.Play("Berserk");
        _objectSprites[1].GetComponent<BoxCollider2D>().enabled = true;
        _objectSprites[2].GetComponent<BoxCollider2D>().enabled = true;
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
