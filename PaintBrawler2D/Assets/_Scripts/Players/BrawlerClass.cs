using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BrawlerClass : HeroScript {

    // Class specific stats
    private int _brawlerDamage = 15;
    private int _brawlerArmor = 5;
    private float _brawlerMoveSpeed = 8f;
    private float _brawlerAttackSpeed = 0.2f;
    private float _brawlerManaRegen = 5f;

    private float _comboTimer = 2f;
    private float _comboTimerReset = 2f;
    private int _comboCounter = 0;

    [SerializeField]
    private GameObject _fistDamageColliderLeft;
    [SerializeField]
    private GameObject _fistDamageColliderRight;


    public int GetBrawlerDamage() { return _brawlerDamage; }

    // Use this for initialization
    void Start () {
        InitializeClass(_playerNumber);
        InitializeStats();

        _animator = _characterObj.GetComponent<Animator>();
    }
	
    public void InitializeStats() {
        _damage = _brawlerDamage;
        _armor = _brawlerArmor;
        _moveSpeed = _brawlerMoveSpeed;
        _attackSpeed = _brawlerAttackSpeed;
        _manaRegen = _brawlerManaRegen;

        _fistDamageColliderLeft.SetActive(false);
        _fistDamageColliderRight.SetActive(false);

        _hitPointMax = _hitPoints;
    }

    public void AttackRegen()
    {
        if ((_hitPoints + _damage / 10) < _hitPointMax) { 
            _hitPoints += _damage / 10;
            GameObject DamageCounter = Instantiate(_damageCounter, transform.position, transform.rotation) as GameObject;
            DamageCounter.GetComponent<DamageText>().Initialize(_damage / 10, "Green");
        }
        else{
            _hitPoints = _hitPointMax;
        }

        _UIHealthBar.GetComponent<Slider>().value = _hitPoints;
    }

	// Update is called once per frame
	void Update () {
        if (_attackReady == false) {
            _coolDown -= Time.deltaTime;
        }

        if (_coolDown < 0) {
            _attackReady = true;
            _coolDown = _attackSpeed;
        }
        
        ManageCombo();
        ManageDeath();
    }

    void ManageCombo()
    {
        if (_comboTimer > 0)
        {
            _comboTimer -= Time.deltaTime;
        }
        else
        {
            _comboCounter = 0;
        }


        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1") &&
        !_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2") &&
        !_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 3"))
        {
            _fistDamageColliderLeft.SetActive(false);
            _fistDamageColliderRight.SetActive(false);
        }
        else {
            _fistDamageColliderLeft.SetActive(true);
            _fistDamageColliderRight.SetActive(true);
        }
    }

    public override void Attack() {
        if (_attackReady == true) {

            _attackReady = false;
            
            _comboTimer = _comboTimerReset;

            // TURN ON FISTS!
            switch (_comboCounter)
            {
                case 0:
                    _animator.Play("Attack 1");
                    GetComponent<AudioSource>().Play();
                    _fistDamageColliderLeft.SetActive(true);
                    break;
                case 1:
                    _animator.Play("Attack 2");
                    GetComponent<AudioSource>().Play();
                    _fistDamageColliderRight.SetActive(true);
                    break;
                case 2:
                    _animator.Play("Attack 3");
                    GetComponent<AudioSource>().Play();
                    _fistDamageColliderLeft.SetActive(true);
                    _fistDamageColliderRight.SetActive(true);
                    _comboTimer = 0.5f;
                    break;
                default:
                    _comboCounter = 0;
                    _comboTimer = 0f;
                    break;
            }
            _comboCounter++;
        }
    }
    
}
