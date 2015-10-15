using UnityEngine;
using System.Collections;

public class SharpShooterClass : HeroScript {

    // Class specific stats
    private int _sharpshooterDamage = 15;
    private int _sharpshooterArmor = 2;
    private float _sharpshooterMoveSpeed = 12f;
    private float _sharpshooterAttackSpeed = 0.25f;
    private float _attackSpeedReset = 0.25f;
    private float _sharpshooterManaRegen = 8f;

    public int GetDamage() { return _damage; }

    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private GameObject _secondaryBullet;
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
    }

    public override void Attack() {
        if (_attackReady == true) {

            if (_fireLeft)
            {
                GameObject BulletObj = Instantiate(_bullet, _firingPointLeft.transform.position, _firingPointLeft.transform.rotation) as GameObject;
                BulletObj.GetComponent<BulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);
                Instantiate(_muzzleFlash, _firingPointLeft.transform.position, _firingPointLeft.transform.rotation);

                _animator.Play("ShootLeft");
                _sharpshooterAttackSpeed = _attackSpeedReset;
                _fireLeft = false;
            }
            else
            {
                GameObject BulletObj = Instantiate(_bullet, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;
                BulletObj.GetComponent<BulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);
                Instantiate(_muzzleFlash, _firingPointRight.transform.position, _firingPointRight.transform.rotation);

                _animator.Play("ShootRight");
                _sharpshooterAttackSpeed = _attackSpeedReset;
                _fireLeft = true;
            }
        }
    }

    private void UnleashChargeAttack() {

        GameObject BulletObj = Instantiate(_bullet, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;
        BulletObj.GetComponent<BulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], -_firingDirection, gameObject);

        BulletObj = Instantiate(_bullet, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;
        BulletObj.GetComponent<BulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);
        Instantiate(_muzzleFlash, _firingPointRight.transform.position, _firingPointRight.transform.rotation);
        Instantiate(_muzzleFlash, _firingPointLeft.transform.position, _firingPointLeft.transform.rotation);

        _animator.Play("ShootLeft");
        _animator.Play("ShootRight");

        _sharpshooterAttackSpeed = _attackSpeedReset;
    }

    public override void SecondaryAttack()
    {
        if (_attackReady == true)
        {
            if (_fireLeft)
            {
                GameObject BulletObj = Instantiate(_secondaryBullet, _firingPointLeft.transform.position, _firingPointLeft.transform.rotation) as GameObject;
                BulletObj.GetComponent<SecondaryBulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);
                Instantiate(_secondaryMuzzleFlash, _firingPointLeft.transform.position, _firingPointLeft.transform.rotation);

                _animator.Play("ShootLeft");
                _sharpshooterAttackSpeed = _attackSpeedReset;
                _fireLeft = false;
            }
            else
            {
                GameObject BulletObj = Instantiate(_secondaryBullet, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;
                BulletObj.GetComponent<SecondaryBulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);
                Instantiate(_secondaryMuzzleFlash, _firingPointRight.transform.position, _firingPointRight.transform.rotation);

                _animator.Play("ShootRight");
                _sharpshooterAttackSpeed = _attackSpeedReset;
                _fireLeft = true;
            }
            _manaPoints -= _attackManaCost;
        }
    }

    public override void SpecialAttack()
    {
        base.SpecialAttack();
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
}
