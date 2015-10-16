using UnityEngine;
using System.Collections;

public class MageExplosionScript : MonoBehaviour {

    private GameObject _parent;
    private MageClass _parentScript;

    private float _maxSize = 2.5f;
    private float _growSpeed = 0.15f;

    [SerializeField]
    private GameObject _particle1;
    [SerializeField]
    private GameObject _particle2;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale += new Vector3(1f, 1f, 1f) * _growSpeed;	 
        if (transform.localScale.x > _maxSize) {
            GetComponent<SphereCollider>().enabled = false;
            _particle1.GetComponent<ParticleSystem>().emissionRate = 0f;
            _particle2.GetComponent<ParticleSystem>().emissionRate = 0f;

            if (!GetComponent<AudioSource>().isPlaying) { 
                Destroy(gameObject);
            }
        }   
	}

    public void Initialize(Color color, string colorName, GameObject parent)
    {
        _parent = parent;
        _parentScript = _parent.GetComponent<MageClass>();
       _particle1.GetComponent<ParticleSystem>().startColor = color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.transform.gameObject.GetComponent<EnemyScript>().TakeSplashDamage(_parentScript.GetDamage()/2);
        }
    }
}
