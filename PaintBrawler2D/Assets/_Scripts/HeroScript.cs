using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class HeroScript : MonoBehaviour {

    // List of attackers that are currently engaging the player
    public List<GameObject> AttackerList = new List<GameObject>();

    // Stats
    [SerializeField]
    protected int _hitPoints = 100;
    [SerializeField]
    protected int _manaPoints = 100;
    [SerializeField]
    protected int _playerNumber;
    [SerializeField]
    protected int _damage = 0;
    [SerializeField]
    protected int _armor = 0;
    [SerializeField]
    protected float _moveSpeed = 0f;
    [SerializeField]
    protected float _coolDown = 0f;
    [SerializeField]
    protected float _attackSpeed = 0f;
    [SerializeField]
    protected float _manaRegen = 0f;
    [SerializeField]
    protected bool _attackReady = false;

    // UI Purposes
    [SerializeField]
    protected GameObject _UIHealthBar;
    [SerializeField]
    protected GameObject _UIManaBar;

    [SerializeField]
    protected GameObject _characterObj;

    // This might be not used later - for prototyping purposes only
    [SerializeField]
    protected GameObject[] _objectSpritesPrimary;
    [SerializeField]
    protected GameObject[] _objectSpritesSecondary;


    // Class specific stats

    //private int _mageDamage = 10;
    //private int _mageArmor = 0;
    //private float _mageMoveSpeed = 10f;
    //private float _mageAttackSpeed = 1.25f;
    //private float _mageManaRegen = 15f;

    // Color variables
    [SerializeField]
    protected Color[] _primaryColorArray = {new Color (255f/255f, 0f, 0f),
                                          new Color (255f/255f, 215f/255f, 0f),
                                          new Color (0f, 0f, 255f/255f) };

    [SerializeField]
    protected Color[] _secondaryColorArray = {new Color (15f/255f, 148f/255f, 19f/255f),
                                            new Color (104f/255f, 25f/255f, 193f/255f),
                                            new Color(255f/255f, 137f/255f, 0f)};


    [SerializeField]
    protected string[] _primaryColorString = {"Red", "Yellow", "Blue" };
    protected string[] _secondaryColorString = { "Green", "Purple", "Orange" };

    [SerializeField]
    protected Color _primaryColor;
    [SerializeField]
    protected Color _secondaryColor;
    [SerializeField]
    protected string _currentPrimaryColor;
    [SerializeField]
    protected string _currentSecondaryColor;
    protected Animator _animator;

    [SerializeField]
    private GameObject _deathAnimation;

    public virtual void Attack() { }
    public virtual void SecondaryAttack() { }

    public GameObject GetCharacterObj() { return _characterObj; }
    public string GetPrimaryColorString() { return _currentPrimaryColor; }
    public string GetSecondaryColorString() { return _currentSecondaryColor; }

    protected int _firingDirection = -1;

    protected void InitializeClass(int PlayerNumber) {
        // Setting Player Color
        _primaryColor = _primaryColorArray[PlayerNumber - 1];
        _secondaryColor = _secondaryColorArray[PlayerNumber - 1];

        _currentPrimaryColor = _primaryColorString[PlayerNumber - 1];
        _currentSecondaryColor = _secondaryColorString[PlayerNumber - 1];

        foreach (GameObject x in _objectSpritesPrimary) {
            x.GetComponent<SpriteRenderer>().color = _primaryColor;
        }

        foreach (GameObject x in _objectSpritesSecondary)
        {
            x.GetComponent<SpriteRenderer>().color = _secondaryColor;
        }

        _UIHealthBar.GetComponent<Slider>().maxValue = _hitPoints;
        _UIHealthBar.GetComponent<Slider>().value = _hitPoints;
        _UIManaBar.GetComponent<Slider>().maxValue = _manaPoints;
        _UIManaBar.GetComponent<Slider>().value = _manaPoints;
    }

    public void ManageFlipSprite(Vector3 Direction) {
        if (Direction.x < 0) {
            _characterObj.transform.localEulerAngles = new Vector3(_characterObj.transform.localEulerAngles.x, 180f, 0f);
            _firingDirection = 1;
        }
        else {
            _characterObj.transform.localEulerAngles = new Vector3(_characterObj.transform.localEulerAngles.x, 0f, 0f);
            _firingDirection = -1;
        }
    }

    public void TakeDamage(int Damage)
    {
        _hitPoints -= Damage;
        _UIHealthBar.GetComponent<Slider>().value = _hitPoints;
    }

    public void Jump() {
        _animator.CrossFade("Jump", 0.01f);
    }

    // Update is called once per frame
    void Update () {
	    if (_hitPoints <= 0) {
            // Dead
        }
	}
}
