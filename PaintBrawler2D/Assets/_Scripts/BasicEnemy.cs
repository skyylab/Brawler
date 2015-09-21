using UnityEngine;
using System.Collections;

public class BasicEnemy : EnemyScript {
    
    void Start()
    {
        InitializeClass();
    }

    void Update()
    {
        ManageMovement();
        ManageAttack();

        if (_hitPoints < 0)
        {
            // Die
            Destroy(gameObject);
        }
    }

    public void ManageAttack() {

        _coolDown -= Time.deltaTime;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            _objectSprites[1].GetComponent<BoxCollider2D>().enabled = false;
            _objectSprites[2].GetComponent<BoxCollider2D>().enabled = false;
        }

        if (_inAttackRange && _coolDown < 0)
        {
            Attack();
        }
    }

    public override void Attack()
    {
        _attackLanded = false;
        _animator.CrossFade("Attack", 0.01f);
        _objectSprites[1].GetComponent<BoxCollider2D>().enabled = true;
        _objectSprites[2].GetComponent<BoxCollider2D>().enabled = true;
        _coolDown = _coolDownSet;
    }
}
