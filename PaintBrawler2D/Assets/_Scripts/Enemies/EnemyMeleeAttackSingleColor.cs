using UnityEngine;
using System.Collections;

public class EnemyMeleeAttackSingleColor : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private EnemyScriptSingleColor _parentScript;

    void Start() {
        _parentScript = _parent.GetComponent<EnemyScriptSingleColor>();
    }

	void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player" && !_parentScript.GetAttackLanded()) {
            other.gameObject.GetComponent<HeroScriptSingleColor>().TakeDamage(_parentScript.GetDamage());
            _parentScript.SetAttackLanded(true);
        }
    }
}
