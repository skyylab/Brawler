using UnityEngine;
using System.Collections;

public class SharpShooterClass : HeroScript {

    // Class specific stats
    private int _sharpshooterDamage = 12;
    private int _sharpshooterArmor = 0;
    private float _sharpshooterMoveSpeed = 12f;
    private float _sharpshooterAttackSpeed = 0.35f;
    private float _attackSpeedReset = 0.25f;
    private float _sharpshooterManaRegen = 8f;

    public int GetDamage() { return _damage; }

    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private GameObject _secondaryBullet;
    [SerializeField]
    private GameObject _finisherBullet;
    [SerializeField]
    private GameObject _muzzleFlash;
    [SerializeField]
    private GameObject _secondaryMuzzleFlash;
    [SerializeField]
    private GameObject _firingPointLeft;
    [SerializeField]
    private GameObject _firingPointRight;

    [SerializeField]
    private float _specialAttackTimer = 5f;
    private float _specialAttackTimerReset;
    [SerializeField]
    private float _specialAttackSpeed = 0.15f;
    private float _specialAttackSpeedReset;

    private bool _fireLeft = true;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        InitializeClass(_playerNumber);
        InitializeStats();
    }

    public void InitializeStats()
    {
        _damage = _sharpshooterDamage;
        _armor = _sharpshooterArmor;
        _moveSpeed = _sharpshooterMoveSpeed;
        _attackSpeed = _sharpshooterAttackSpeed;
        _manaRegen = _sharpshooterManaRegen;
        _animator = _characterObj.GetComponent<Animator>();
        _specialAttackTimerReset = _specialAttackTimer;
        _specialAttackTimer = 0f;
        _specialAttackSpeedReset = _specialAttackSpeed;
        _sharpshooterMoveSpeed = GetComponent<PlayerMovement>().moveSpeed;
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
        ManageAttack();
        ManageDeath();
    }

    private void ManageAttack() {
        _sharpshooterAttackSpeed -= Time.deltaTime;
        _specialAttackSpeed -= Time.deltaTime;
        _specialAttackTimer -= Time.deltaTime;
        if (_sharpshooterAttackSpeed < 0) {
            _attackReady = true;
        }
        else {
            _attackReady = false;
        }

        if (_chargeAttackTime > 0 && _chargeButtonReleased && _sharpshooterAttackSpeed < 0) {
            UnleashChargeAttack();
        }

        if (_specialAttackTimer > 0 && _specialAttackSpeed < 0) {
            SpecialAttackSingleFire();
        }

        if (_specialAttackTimer < 0) {
            _specialActive = false;
        }

        if ((_animator.GetCurrentAnimatorStateInfo(0).IsName("ShootLeft") ||
            _animator.GetCurrentAnimatorStateInfo(0).IsName("ShootRight")) &&
            _specialActive == false && _chargeAttackTime < 0) {
            GetComponent<PlayerMovement>().moveSpeed = _sharpshooterMoveSpeed/3;
        }
        else
        {
            GetComponent<PlayerMovement>().moveSpeed = _sharpshooterMoveSpeed;
        }
    }

    public override void Attack() {
        if (!_finisherReady) {
            if (_attackReady == true) {

                if (_fireLeft)
                {
                    _animator.Play("ShootLeft");
                    GameObject BulletObj = Instantiate(_bullet, _firingPointLeft.transform.position, _firingPointLeft.transform.rotation) as GameObject;
                    BulletObj.GetComponent<BulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);
                    Instantiate(_muzzleFlash, _firingPointLeft.transform.position, _firingPointLeft.transform.rotation);

                    _sharpshooterAttackSpeed = _attackSpeedReset;
                    _fireLeft = false;
                }
                else
                {
                    _animator.Play("ShootRight");
                    GameObject BulletObj = Instantiate(_bullet, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;
                    BulletObj.GetComponent<BulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);
                    Instantiate(_muzzleFlash, _firingPointRight.transform.position, _firingPointRight.transform.rotation);

                    _sharpshooterAttackSpeed = _attackSpeedReset;
                    _fireLeft = true;
                }
            }
        }
        else {
            _animator.Play("ShootRight");

            GameObject BulletObj = Instantiate(_finisherBullet, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;
            BulletObj.GetComponent<FinisherBulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject, _finisherObj);
            Instantiate(_muzzleFlash, _firingPointRight.transform.position, _firingPointRight.transform.rotation);

            _sharpshooterAttackSpeed = _attackSpeedReset;
            _finisherReady = false;
        }
    }


    public override void PlayIdleAnim()
    {
        base.PlayIdleAnim();
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("ShootLeft") &&
            !_animator.GetCurrentAnimatorStateInfo(0).IsName("ShootRight") &&
            !_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") &&
            !_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") &&
            _hitPoints > 0) {
            _animator.Play("Idle");
        }
    }
    public override void PlayWalkAnim()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("ShootLeft") &&
            !_animator.GetCurrentAnimatorStateInfo(0).IsName("ShootRight") &&
            !_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") &&
            !_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") &&
            _hitPoints > 0)
        {
            _animator.Play("RunFox");
        }
    }

    private void UnleashChargeAttack() {
        _animator.Play("ShootLeft");
        _animator.Play("ShootRight");

        GameObject BulletObj = Instantiate(_bullet, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;
        BulletObj.GetComponent<BulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], -_firingDirection, gameObject);

        BulletObj = Instantiate(_bullet, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;
        BulletObj.GetComponent<BulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);
        Instantiate(_muzzleFlash, _firingPointRight.transform.position, _firingPointRight.transform.rotation);
        Instantiate(_muzzleFlash, _firingPointLeft.transform.position, _firingPointLeft.transform.rotation);

        _sharpshooterAttackSpeed = _attackSpeedReset;
    }

    public override void SecondaryAttack()
    {
        if (_attackReady == true)
        {
            //if (_fireLeft)
            //{
            //    _animator.Play("ShootLeft");
            //    GameObject BulletObj = Instantiate(_secondaryBullet, _firingPointLeft.transform.position, _firingPointLeft.transform.rotation) as GameObject;
            //    BulletObj.GetComponent<SecondaryBulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);
            //    Instantiate(_secondaryMuzzleFlash, _firingPointLeft.transform.position, _firingPointLeft.transform.rotation);

            //    _sharpshooterAttackSpeed = _attackSpeedReset;
            //    _fireLeft = false;
            //}
            //else
            //{
            //    _animator.Play("ShootRight");
            //    GameObject BulletObj = Instantiate(_secondaryBullet, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;
            //    BulletObj.GetComponent<SecondaryBulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);
            //    Instantiate(_secondaryMuzzleFlash, _firingPointRight.transform.position, _firingPointRight.transform.rotation);

            //    _sharpshooterAttackSpeed = _attackSpeedReset;
            //    _fireLeft = true;
            //}

            if (_manaPoints > _attackManaCost) { 
                _specialActive = true;
                _specialAttackTimer = _specialAttackTimerReset;

                _manaPoints -= _attackManaCost;
            }
            _sharpshooterAttackSpeed = _attackSpeedReset;
        }
    }

    public override void SpecialAttack()
    {
        base.SpecialAttack();
        _specialActive = true;
        _specialAttackTimer = _specialAttackTimerReset;
    }

    private void SpecialAttackSingleFire() {
        GameObject BulletObj = Instantiate(_secondaryBullet, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;
        BulletObj.GetComponent<SecondaryBulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);

        BulletObj = Instantiate(_secondaryBullet, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;
        BulletObj.GetComponent<SecondaryBulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);
        Instantiate(_secondaryMuzzleFlash, _firingPointRight.transform.position, _firingPointRight.transform.rotation);
        Instantiate(_secondaryMuzzleFlash, _firingPointLeft.transform.position, _firingPointLeft.transform.rotation);

        _animator.Play("ShootLeft");
        _animator.Play("ShootRight");

        _specialAttackSpeed = _specialAttackSpeedReset;
    }


    public override void AddDamage()
    {
        _damage += 1;
    }

    public override void AddSpeed() {
        base.AddSpeed();
        _sharpshooterMoveSpeed += 0.01f;
    }
}
