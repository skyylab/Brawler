using UnityEngine;
using System.Collections;

public class BrawlerFistDamageCollider : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private BrawlerClass _parentScript;

    [SerializeField]
    private GameObject _powPrefab;

    void Start() {
        _parentScript = _parent.GetComponent<BrawlerClass>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            other.GetComponent<EnemyScript>().TakeDamage(_parentScript.GetBrawlerDamage(), 
                                                         _parentScript.GetPrimaryColorString(),
                                                         _parentScript.GetSecondaryColorString());

            Instantiate(_powPrefab, transform.position + transform.right, transform.rotation);
            
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
