using UnityEngine;
using System.Collections;

public class MageClass : HeroScript {


    private int _mageDamage = 15;
    private int _mageArmor = 0;
    private float _mageMoveSpeed = 10f;
    private float _mageAttackSpeed = 0f;
    private float _attackSpeedReset = 1.25f;
    private float _mageManaRegen = 15f;

    public int GetDamage() { return _damage; }

    [SerializeField]
    private GameObject _projectile;
    [SerializeField]
    private GameObject _muzzleFlash;
    [SerializeField]
    private GameObject _firingPointLeft;
    [SerializeField]
    private GameObject _firingPointRight;

    void Start () {
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
    void Update () {
        ManageAttack();
        ManageDeath();
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
    }

    public override void Attack()
    {
        if (_attackReady == true)
        {
            _animator.Play("AttackRight");
            GameObject BulletObj = Instantiate(_projectile, _firingPointRight.transform.position, _firingPointRight.transform.rotation) as GameObject;
            BulletObj.GetComponent<FireballProjectile>().Initialize(_primaryColorArray[_playerNumber - 1], _primaryColorString[_playerNumber - 1], _firingDirection, gameObject);
            //Instantiate(_muzzleFlash, _firingPoint.transform.position, _firingPoint.transform.rotation);

            _mageAttackSpeed = _attackSpeedReset;
        }
    }

    public override void SecondaryAttack()
    {
        if (_attackReady == true)
        {
            _animator.Play("AttackRight");
            GameObject BulletObj = Instantiate(_projectile, _firingPointLeft.transform.position, _firingPointLeft.transform.rotation) as GameObject;
            BulletObj.GetComponent<FireballProjectile>().Initialize(_secondaryColorArray[_playerNumber - 1], _secondaryColorString[_playerNumber - 1], _firingDirection, gameObject);
            //Instantiate(_muzzleFlash, _firingPoint.transform.position, _firingPoint.transform.rotation);
            
            _mageAttackSpeed = _attackSpeedReset;
        }
    }
}
