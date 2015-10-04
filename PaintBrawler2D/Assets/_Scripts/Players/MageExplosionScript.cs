using UnityEngine;
using System.Collections;

public class MageExplosionScript : MonoBehaviour {

    private GameObject _parent;
    private MageClass _parentScript;
    private string _colorName = "";

    private float _maxSize = 2.5f;
    private float _growSpeed = 0.15f;

    [SerializeField]
    private GameObject _fireballSprite;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale += new Vector3(1f, 1f, 1f) * _growSpeed;	 
        if (transform.localScale.x > _maxSize) {
            GetComponent<CircleCollider2D>().enabled = false;
            _fireballSprite.GetComponent<SpriteRenderer>().enabled = false;
            if (!GetComponent<AudioSource>().isPlaying) { 
                Destroy(gameObject);
            }
        }   
	}

    public void Initialize(Color color, string colorName, GameObject parent)
    {
        _colorName = colorName;
        _parent = parent;
        _parentScript = _parent.GetComponent<MageClass>();
        _fireballSprite.GetComponent<SpriteRenderer>().color = color;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.transform.gameObject.GetComponent<EnemyScript>().TakeSplashDamage(_parentScript.GetDamage()/2);
        }
    }
}
