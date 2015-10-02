using UnityEngine;
using System.Collections;

public class EnterEnemyCircleRangeSingleColor : MonoBehaviour {

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
            if (_parentScript._currentlyAttacking == false){
                _parentScript.ReachedCirclingDistance();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_parentScript._currentlyAttacking == false){
                _parentScript.MovedOutCirclingDistance();
            }
        }
    }
}
