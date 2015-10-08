using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class EnemyScript : MonoBehaviour {
    // Stats
    public Vector2 RandomCirclePoint;

    [SerializeField]
    protected int _hitPoints = 100;
    [SerializeField]
    protected int _damage = 0;
    [SerializeField]
    protected int _armor = 0;
    [SerializeField]
    protected float _moveSpeed = 0f;
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
        fleeing
    }
    public EnemyState _currentState = EnemyState.initializing;

    // Movement - calculated for sprite direction
    [SerializeField]
    protected Vector2 _lastPosition;

    [SerializeField]
    protected GameObject[] _objectSprites;
    [SerializeField]
    protected GameObject _objectWholeSprite;
    [SerializeField]
    protected GameObject _animatedObj;
    protected float _moveDirection;

    // Color variables
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
    private GameObject _bigDamage;

    [SerializeField]
    protected string _pastMixColor = "";
    [SerializeField]
    protected GameObject _particleGenerator;
    [SerializeField]
    protected GameObject _colorExplosion;

    [SerializeField]
    protected GameObject _fleeTarget;

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

    public virtual void ManageMovement() { }

    // Stay within range, but do not attack
    public void ReachedCirclingDistance() { _currentState = EnemyState.circling; }
    public void MovedOutCirclingDistance() { _currentState = EnemyState.chasing; }
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

    public virtual void AccumulateColor(int Damage, string PrimaryColor)
    {
        
    }

    public void TakeDamage(int Damage, Color Color)
    {
        if (Color == _secondaryColor)
        {
            _hitPoints -= Damage;
            _healthSlider.GetComponent<Slider>().value = _hitPoints;
            GameObject DamageText = Instantiate(_damageText, transform.position, transform.rotation) as GameObject;
            DamageText.GetComponent<DamageText>().Initialize(Damage, "Red");
        }
    }

    public void TakeSplashDamage(int Damage) {
        _hitPoints -= Damage;
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

    protected virtual void InitializeClass()
    {
        
    }

    // This determines that the player and enemy unit are on the same plane
    protected void ManageRayCast()
    {
        Debug.DrawRay(transform.position + Vector3.left, Vector3.left * 50);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.left, Vector2.left);

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

    public void Avoid(GameObject avoidObject) {
        transform.position = Vector2.MoveTowards(transform.position, avoidObject.transform.position, -Time.deltaTime * _moveSpeedActual);
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

}
