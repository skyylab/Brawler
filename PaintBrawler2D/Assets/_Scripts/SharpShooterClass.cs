using UnityEngine;
using System.Collections;

public class SharpShooterClass : HeroScript {

    // Class specific stats
    private int _sharpshooterDamage = 20;
    private int _sharpshooterArmor = 2;
    private float _sharpshooterMoveSpeed = 12f;
    private float _sharpshooterAttackSpeed = 0.25f;
    private float _sharpshooterManaRegen = 8f;

    public int GetDamage() { return _damage; }

    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private GameObject _muzzleFlash;
    [SerializeField]
    private GameObject _firingPoint;

    // Use this for initialization
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update () {
        ManageAttack();
	}

    private void ManageAttack() {
        _sharpshooterAttackSpeed -= Time.deltaTime;
        if (_sharpshooterAttackSpeed < 0) {
            _attackReady = true;
        }
        else {
            _attackReady = false;
        }
    }

    public override void Attack() {
        if (_attackReady == true) {
            GameObject BulletObj = Instantiate(_bullet, _firingPoint.transform.position, _firingPoint.transform.rotation) as GameObject;
            BulletObj.GetComponent<BulletScript>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);
            Instantiate(_muzzleFlash, _firingPoint.transform.position, _firingPoint.transform.rotation);

            _animator.Play("Shooting");
        }
    }

    public override void SecondaryAttack()
    {
        if (_attackReady == true)
        {
            GameObject BulletObj = Instantiate(_bullet, _firingPoint.transform.position, _firingPoint.transform.rotation) as GameObject;
            BulletObj.GetComponent<BulletScript>().Initialize(_secondaryColorArray[_playerNumber - 1], _secondaryColorString[_playerNumber - 1], _firingDirection, gameObject);
            Instantiate(_muzzleFlash, _firingPoint.transform.position, _firingPoint.transform.rotation);
            _animator.Play("Shooting");
        }
    }
}
