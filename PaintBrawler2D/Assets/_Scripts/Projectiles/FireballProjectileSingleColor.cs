using UnityEngine;
using System.Collections;

public class FireballProjectileSingleColor : MonoBehaviour {

    private GameObject _parent;
    private MageClassSingleColor _parentScript;
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
        _parentScript = _parent.GetComponent<MageClassSingleColor>();
        _fireballSprite.GetComponent<SpriteRenderer>().color = color;
        _color = color;
        _firingDirection = Direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.transform.gameObject.GetComponent<EnemyScriptSingleColor>().AccumulateColor(_parentScript.GetDamage(),
                                                                                              _parentScript.GetPrimaryColorString());
            GameObject Explosion = Instantiate(_explosion, transform.position, transform.rotation) as GameObject;
            Explosion.GetComponent<MageExplosionScriptSingleColor>().Initialize(_color, _colorName, _parent);
            Destroy(gameObject);
        }
    }
}
