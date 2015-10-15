using UnityEngine;
using System.Collections;

public class SecondaryFireballProjectile : MonoBehaviour {

    private GameObject _parent;
    private MageClass _parentScript;

    private float _moveSpeed = 0.5f;
    private int _firingDirection = 1;
    private float _life = 2.0f;

    [SerializeField]
    private GameObject _fireballSprite;
    [SerializeField]
    private GameObject _fireballSpriteInner;
    [SerializeField]
    private GameObject _explosion;

    private int _objectsHit = 0;
    private int _maxObjectsHit = 3;

    private GameObject _pullLocations;

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

    public void Initialize(int Direction, GameObject parent, GameObject pullLocations)
    {
        _parent = parent;
        _parentScript = _parent.GetComponent<MageClass>();
        _firingDirection = Direction;
        _pullLocations = pullLocations;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.transform.gameObject.GetComponent<EnemyScript>().PullObject(_pullLocations);

            GameObject Explosion = Instantiate(_explosion, transform.position, transform.rotation) as GameObject;
            Explosion.GetComponent<MageExplosionScript>().Initialize(new Color(1f, 1f, 1f), "Blue", _parent);
            //Destroy(gameObject);

            _objectsHit++;

            if (_objectsHit > _maxObjectsHit) {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
