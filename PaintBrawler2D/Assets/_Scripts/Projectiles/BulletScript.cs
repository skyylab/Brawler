using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    private GameObject _parent;
    private SharpShooterClass _parentScript;

    [SerializeField]
    private GameObject _particles;

    private float _moveSpeed = 0.5f;
    private int _firingDirection = 1;
    private float _life = 2.0f;

    [SerializeField]
    private GameObject _bulletHit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.left * _moveSpeed * _firingDirection;

        _life -= Time.deltaTime;

        if (!GetComponent<AudioSource>().isPlaying) {
            Destroy(gameObject);
        }
	}

    public void Initialize(Color color, string colorName, int Direction, GameObject parent) {
        _parent = parent;
        _parentScript = _parent.GetComponent<SharpShooterClass>();
        GetComponent<SpriteRenderer>().color = color;
        _firingDirection = Direction;

        if (_firingDirection == 1)
        {
            transform.localEulerAngles += new Vector3(0f, 180f, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyScript>().AccumulateColor(_parentScript.GetDamage(),
                                                                         _parentScript.GetPrimaryColorString());
            Instantiate(_bulletHit, transform.position, transform.rotation);

            if (transform.localEulerAngles.y != 0)
            {
                other.gameObject.GetComponent<Rigidbody>().AddForce(_parent.transform.right * -2000f);
            }
            else
            {
                other.gameObject.GetComponent<Rigidbody>().AddForce(_parent.transform.right * 2000f);
            }

            GetComponent<SphereCollider>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            _particles.SetActive(false);
        }
    }
}
