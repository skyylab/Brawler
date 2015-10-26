using UnityEngine;
using System.Collections;

public class MageClass : HeroScript {

    private int _mageDamage = 20;
    private int _mageArmor = 0;
    private float _mageMoveSpeed = 10f;
    private float _mageAttackSpeed = 0f;
    private float _attackSpeedReset = 1.25f;
    private float _mageManaRegen = 15f;

    public int GetDamage() { return _damage; }

    [SerializeField]
    private GameObject _projectile;
    [SerializeField]
    private GameObject _secondaryProjectile;
    [SerializeField]
    private GameObject _finisherProjectile;
    [SerializeField]
    private GameObject _muzzleFlash;
    [SerializeField]
    private GameObject _firingPointLeft;
    [SerializeField]
    private GameObject _firingPointRight;

    [SerializeField]
    private GameObject _leftHandFire;
    [SerializeField]
    private GameObject _rightHandFire;

    [SerializeField]
    private GameObject _drawObjects;

    public override void Start ()
    {
        base.Start();
        InitializeClass(_playerNumber);
        InitializeStats();

	}

    public void InitializeStats()
    {
        _damage = _mageDamage;
        _armor = _mageArmor;
        _moveSpeed = _mageMoveSpeed;
        _attackSpeed = _mageAttackSpeed;
        _manaRegen = _mageManaRegen;
        _animator = _characterObj.GetComponent<Animator>();
    }

    // Update is called once per frame
    public override void Update () {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage")) { 
            base.Update();
            ManageAttack();
            ManageDeath();
        }
    }

    private void ManageAttack()
    {
        _mageAttackSpeed -= Time.deltaTime;
        if (_mageAttackSpeed < 0)
        {
            _attackReady = true;
        }
        else
        {
            _attackReady = false;
        }

        if (_chargeAttackTime > 0 && _chargeButtonReleased && _mageAttackSpeed < -1)
        {
            UnleashChargeAttack();
        }
    }

    public override void Attack()
    {
        if (!_finisherReady) { 
            if (_attackReady == true)
            {
                _animator.Play("AttackRight");
                GameObject BulletObj = Instantiate(_projectile, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;
                BulletObj.GetComponent<FireballProjectile>().Initialize(_primaryColorArray[_playerNumber - 1],
                                                                        _primaryColorString[_playerNumber - 1],
                                                                        _firingDirection,
                                                                        gameObject,
                                                                        1f + _chargeAttackTime,
                                                                        _damage);
                //Instantiate(_muzzleFlash, _firingPoint.transform.position, _firingPoint.transform.rotation);

                _mageAttackSpeed = _attackSpeedReset;
            }
        }
        else {
            _animator.Play("AttackRight");
            GameObject BulletObj = Instantiate(_finisherProjectile, _finisherObj.transform.position, _finisherObj.transform.rotation) as GameObject;
            BulletObj.GetComponent<FinisherFireballProjectile>().Initialize(_firingDirection,
                                                                            gameObject,
                                                                            1f + _chargeAttackTime,
                                                                            300,
                                                                            _finisherObj);

            _mageAttackSpeed = _attackSpeedReset;
            _finisherReady = false;
        }
    }

    public override void SecondaryAttack()
    {
        if (_attackReady == true && _manaPoints > 0)
        {
            _animator.Play("AttackRight");
            GameObject BulletObj = Instantiate(_secondaryProjectile, _firingPointLeft.transform.position, _firingPointRight.transform.rotation) as GameObject;
            BulletObj.GetComponent<SecondaryFireballProjectile>().Initialize(_firingDirection, gameObject, _drawObjects);
            //Instantiate(_muzzleFlash, _firingPoint.transform.position, _firingPoint.transform.rotation);

            _mageAttackSpeed = _attackSpeedReset;
            _manaPoints -= _attackManaCost;
        }
    }

    public override void SpecialAttack()
    {
        base.SpecialAttack();
        _specialAttackCooldown = 0f;
        GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject x in AllEnemies) {
            GameObject BulletObj = Instantiate(_secondaryProjectile, x.transform.position, x.transform.rotation) as GameObject;
            BulletObj.GetComponent<SecondaryFireballProjectile>().Initialize(_firingDirection, gameObject, _drawObjects);
            x.GetComponent<EnemyScript>().TakeFlatDamage(_damage);
        }
    }


    public override void PlayWalkAnim()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackRight") &&
            !_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") &&
            !_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") &&
            _hitPoints > 0)
        {
            _animator.Play("Walk");
        }
    }

    public override void PlayIdleAnim()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackRight") &&
           !_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") &&
           !_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") &&
           _hitPoints > 0)
        {
            _animator.Play("Idle");
        }
    }

    private void UnleashChargeAttack()
    {
        GameObject BulletObj = Instantiate(_projectile, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;

        BulletObj.GetComponent<FireballProjectile>().Initialize(_primaryColorArray[_playerNumber - 1], 
                                                                _primaryColorString[_playerNumber - 1], 
                                                                _firingDirection, 
                                                                gameObject, 
                                                                1f + _chargeAttackTime, 
                                                                _damage * ((int)_chargeAttackTime + 2));
        _animator.Play("AttackRight");

        _mageAttackSpeed = _attackSpeedReset;
    }

}
