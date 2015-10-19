using UnityEngine;
using System.Collections;

public class SecondaryBulletScript : MonoBehaviour {

    private GameObject _parent;
    private SharpShooterClass _parentScript;

    [SerializeField]
    private GameObject _particles;
    [SerializeField]
    private GameObject _particles2;

    private float _moveSpeed = 0.5f;
    private int _firingDirection = 1;
    private float _life = 2.0f;

    [SerializeField]
    private GameObject _bulletHit;
    
    private string _colorName;
	
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
        _colorName = colorName;

        if (_firingDirection == 1)
        {
            transform.localEulerAngles += new Vector3(0f, 180f, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (other.GetComponent<EnemyScript>().GetColorString() == _colorName ||
                other.GetComponent<EnemyScript>().GetColorString() == "" ||
                other.GetComponent<EnemyScript>().GetColorString() == "Purple" ||
                other.GetComponent<EnemyScript>().GetColorString() == "Orange")
            { 
                other.GetComponent<EnemyScript>().AccumulateColor(_parentScript.GetDamage(),
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
                _particles2.SetActive(false);
            }
        }
    }
}
