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
        _fireballSprite.GetComponent<SpriteRenderer>().color = color;
        _color = color;
        _firingDirection = Direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.transform.gameObject.GetComponent<EnemyScript>().TakeDamage(_parentScript.GetDamage(),
                                                                              _parentScript.GetPrimaryColorString(),
                                                                              _parentScript.GetSecondaryColorString());
            GameObject Explosion = Instantiate(_explosion, transform.position, transform.rotation) as GameObject;
            Explosion.GetComponent<MageExplosionScript>().Initialize(_color, _colorName, _parent);
            Destroy(gameObject);
        }
    }
}
