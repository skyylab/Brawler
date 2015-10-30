using UnityEngine;
using System.Collections;
using Giverspace;

public class FireballProjectile : MonoBehaviour {

    private GameObject _parent;
    private MageClass _parentScript;
    private Color _color;

    private int _damage;

    private float _moveSpeed = 0.25f;
    private int _firingDirection = 1;
    private float _life = 2.0f;
    private string _colorName = "";

    [SerializeField]
    private GameObject _sprite;

    [SerializeField]
    private GameObject _explosion;

    private int _objectsHit = 0;
    private int _maxObjectsHit = 3;

    // Use this for initialization
    void Awake()
    {
        _sprite.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * _moveSpeed * _firingDirection;

        _life -= Time.deltaTime;

        if (_life < 0)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Color color, string colorName, int Direction, GameObject parent, float size, int Damage)
    {
        _colorName = colorName;
        _parent = parent;
        _parentScript = _parent.GetComponent<MageClass>();
        _color = color;
        _firingDirection = Direction;

        if (_firingDirection == 1)
        {
            _sprite.transform.localEulerAngles += new Vector3(0f, 180f, 0f);
        }
        size = size / 1.5f;

        transform.localScale *= size;

        _sprite.GetComponent<SpriteRenderer>().enabled = true;

        _damage = Damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.transform.gameObject.GetComponent<EnemyScript>().AccumulateColor(_damage, 
                                                                                   _parentScript.GetPrimaryColorString(),
                                                                                   _parent);


            Log.Metrics.TotalDamageDealt(_damage, PlayerNumber.Mage);

            GameObject Explosion = Instantiate(_explosion, transform.position, transform.rotation) as GameObject;
            Explosion.GetComponent<MageExplosionScript>().Initialize(_color, _colorName, _parent);
            
            gameObject.GetComponent<SphereCollider>().enabled = false;
            
        }
    }
}
