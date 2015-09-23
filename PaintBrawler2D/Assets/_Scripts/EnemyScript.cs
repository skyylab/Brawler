using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class EnemyScript : MonoBehaviour {
    // Stats
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
    private GameObject _healthSlider;

    // Range finders
    [SerializeField]
    protected GameObject _aquisitionRange;
    [SerializeField]
    private float _aquisitionRangeValue;
    [SerializeField]
    protected GameObject _attackRange;
    [SerializeField]
    private float _attackRangeValue;
    [SerializeField]
    protected GameObject _circleRange;
    [SerializeField]
    private float _circleRangeValue;
    [SerializeField]
    protected List<GameObject> _aquiredTargets = new List<GameObject>();
    [SerializeField]
    protected List<GameObject> _attackTargets = new List<GameObject>();

    // Attacking variables
    protected bool _inAttackRange = false;
    protected bool _attackLanded = false;
    public bool _currentlyAttacking = false;
    protected enum EnemyState {
        initializing,
        idle,
        sawPlayer,
        chasing,
        circling,
        attacking,
        fleeing
    }
    protected EnemyState _currentState = EnemyState.initializing;

    // Movement - calculated for sprite direction
    [SerializeField]
    protected Vector2 _lastPosition;

    [SerializeField]
    protected GameObject[] _objectSprites;
    [SerializeField]
    protected GameObject _objectWholeSprite;
    [SerializeField]
    private GameObject _animatedObj;

    // Color variables
    [SerializeField]
    private Color[] _primaryColorArray = {new Color (255f/255f, 0f, 0f),
                                          new Color (255f/255f, 215f/255f, 0f),
                                          new Color (0f, 0f, 255f/255f) };

    [SerializeField]
    private Color[] _secondaryColorArray = {new Color (15f/255f, 148f/255f, 19f/255f),
                                            new Color (255f/255f, 137f/255f, 0f),
                                            new Color (104f/255f, 25f/255f, 193f/255f) };

    [SerializeField]
    protected Color _primaryColor;
    [SerializeField]
    protected string _currentColor;
    [SerializeField]
    protected Color _changedColor;
    protected Animator _animator;

    [SerializeField]
    private GameObject _deathAnimation;


    public virtual void Attack(){ }

    public int GetDamage() { return _damage; }
    public bool GetAttackLanded() { return _attackLanded; }
    public void SetAttackLanded(bool AttackLanded) { _attackLanded = AttackLanded; }
    public void StopSpeed() { _moveSpeedActual = 0f; }
    public void StartSpeed() { _moveSpeedActual = _moveSpeed; }

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

    // Idle Animations
    public virtual void Idle() {}
    public virtual void SawPlayer() {}
    // Start approach
    public virtual void Chasing() {}
    // Stay within range, but do not attack
    public void ReachedCirclingDistance() { _currentState = EnemyState.circling; }
    public void MovedOutCirclingDistance() { _currentState = EnemyState.chasing; }
    public virtual void Circling() {}
    // Attack
    public virtual void Attacking() {}
    // Flee
    public virtual void Fleeing() {}

    public virtual void ManageMovement() { }

    public void TakeDamage(int Damage, string PrimaryColor, string SecondaryColor)
    {
        Color ChangedColor = _primaryColorArray[0];

        if (PrimaryColor == "Red") {
            ChangedColor = _primaryColorArray[0];
        }
        else if (PrimaryColor == "Yellow")
        {
            ChangedColor = _primaryColorArray[1];
        }
        else if (PrimaryColor == "Blue")
        {
            ChangedColor = _primaryColorArray[2];
        }

        if (!_colorChanged) {
            if (_primaryColor != ChangedColor) {

                foreach (GameObject x in _objectSprites)
                {
                    x.GetComponent<SpriteRenderer>().color = MixColor(_currentColor, PrimaryColor);
                }

                _currentColor = MixColorString(_currentColor, PrimaryColor);
            }
            _colorChanged = true;
        }
        else {
            if (MatchedColor (_currentColor, SecondaryColor)) {
                Damage *= 2;
            }
            else {
                Damage = Damage / 4;
            }
        }

        _hitPoints -= Damage;
        _healthSlider.GetComponent<Slider>().value = _hitPoints;
    }

    private bool MatchedColor(string Color1, string Color2) {
        if (Color1 == Color2) {
            return true;
        }
        return false;
    }

    private Color MixColor(string Color1, string Color2) {

        if (Color1 == "Red" && Color2 == "Blue") {
            return _secondaryColorArray[2];
        }

        if (Color1 == "Red" && Color2 == "Yellow") {
            return _secondaryColorArray[1];
        }

        if (Color1 == "Yellow" && Color2 == "Red")
        {
            return _secondaryColorArray[1];
        }

        if (Color1 == "Yellow" && Color2 == "Blue")
        {
            return _secondaryColorArray[0];
        }

        if (Color1 == "Blue" && Color2 == "Red")
        {
            return _secondaryColorArray[2];
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
        int RandomNumber = Random.Range(0,3);
        // Setting Player Color
        _primaryColor = _primaryColorArray[RandomNumber];

        switch (RandomNumber) {
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

        foreach (GameObject x in _objectSprites) {
            x.GetComponent<SpriteRenderer>().color = _primaryColor;
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
