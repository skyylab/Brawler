using UnityEngine;
using System.Collections;
using Giverspace;

public class FinisherFireballProjectile : MonoBehaviour {

    private GameObject _parent;
    private MageClass _parentScript;
    private Color _color;

    private int _damage;

    private float _moveSpeed = 0.25f;
    private int _firingDirection = 1;
    private float _life = 5.0f;
    private string _colorName = "";

    [SerializeField]
    private GameObject _sprite;

    [SerializeField]
    private GameObject _explosion;
    private GameObject _target;

    private int _objectsHit = 0;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * 2000);

        _life -= Time.deltaTime;

        if (_life < 0)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(int Direction, GameObject parent, float size, int Damage, GameObject target)
    {
        _parent = parent;
        _parentScript = _parent.GetComponent<MageClass>();
        _target = target;

        transform.localScale *= size;

        _damage = Damage;

        if (_firingDirection == 1)
        {
            _sprite.transform.localEulerAngles += new Vector3(0f, 180f, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && other.gameObject == _target)
        {
            //other.GetComponent<EnemyScript>().AccumulateColor(300,
            //                                                  _parentScript.GetPrimaryColorString(),
            //                                                  _parent);

            other.GetComponent<EnemyScript>().TakeFlatDamage(300, _parentScript.GetPrimaryColorString());
            Log.Metrics.TotalDamageDealt(_parentScript.GetDamage(), PlayerNumber.Mage);

            _sprite.SetActive(false);

            GameObject Explosion = Instantiate(_explosion, transform.position, transform.rotation) as GameObject;
            Explosion.GetComponent<MageExplosionScript>().Initialize(_parentScript.GetPrimaryColor(), _parentScript.GetPrimaryColorString(), _parent);
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
