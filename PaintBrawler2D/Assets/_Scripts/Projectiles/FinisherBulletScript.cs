using UnityEngine;
using System.Collections;

public class FinisherBulletScript : Finisher {

    [SerializeField]
    private GameObject _particles;

    private float _moveSpeed = 0.5f;
    private int _firingDirection = 1;
    private float _life = 2.0f;

    [SerializeField]
    private GameObject _bulletHit;
    private GameObject _target;
    
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * 2000);

        _life -= Time.deltaTime;

        if (!GetComponent<AudioSource>().isPlaying) {
            Destroy(gameObject);
        }
	}

    public void Initialize(Color color, string colorName, int Direction, GameObject parent, GameObject target) {
        _parent = parent;
        _parentScript = _parent.GetComponent<HeroScript>();
        GetComponent<SpriteRenderer>().color = color;
        _firingDirection = Direction;
        _target = target;

        if (_firingDirection == 1)
        {
            transform.localEulerAngles += new Vector3(0f, 180f, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyScript>().AccumulateColor( 300,
                                                              _parentScript.GetPrimaryColorString(),
                                                              _parent);

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

        if (other.tag == "FinishingCollider") {

            GetComponent<SphereCollider>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            _particles.SetActive(false);
        }
    }
}
