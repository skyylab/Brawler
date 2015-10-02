using UnityEngine;
using System.Collections;

public class EnterEnemyAttackRangeSingleColor : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private EnemyScriptSingleColor _parentScript;

    void Start()
    {
        _parentScript = _parent.GetComponent<EnemyScriptSingleColor>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _parentScript.AddToAttackList(other.gameObject);
            _parentScript.StopSpeed();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _parentScript.RemoveFromAttackList(other.gameObject);
            _parentScript.StartSpeed();
        }
    }
}
