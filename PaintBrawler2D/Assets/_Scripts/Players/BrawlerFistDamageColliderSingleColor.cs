using UnityEngine;
using System.Collections;

public class BrawlerFistDamageColliderSingleColor : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private BrawlerClassSingleColor _parentScript;

    [SerializeField]
    private GameObject _powPrefab;

    void Start() {
        _parentScript = _parent.GetComponent<BrawlerClassSingleColor>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            other.GetComponent<EnemyScriptSingleColor>().AccumulateColor(_parentScript.GetBrawlerDamage(),
                                                                         _parentScript.GetPrimaryColorString());

            Instantiate(_powPrefab, transform.position + transform.right, transform.rotation);
            _parentScript.AttackRegen();
            
            if (_parentScript.GetCharacterObj().transform.localEulerAngles.y != 0)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(_parent.transform.right * -2000f);
            }
            else { 
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(_parent.transform.right * 2000f);
            }
        }
    }

}
