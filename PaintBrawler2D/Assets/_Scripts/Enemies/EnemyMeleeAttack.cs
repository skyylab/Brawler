using UnityEngine;
using System.Collections;

public class EnemyMeleeAttack : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private EnemyScript _parentScript;

    void Start() {
        _parentScript = _parent.GetComponent<EnemyScript>();
    }

	void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player" && !_parentScript.GetAttackLanded())
        {
            Debug.Log(other.tag);
            other.gameObject.GetComponent<HeroScript>().TakeDamage(_parentScript.GetDamage());
            _parentScript.SetAttackLanded(true);
        }
    }
}
