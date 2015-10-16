using UnityEngine;
using System.Collections;

public class BulletEnemyScript : MonoBehaviour {

    private GameObject _parent;

    private float _moveSpeed = 0.15f;
    private int _firingDirection = 1;
    private float _life = 5.0f;
    private int _damage;

    [SerializeField]
    private GameObject _bulletHit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.left * _moveSpeed * _firingDirection;

        _life -= Time.deltaTime;

        if (!GetComponent<AudioSource>().isPlaying && _life < 0f) {
            Destroy(gameObject);
        }
	}

    public void Initialize(int Damage, int Direction) {
        _firingDirection = Direction;
        _damage = Damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<HeroScript>().TakeDamage(_damage);
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
