using UnityEngine;
using System.Collections;

public class ExplosionBirthTimer : MonoBehaviour {

    [SerializeField]
    private float _deathTimer = 0.6f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private Color _explosionColor = new Color (1f, 1f, 1f);
    [SerializeField]
    private int _damage = 0;

	// Use this for initialization
	void Start () {
	
	}

    public void InitializeColor(Color ExplosionColor, int Damage)
    {
        _explosionColor = ExplosionColor;
        GetComponent<ParticleSystem>().startColor = _explosionColor;
        _damage += Damage;
    }
	
	// Update is called once per frame
	void Update () {
        _deathTimer -= Time.deltaTime;
         
        if (_deathTimer < 0)
        {
            GameObject ExplosionPrefab = Instantiate(_explosionPrefab, transform.position, transform.rotation) as GameObject;
            ExplosionPrefab.GetComponent<ColorExplosionDealDamage>().Initialize(_damage, _explosionColor);
            Destroy(gameObject);
        }
	}
}
