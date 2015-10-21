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
    protected int _manaPointsMax = 100;
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
    private float _deathAnimTimer = 1f;

    [SerializeField]
    protected float _manaRegenTimer = 0.6f;
    [SerializeField]
    protected float _manaRegenTimerReset;
    [SerializeField]
    protected int _attackManaCost = 5;
    [SerializeField]
    protected int _manaRegenValue = 1;

    [SerializeField]
    private ParticleSystem _chargeParticleEffects;
    [SerializeField]
    private ParticleSystem _chargeParticleEffects2;
    [SerializeField]
    private float _chargeTimerMax = 3f;
    [SerializeField]
    private float _chargeTimerMaxReset = 3f;

    [SerializeField]
    protected float _chargeAttackTime;
    [SerializeField]
    protected bool _chargeButtonReleased = false;

    // UI Purposes
    [SerializeField]
    protected GameObject _UIHealthBar;
    [SerializeField]
    protected GameObject _UIManaBar;
    [SerializeField]
    protected GameObject _UISpecialBar;

    [SerializeField]
    protected GameObject _characterObj;

    // This might be not used later - for prototyping purposes only
    [SerializeField]
    protected GameObject[] _objectSpritesPrimary;

    [SerializeField]
    protected GameObject _deadPlayer;

    [SerializeField]
    protected GameObject _mainCamera;

    protected float _beeSpecialScale = 1f;


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

    [SerializeField]
    protected bool _secondaryAttackActive = false;
    [SerializeField]
    protected bool _specialActive = false;

    [SerializeField]
    protected float _specialAttackCooldown = 60f;

    // Audio
    [SerializeField]
    private AudioClip[] _getHit;
    private AudioSource _audio;


    public bool GetSpecialStatus() { return _specialActive; }
    public bool ShouldMove() {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage")) {
            return false;
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) {
            return false;
        }
        return true;
    }

    public virtual void Attack() { }
    public virtual void ChargeAttack() {
        float MaxSize = 6f;
        _chargeButtonReleased = false;

        _chargeParticleEffects2.startSize = 1f;

        if (_chargeParticleEffects.startSize < MaxSize) { 
            _chargeParticleEffects.startSize += 0.05f;
            _chargeAttackTime += Time.deltaTime;
        }
    }

    public virtual void ResetChargeAttack()
    {
        _chargeParticleEffects2.startSize = 0f;
        _chargeParticleEffects.startSize = 0f;
        _chargeButtonReleased = true;
    }

    public void ChargeAttackReset() {
        _chargeAttackTime = 0f;
    }

    public virtual void SecondaryAttack() { }

    public virtual void SpecialAttack() { }

    public virtual void Play() { }
    public virtual void PlayWalkAnim() { }
    public virtual void PlayIdleAnim() { }

    public GameObject GetCharacterObj() { return _characterObj; }
    public string GetPrimaryColorString() { return _currentPrimaryColor; }
    public bool CanActivateSpecial () { return _specialAttackCooldown >= 60; }
    public bool ReturnIsAlive() { return _isAlive; }

    protected int _firingDirection = -1;

    public virtual void Start() {
        _manaRegenTimerReset = _manaRegenTimer;
        _audio = GetComponent<AudioSource>();
    }

    public virtual void Update() {
        ManageMana();

        if (_chargeButtonReleased && _chargeAttackTime >= 0)
        {
            _chargeAttackTime -= Time.deltaTime;
        }

        if (_specialActive) {
            _specialAttackCooldown = 0f;
        }
        else {
            _specialAttackCooldown += Time.deltaTime;
            if (_specialAttackCooldown > 60f) {
                _specialAttackCooldown = 60;
            }
        }

        _UISpecialBar.GetComponent<Slider>().value = _specialAttackCooldown;
    }

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
            transform.localScale = new Vector3(_characterObj.transform.localScale.x * -1 * _beeSpecialScale,
                                               _characterObj.transform.localScale.y * _beeSpecialScale,
                                               _characterObj.transform.localScale.z * _beeSpecialScale);

            _chargeParticleEffects2.transform.localScale = new Vector3(_chargeParticleEffects2.transform.localScale.x,
                                                                       _chargeParticleEffects2.transform.localScale.y,
                                                                       _chargeParticleEffects2.transform.localScale.z);
            _firingDirection = 1;
        }
        else {
            transform.localScale = new Vector3 (_characterObj.transform.localScale.x * _beeSpecialScale,
                                                _characterObj.transform.localScale.y * _beeSpecialScale,
                                                _characterObj.transform.localScale.z * _beeSpecialScale);

            _chargeParticleEffects2.transform.localScale = new Vector3(_chargeParticleEffects2.transform.localScale.x * -1,
                                                                       _chargeParticleEffects2.transform.localScale.y,
                                                                       _chargeParticleEffects2.transform.localScale.z);
            _firingDirection = -1;
        }
    }

    public void TakeDamage(int Damage, GameObject attacker)
    {
        _hitPoints -= Damage;
        _UIHealthBar.GetComponent<Slider>().value = _hitPoints;
        GameObject DamageCounter = Instantiate(_damageCounter, transform.position, transform.rotation) as GameObject;
        DamageCounter.GetComponent<DamageText>().Initialize(Damage, "Black");

        _audio.pitch = Random.Range(0.8f, 1.2f);
        _audio.PlayOneShot(_getHit[Random.Range(0, _getHit.Length)], 0.4f);

        GetComponent<Rigidbody>().AddForce(Vector3.MoveTowards(transform.position, attacker.transform.position, -500));

        if (_hitPoints > 0) { 
            _animator.Play("TakeDamage");
        }
    }

    public void Jump() {
        _animator.Play("Jump");
    }

    protected void ManageDeath() {
        if (_hitPoints <= 0)
        {
            _deathAnimTimer -= Time.deltaTime;
            _moveSpeed = 0;
            _animator.Play("Death");

            if (_deathAnimTimer < 0) {
                _deadPlayer.SetActive(true);
                _mainCamera.GetComponent<CameraControls>().RemovePlayers(gameObject);
                GameObject [] AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");

                foreach (GameObject x in AllEnemies) {
                    x.GetComponent<EnemyScript>().RemoveFromAttackList(gameObject);
                    x.GetComponent<EnemyScript>().RemoveFromTargetList(gameObject);
                }

                Vector3 currentPosition = transform.position;
                transform.position = new Vector3(300f, 300f, 300f);
                gameObject.tag = "Untagged";
                transform.position = currentPosition;
                _isAlive = false;
                _playDeathOnce = false;
                _characterObj.SetActive(false);
            }
        }
    }

    protected void ManageMana()
    {
        _UIManaBar.GetComponent<Slider>().value = _manaPoints;

        _manaRegen -= Time.deltaTime;

        if (_manaRegen < 0)
        {
            if (_manaPoints + _manaRegenValue < _manaPointsMax)
            {
                _manaPoints += _manaRegenValue;
                _manaRegen = _manaRegenTimerReset;
            }
            else {
                _manaPoints = _manaPointsMax;
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
        _deathAnimTimer = 1f;
    }
}
