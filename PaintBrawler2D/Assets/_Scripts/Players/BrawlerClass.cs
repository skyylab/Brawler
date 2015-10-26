using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BrawlerClass : HeroScript {

    // Class specific stats
    private int _brawlerDamage = 9;
    private int _brawlerArmor = 5;
    private float _brawlerMoveSpeed = 8f;
    private float _brawlerAttackSpeed = 0.3f;

    private float _comboTimer = 2f;
    private float _comboTimerReset;
    private int _comboCounter = 0;

    [SerializeField]
    private GameObject _fistDamageColliderLeft;
    [SerializeField]
    private GameObject _fistDamageColliderRight;
    [SerializeField]
    private GameObject _fistStunColliderLeft;
    [SerializeField]
    private GameObject _fistStunColliderRight;

    [SerializeField]
    private AudioClip _attack1_SFX;
    [SerializeField]
    private AudioClip _attack2_SFX;
    [SerializeField]
    private AudioClip _attack3_SFX;

    [SerializeField]
    private GameObject _stunningObj;

    private float _audioVolume = 0.6f;

    // Special Attack Stuff
    private float _specialAttackTimer = 20f;
    private float _specialAttackTimerReset;
    private int _addedHP = 50;
    private int _addedDamage = 3;
    private float _addedScale = 1.5f;

    public int GetBrawlerDamage() { return _damage; }

    // Use this for initialization
    public override void Start () {
        base.Start();
        InitializeClass(_playerNumber);
        InitializeStats();

        _specialAttackTimerReset = _specialAttackTimer;
        _animator = _characterObj.GetComponent<Animator>();
    }
	
    public void InitializeStats() {
        _damage = _brawlerDamage;
        _armor = _brawlerArmor;
        _moveSpeed = _brawlerMoveSpeed;
        _attackSpeed = _brawlerAttackSpeed;

        _comboTimer = _brawlerAttackSpeed + 0.2f;
        _comboTimerReset = _comboTimer;

        _hitPointMax = _hitPoints;
    }

    // Unused due to balance issues - may reimplement
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
	public override void Update () {

        base.Update();

        if (_attackReady == false) {
            _coolDown -= Time.deltaTime;
        }

        if (_coolDown < 0) {
            _attackReady = true;
            _coolDown = _attackSpeed;
        }
        
        ManageCombo();
        ManageDeath();
        ManageMana();

        if (_chargeAttackTime < 0) {
            _stunningObj.SetActive(false);
        }

        if (_specialActive)
        {
            _specialAttackTimer -= Time.deltaTime;

            if (_specialAttackTimer < 0)
            {
                DeactivateSpecial();
                _specialAttackTimer = _specialAttackTimerReset;
                _specialActive = false;
            }
        }
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

        if (_chargeAttackTime > 0 && _chargeButtonReleased)
        {
            UnleashChargeAttack();
        }
    }

    public override void Attack() {

        if (!_finisherReady)
        {
            if (_attackReady == true) {
            
                _comboTimer = _comboTimerReset;

                _audio.pitch = Random.Range(0.8f, 1.2f);

                // TURN ON FISTS!
                switch (_comboCounter)
                {
                    case 0:
                        _animator.Play("Attack 1");
                        _damage = _brawlerDamage;
                        _audio.PlayOneShot(_attack1_SFX, _audioVolume);
                        break;
                    case 1:
                        _animator.Play("Attack 2");
                        _damage = _brawlerDamage;
                        _audio.PlayOneShot(_attack2_SFX, _audioVolume);
                        break;
                    case 2:
                        _animator.Play("Attack 4");
                        _damage = _brawlerDamage * 2;
                        _audio.PlayOneShot(_attack3_SFX, _audioVolume);
                        _comboTimer = 1.2f;
                        _coolDown = 1.2f;
                        break;
                    default:
                        _comboCounter = 0;
                        _comboTimer = 0f;
                        break;
                }
                _comboCounter++;
            }
        }
        else {
            _animator.Play("Finisher");
            _finisherReady = false;
        }

        _attackReady = false;
    }

    public override void SecondaryAttack()
    {
        base.SecondaryAttack();
        

        _secondaryAttackActive = true;

        if (_attackReady == true && _manaPoints > 0)
        {
            _attackReady = false;

            _comboTimer = _comboTimerReset;

            _audio.pitch = Random.Range(0.8f, 1.2f);

            _manaPoints -= _attackManaCost;

            _manaRegen = _manaRegenTimerReset;

            _animator.Play("Attack 3");
            _audio.PlayOneShot(_attack3_SFX, _audioVolume);
            _comboTimer = 0.5f;

            _comboCounter++;
        }
    }

    public override void SpecialAttack()
    {
        base.SpecialAttack();
        _brawlerDamage += _addedDamage;
        _hitPoints += _addedHP;
        _hitPointMax += _addedHP;
        _specialActive = true;
        _beeSpecialScale = _addedScale;
    }

    public void DeactivateSpecial()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        _beeSpecialScale = 1;
        _brawlerDamage -= _addedDamage;
        _hitPoints = 150;
        _specialActive = false;
    }

    private void UnleashChargeAttack()
    {
        _stunningObj.SetActive(true);
    }

}
