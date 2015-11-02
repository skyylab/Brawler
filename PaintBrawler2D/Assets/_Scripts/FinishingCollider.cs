using UnityEngine;
using System.Collections;

public class FinishingCollider : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private EnemyScript _parentScript;


    private GameObject _target;
    private float _timerReset;

    void Start()
    {
        _parentScript = _parent.GetComponent<EnemyScript>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<HeroScript>().SetFinisher(true, _parent);
        }

        if (other.tag == "Finisher") {
            _parent.GetComponent<EnemyScript>().TakeFlatDamage(300, other.GetComponent<Finisher>().GetParentColor());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<HeroScript>().SetFinisher(false, null);
        }
    }
}
