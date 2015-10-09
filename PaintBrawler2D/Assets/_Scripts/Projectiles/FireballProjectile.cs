using UnityEngine;
using System.Collections;

public class FireballProjectile : MonoBehaviour {

    private GameObject _parent;
    private MageClass _parentScript;
    private Color _color;

    private float _moveSpeed = 0.5f;
    private int _firingDirection = 1;
    private float _life = 2.0f;
    private string _colorName = "";

    [SerializeField]
    private GameObject _fireballSprite;
    [SerializeField]
    private GameObject _explosion;

    private int _objectsHit = 0;
    private int _maxObjectsHit = 3;

    // Use this for initialization
    void Start()
    {

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

    public void Initialize(Color color, string colorName, int Direction, GameObject parent)
    {
        _colorName = colorName;
        _parent = parent;
        _parentScript = _parent.GetComponent<MageClass>();
        _fireballSprite.GetComponent<ParticleSystem>().startColor = color;
        _color = color;
        _firingDirection = Direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.transform.gameObject.GetComponent<EnemyScript>().AccumulateColor(_parentScript.GetDamage(),
                                                                                              _parentScript.GetPrimaryColorString());
            GameObject Explosion = Instantiate(_explosion, transform.position, transform.rotation) as GameObject;
            Explosion.GetComponent<MageExplosionScript>().Initialize(_color, _colorName, _parent);
            //Destroy(gameObject);

            _objectsHit++;

            if (_objectsHit > _maxObjectsHit) {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
