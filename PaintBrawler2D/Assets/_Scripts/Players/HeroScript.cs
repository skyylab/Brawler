using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class HeroScript : MonoBehaviour {

    // List of attackers that are currently engaging the player
    [SerializeField]
    protected List<GameObject> AttackerList = new List<GameObject>();
    protected int _maxAttackerNum = 2;

    // Stats
    [SerializeField]
    protected int _hitPoints = 100;
    [SerializeField]
    protected int _hitPointMax = 100;
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
    [SerializeField]
    protected bool _isAlive = true;
    private bool _playDeathOnce = false;

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
    protected GameObject _deadPlayer;

    [SerializeField]
    protected GameObject _mainCamera;


    // Class specific stats

    // Color variables
    protected Color[] _primaryColorArray = {new Color (217f/255f, 40f/255f, 46f/255f),
                                          new Color (255f/255f, 209f/255f, 64/255f),
                                          new Color (43f/255f, 125f/255f, 225f/255f) };
    
    protected Color[] _secondaryColorArray = {new Color (57f/255f, 212f/255f, 50f/255f),
                                            new Color (130f/255f, 83f/255f, 137f/255f),
                                            new Color(234f/255f, 123f/255f, 54f/255f)};


    [SerializeField]
    protected string[] _primaryColorString = {"Red", "Yellow", "Blue" };
    [SerializeField]
    protected string[] _secondaryColorString = { "Green", "Purple", "Orange" };

    [SerializeField]
    protected Color _primaryColor;
    [SerializeField]
    protected string _currentPrimaryColor;

    [SerializeField]
    private GameObject _deathAnimation;
    protected Animator _animator;

    [SerializeField]
    protected GameObject _damageCounter;

    public virtual void Attack() { }
    public virtual void SecondaryAttack() { }

    public GameObject GetCharacterObj() { return _characterObj; }
    public string GetPrimaryColorString() { return _currentPrimaryColor; }
    public bool ReturnIsAlive() { return _isAlive; }

    protected int _firingDirection = -1;

    public bool AddAttacker(GameObject Attacker) {
        if (_maxAttackerNum > AttackerList.Count && !AttackerList.Contains(Attacker)) { 
            AttackerList.Add(Attacker);
            return true;
        }
        if (AttackerList.Contains(Attacker)) {
            return true;
        }

        return false;
    }

    public bool RemoveAttacker(GameObject Attacker) {
        if (AttackerList.Contains(Attacker)) { 
            AttackerList.Remove(Attacker);
            return true;
        }
        return false;
    }

    protected void InitializeClass(int PlayerNumber) {
        // Setting Player Color
        _primaryColor = _primaryColorArray[PlayerNumber - 1];
        _currentPrimaryColor = _primaryColorString[PlayerNumber - 1];

        foreach (GameObject x in _objectSpritesPrimary) {
            x.GetComponent<SpriteRenderer>().color = _primaryColor;
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
        GameObject DamageCounter = Instantiate(_damageCounter, transform.position, transform.rotation) as GameObject;
        DamageCounter.GetComponent<DamageText>().Initialize(Damage, "Blue");
    }

    public void Jump() {
        _animator.CrossFade("Jump", 0.01f);
    }

    protected void ManageDeath() {
        if (_hitPoints <= 0)
        {
            _moveSpeed = 0;
            if (!_playDeathOnce)
            {
                _animator.Play("Death");
                _playDeathOnce = true;
            }

            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) {
                _deadPlayer.SetActive(true);
                _mainCamera.GetComponent<CameraControls>().RemovePlayers(gameObject);
                Vector3 currentPosition = transform.position;
                transform.position = new Vector3(300f, 300f, 300f);
                gameObject.tag = "Untagged";
                transform.position = currentPosition;
                _isAlive = false;
                _characterObj.SetActive(false);
            }
        }
    }

    public void RevivePlayer()
    {
        _hitPoints = _hitPointMax;
        _manaPoints = 100;
        _mainCamera.GetComponent<CameraControls>().AddPlayers(gameObject);
        gameObject.tag = "Player";
        _isAlive = true;
        _deadPlayer.SetActive(false);
        _characterObj.SetActive(true);
        _UIHealthBar.GetComponent<Slider>().value = _hitPoints;
        _UIManaBar.GetComponent<Slider>().value = _manaPoints;
    }
}
