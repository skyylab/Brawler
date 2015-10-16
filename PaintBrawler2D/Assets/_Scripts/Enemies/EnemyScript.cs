using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class EnemyScript : MonoBehaviour {
    // Stats
    public Vector3 RandomSpherePoint;

    [SerializeField]
    protected int _hitPoints = 100;
    [SerializeField]
    protected int _damage = 0;
    [SerializeField]
    protected int _armor = 0;
    [SerializeField]
    protected float _moveSpeed = 0f;
    [SerializeField]
    protected float _moveSpeedActual = 0f;
    [SerializeField]
    protected float _coolDown = 0f;
    protected float _coolDownSet = 0f;
    [SerializeField]
    protected float _attackSpeed = 0f;
    [SerializeField]
    protected bool _attackReady = false;
    [SerializeField]
    protected bool _colorChanged = false;
    [SerializeField]
    protected GameObject _healthSlider;
    public enum ColorType
    {
        NonColored,
        Primary,
        Secondary
    }
    public ColorType _colorType;

    // Range finders
    [SerializeField]
    protected GameObject _aquisitionRange;
    [SerializeField]
    protected float _aquisitionRangeValue;
    [SerializeField]
    protected GameObject _attackRange;
    [SerializeField]
    protected float _attackRangeValue;
    [SerializeField]
    protected GameObject _circleRange;
    [SerializeField]
    protected float _circleRangeValue;
    [SerializeField]
    public List<GameObject> _aquiredTargets = new List<GameObject>();
    [SerializeField]
    protected List<GameObject> _attackTargets = new List<GameObject>();

    [SerializeField]
    private GameObject _damageText;

    [SerializeField]
    private GameObject _mainCamera;

    // Attacking variables
    protected bool _inAttackRange = false;
    protected bool _inCircleRange = false;
    protected bool _attackLanded = false;
    public bool _currentlyAttacking = false;
    public enum EnemyState {
        initializing,
        idle,
        sawPlayer,
        chasing,
        circling,
        attacking,
        fleeing,
        pulled
    }
    public EnemyState _currentState = EnemyState.initializing;

    // Status variables

    [SerializeField]
    protected GameObject _fleeTarget;
    [SerializeField]
    protected bool _notColored;
    [SerializeField]
    protected bool _currentlyPulled;
    [SerializeField]
    protected float _pullTimer = 2f;
    protected float _pullTimerReset;

    [SerializeField]
    protected bool _isOffScreen;

    // Movement - calculated for sprite direction
    [SerializeField]
    protected Vector3 _lastPosition;
    [SerializeField]
    protected float _stunDuration;

    [SerializeField]
    protected GameObject[] _objectSprites;
    [SerializeField]
    protected GameObject _objectWholeSprite;
    [SerializeField]
    protected GameObject _animatedObj;
    [SerializeField]
    protected float _moveDirection;
    [SerializeField]
    protected GameObject _avoidEnemy;

    // Color variables
    protected Color _whiteColor = new Color(1f, 1f, 1f);
    protected Color[] _primaryColorArray = {new Color (217f/255f, 40f/255f, 46f/255f),
                                          new Color (255f/255f, 209f/255f, 64/255f),
                                          new Color (43f/255f, 125f/255f, 225f/255f) };

    protected Color[] _secondaryColorArray = {new Color (57f/255f, 212f/255f, 50f/255f),
                                            new Color (130f/255f, 83f/255f, 137f/255f),
                                            new Color(234f/255f, 123f/255f, 54f/255f)};

    [SerializeField]
    protected Color _secondaryColor;
    [SerializeField]
    protected string _currentColor;
    [SerializeField]
    protected Color _changedColor;
    protected Animator _animator;

    [SerializeField]
    private GameObject _deathAnimation;

    [SerializeField]
    private GameObject[] _mixSprites;

    [SerializeField]
    protected string _pastMixColor = "";
    [SerializeField]
    protected GameObject _particleGenerator;
    [SerializeField]
    protected GameObject _colorExplosion;

    [SerializeField]
    private GameObject _niceText;

    [SerializeField]
    private GameObject _pullLocation;

    // Virtual functions
    public virtual void Attack(){ }
    // Idle Animations
    public virtual void Idle() { }
    public virtual void SawPlayer() { }
    // Start approach
    public virtual void Chasing() { }
    public virtual void Circling() { }
    // Attack
    public virtual void Attacking() { }
    // Flee
    public virtual void Fleeing() { }
    // Attack
    public virtual void Pulling() {
        _pullTimer -= Time.deltaTime;

        if (_pullTimer > 0) {
            transform.position = Vector3.MoveTowards(transform.position, _pullLocation.transform.position, Time.deltaTime * 30);

            if (transform.position.x >= _pullLocation.transform.position.x - 1f && transform.position.x <= _pullLocation.transform.position.x + 1f) {
                _pullTimer = -1;
                Stun(2f);
            }
        }
        else {
            _currentState = EnemyState.chasing;
        }
    }

    public virtual void ManageMovement() { }

    // Stay within range, but do not attack
    public void ReachedCirclingDistance() { _currentState = EnemyState.circling; }
    public void MovedOutCirclingDistance() {  }
    public void SetFleeTarget(GameObject FleeTarget) { _fleeTarget = FleeTarget; }

    public void SetCircleRange(bool Set) { _inCircleRange = Set; }
    public void SetAttackRange(bool Set) { _inAttackRange = Set; }

    // Accessors
    public bool GetCircleRange() { return _inCircleRange; }
    public bool GetAttackRange() { return _inAttackRange; }
    public bool GetAttackLanded() { return _attackLanded; }

    public void SetAttackLanded(bool AttackLanded) { _attackLanded = AttackLanded; }
    public void StopSpeed() { _moveSpeedActual = 0f; }
    public void StartSpeed() { _moveSpeedActual = _moveSpeed; }

    public int AttackListSize() { return _attackTargets.Count; }
    public int GetDamage() { return _damage; }

    public string GetColorString() { return ReturnColorString(_secondaryColor); }

    void Awake()
    {
        _mainCamera = Camera.main.gameObject;
        _pullTimerReset = _pullTimer;
    }

    public virtual void Update() {
        _stunDuration -= Time.deltaTime;
        GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);

        if (_stunDuration > 0) {
            _animator.Play("Stunned");
        }
        else {
        }

    }

    public void AddToTargetList(GameObject newTarget) {
        _aquiredTargets.Add(newTarget);
    }

    public void RemoveFromTargetList(GameObject newTarget) {
        _aquiredTargets.Remove(newTarget);
    }

    public void AddToAttackList(GameObject newTarget)
    {
        _attackTargets.Add(newTarget);
        _inAttackRange = true;
    }

    public void RemoveFromAttackList(GameObject newTarget)
    {
        _attackTargets.Remove(newTarget);
        if (_attackTargets.Count < 1) {
            _inAttackRange = false;
        }
    }

    protected void ManageOffScreenMovement() {

        Debug.Log(_mixSprites[0].GetComponent<SpriteRenderer>().isVisible);

        if (!_mixSprites[0].GetComponent<Renderer>().isVisible) {
            _isOffScreen = false;
            _moveSpeedActual = _moveSpeed;
        }
        else {
            _isOffScreen = true;
            _moveSpeedActual = 0f;
            Vector3 centerCameraPosition = new Vector3(_mainCamera.transform.position.x, _mainCamera.transform.position.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, centerCameraPosition, Time.deltaTime * _moveSpeed);
        }
    }

    public void AccumulateColor(int Damage, string PrimaryColor)
    {
        // Take Damage if hit by primary color and enemy is primary
        if (_colorType == ColorType.Secondary)
        {
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
        else
        {
            TakeDamage(Damage, returnPrimaryColor(PrimaryColor));
        }
    }

    public void GainStats(float scalingValue, int damageIncrease, int HPIncrease) {
        transform.localScale *= scalingValue;
        _damage += damageIncrease;
        _hitPoints += HPIncrease;


        GameObject Text = Instantiate(_niceText, transform.position + Vector3.left * 2, transform.rotation) as GameObject;
        Text.GetComponent<NiceText>().Initialize("Red", "Absorbed!", 0);
    }

    public void Stun(float Duration) {
        _stunDuration = Duration;
    }

    public void TakeFlatDamage(int Damage) {

        GameObject DamageText = Instantiate(_damageText, transform.position, transform.rotation) as GameObject;
        DamageText.GetComponent<DamageText>().Initialize(Damage, "Red");
        _hitPoints -= Damage;
        _healthSlider.GetComponent<Slider>().value = _hitPoints;

        if (_hitPoints < 0)
        {
            // Die
            _mainCamera.GetComponent<CameraControls>().EnemyKilled();
            gameObject.transform.parent = GameObject.Find("UnusedObjects").transform;
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int Damage, Color Color)
    {
        if (_colorType == ColorType.NonColored)
        {
            _secondaryColor = Color;
        }

        if (Color == _secondaryColor)
        {
            _hitPoints -= Damage;
            _healthSlider.GetComponent<Slider>().value = _hitPoints;
            GameObject DamageText = Instantiate(_damageText, transform.position, transform.rotation) as GameObject;
            DamageText.GetComponent<DamageText>().Initialize(Damage, "Red");

            if (_colorType == ColorType.Primary) {
                DamageText = Instantiate(_niceText, transform.position + Vector3.left * 2, transform.rotation) as GameObject;
                DamageText.GetComponent<NiceText>().Initialize(ReturnColorString(Color), "Nice!", 1);
            }

            if (_colorType == ColorType.Secondary)
            {
                DamageText = Instantiate(_niceText, transform.position + Vector3.left * 2, transform.rotation) as GameObject;
                DamageText.GetComponent<NiceText>().Initialize(ReturnColorString(Color), "Great!", 2);
            }
        }
        else {
            GainStats(1.1f, 5, 10);
        }

        if (_hitPoints < 0)
        {
            // Die
            _mainCamera.GetComponent<CameraControls>().EnemyKilled();
            gameObject.transform.parent = GameObject.Find("UnusedObjects").transform;
            gameObject.SetActive(false);
        }
    }

    public void TakeSplashDamage(int Damage) {
        
    }

    protected Color returnPrimaryColor(string Color1) {
        switch (Color1) {
            case "Red":
                return _primaryColorArray[0];
            case "Yellow":
                return _primaryColorArray[1];
            case "Blue":
                return _primaryColorArray[2];
        }
        return new Color(0f, 0f, 0f);
    }

    protected string ReturnColorString(Color Color1)
    {
        if (Color1 == new Color(217f / 255f, 40f / 255f, 46f / 255f)) { return "Red"; }
        else if (Color1 == new Color(255f / 255f, 209f / 255f, 64 / 255f)) { return "Yellow"; }
        else if (Color1 == new Color(43f / 255f, 125f / 255f, 225f / 255f)) { return "Blue"; }
        else if (Color1 == new Color(57f / 255f, 212f / 255f, 50f / 255f)) { return "Green"; }
        else if (Color1 == new Color(130f / 255f, 83f / 255f, 137f / 255f)) { return "Purple"; }
        else if (Color1 == new Color(234f / 255f, 123f / 255f, 54f / 255f)) { return "Orange"; }
        
        return "";
    }

    protected Color MixColor(string Color1, string Color2) {

        if (Color1 == "Red" && Color2 == "Blue") {
            return _secondaryColorArray[1];
        }

        if (Color1 == "Red" && Color2 == "Yellow") {
            return _secondaryColorArray[2];
        }

        if (Color1 == "Yellow" && Color2 == "Red")
        {
            return _secondaryColorArray[2];
        }

        if (Color1 == "Yellow" && Color2 == "Blue")
        {
            return _secondaryColorArray[0];
        }

        if (Color1 == "Blue" && Color2 == "Red")
        {
            return _secondaryColorArray[1];
        }

        if (Color1 == "Blue" && Color2 == "Yellow")
        {
            return _secondaryColorArray[0];
        }

        return _secondaryColorArray[0];
    }

    private string MixColorString(string Color1, string Color2)
    {

        if (Color1 == "Red" && Color2 == "Blue")
        {
            return "Purple";
        }

        if (Color1 == "Red" && Color2 == "Yellow")
        {
            return "Orange";
        }

        if (Color1 == "Yellow" && Color2 == "Red")
        {
            return "Orange";
        }

        if (Color1 == "Yellow" && Color2 == "Blue")
        {
            return "Green";
        }

        if (Color1 == "Blue" && Color2 == "Red")
        {
            return "Purple";
        }

        if (Color1 == "Blue" && Color2 == "Yellow")
        {
            return "Green";
        }

        return "";
    }

    protected void InitializeClass()
    {
        SetInitialColors(_colorType);
        // Setting range
        _aquisitionRange.GetComponent<SphereCollider>().radius = _aquisitionRangeValue;
        _attackRange.GetComponent<SphereCollider>().radius = _attackRangeValue;

        _healthSlider.GetComponent<Slider>().maxValue = _hitPoints;
        _healthSlider.GetComponent<Slider>().value = _hitPoints;

        _animator = _animatedObj.GetComponent<Animator>();
        _moveSpeedActual = _moveSpeed;
        _coolDownSet = _coolDown;
    }

    private void SetInitialColors(ColorType ColorType)
    {
        int RandomNumber = Random.Range(0, 3);
        if (ColorType == ColorType.Primary)
        {
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
        }
        else if (ColorType == ColorType.Secondary)
        {
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
        }
        else
        {
            // Setting Player Color
            _secondaryColor = _whiteColor;
            _currentColor = "White";
        }

        foreach (GameObject x in _objectSprites)
        {
            x.GetComponent<SpriteRenderer>().color = _secondaryColor;
        }
    }

    // This determines that the player and enemy unit are on the same plane
    protected void ManageRayCast()
    {
        Debug.DrawRay(transform.position + Vector3.left, Vector3.left * 50);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.left, Vector3.left);

        if (hit.collider != null)
        {
            if (hit.transform.gameObject.tag == "Player")
            {

            }
        }

    }

    protected void ManageSpriteOrientation() {
        float DeltaPosition = transform.position.x - _lastPosition.x;

        if (Mathf.Abs(DeltaPosition) > 0)
        {
            _moveDirection = DeltaPosition;
        }
        if (_moveDirection > 0)
        {
            _objectWholeSprite.transform.localEulerAngles = new Vector2(0f, 0f);
        }
        else if (_moveDirection < 0)
        {
            _objectWholeSprite.transform.localEulerAngles = new Vector2(0f, 180f);
        }
    }

    public void AssignAvoid(GameObject avoidObject) {
        //transform.position = Vector3.MoveTowards(transform.position, avoidObject.transform.position, -Time.deltaTime * _moveSpeedActual);
        _avoidEnemy = avoidObject;
    }

    public Vector2 GetRandomPointCircle(float angleDegrees, float radius) {
        // initialize calculation variables
        float _x = 0;
        float _y = 0;
        float angleRadians = 0;
        Vector2 _returnVector;
        // convert degrees to radians
        angleRadians = angleDegrees * Mathf.PI / 180.0f;
        // get the 2D dimensional coordinates
        _x = radius * Mathf.Cos(angleRadians);
        _y = radius * Mathf.Sin(angleRadians);
        // derive the 2D vector
        _returnVector = new Vector2(_x, _y);
        // return the vector info
        return _returnVector;
    }

    public void PullObject(GameObject PullLocation) {
        _currentState = EnemyState.pulled;
        _pullTimer = _pullTimerReset;
        _pullLocation = PullLocation.GetComponent<DrawSpots>().ReturnSpot(ReturnColorString(_secondaryColor));
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
            case EnemyState.pulled:
                Pulling();
                break;
            default:
                break;
        }
    }

}
